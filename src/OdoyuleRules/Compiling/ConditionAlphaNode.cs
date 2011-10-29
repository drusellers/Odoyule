// Copyright 2011 Chris Patterson
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace OdoyuleRules.Compiling
{
    using System;
    using Configuration.RulesEngineConfigurators;
    using Models.RuntimeModel;

    public interface ConditionAlphaNode
    {
        void Select<TSelect>(Action<MemoryNode<TSelect>> callback)
            where TSelect : class;
    }

    public interface ReverseConditionAlphaNode
    {
        void MatchLeftJoinNode<TT, TTDiscard>(Action<LeftJoinNode<TT, TTDiscard>> callback)
            where TT : class;
    }

    public class ConditionAlphaNode<T> :
        ConditionAlphaNode,
        ReverseConditionAlphaNode
        where T : class
    {
        readonly RuntimeConfigurator _configurator;
        readonly AlphaNode<T> _node;
        bool _add = true;
        ConditionAlphaNode _parent;

        public ConditionAlphaNode(RuntimeConfigurator configurator, AlphaNode<T> node)
        {
            _node = node;
            _configurator = configurator;
        }

        public void Select<TSelect>(Action<MemoryNode<TSelect>> callback)
            where TSelect : class
        {
            var node = _node as MemoryNode<TSelect>;
            if (node != null)
            {
                callback(node);
                return;
            }

            if (_parent == null)
            {
                if (!typeof (T).IsGenericType || typeof (T).GetGenericTypeDefinition() != typeof (Token<,>))
                    throw new RulesEngineException("Unable to map " + typeof (T) + " to " + typeof (TSelect));

                Type[] arguments = typeof (T).GetGenericArguments();

                _parent = (ConditionAlphaNode) Activator.CreateInstance(
                    typeof (ConditionAlphaNode<,>).MakeGenericType(arguments),
                    _configurator, this);
            }

            _parent.Select(callback);
        }

        public void MatchLeftJoinNode<TT, TTDiscard>(Action<LeftJoinNode<TT, TTDiscard>> callback)
            where TT : class
        {
            var self = this as ConditionAlphaNode<Token<TT, TTDiscard>>;
            if (self != null)
            {
                _configurator.MatchLeftJoinNode(self._node, callback);
            }
        }
    }

    public class ConditionAlphaNode<T, TDiscard> :
        ConditionAlphaNode,
        ReverseConditionAlphaNode
        where T : class
    {
        readonly ReverseConditionAlphaNode _reverse;
        RuntimeConfigurator _configurator;
        LeftJoinNode<T, TDiscard> _node;
        ConditionAlphaNode _parent;

        public ConditionAlphaNode(RuntimeConfigurator configurator, ReverseConditionAlphaNode reverse)
        {
            _configurator = configurator;
            _reverse = reverse;
        }

        public void Select<TSelect>(Action<MemoryNode<TSelect>> callback)
            where TSelect : class
        {
            if (typeof (TSelect) == typeof (T) && _node == null)
            {
                _reverse.MatchLeftJoinNode<T, TDiscard>(n => _node = n);
            }

            var node = _node as MemoryNode<TSelect>;
            if (node != null)
            {
                callback(node);
                return;
            }

            if (_parent == null)
            {
                if (!typeof (T).IsGenericType || typeof (T).GetGenericTypeDefinition() != typeof (Token<,>))
                    throw new RulesEngineException("Unable to map " + typeof (T) + " to " + typeof (TSelect));

                Type[] arguments = typeof (T).GetGenericArguments();

                _parent = (ConditionAlphaNode) Activator.CreateInstance(
                    typeof (ConditionAlphaNode<,>).MakeGenericType(arguments),
                    _configurator, this);
            }

            _parent.Select<TSelect>(callback);
        }

        public void MatchLeftJoinNode<TT, TTDiscard>(Action<LeftJoinNode<TT, TTDiscard>> callback)
            where TT : class
        {
            var self = this as ConditionAlphaNode<Token<TT, TTDiscard>, TDiscard>;
            if (self != null)
            {
                if (_node == null)
                {
                    _reverse.MatchLeftJoinNode<T, TDiscard>(leftJoin => { _node = leftJoin; });
                }

                _configurator.MatchLeftJoinNode(self._node, callback);
            }
        }
    }
}