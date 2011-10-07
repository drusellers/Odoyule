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
        void Select<TSelect>(Action<ActivationNode<TSelect>> callback)
            where TSelect : class;

        Activation<TSelect> Node<TSelect>() 
            where TSelect : class;
    }

    public class ConditionAlphaNode<T, TDiscard> :
        ConditionAlphaNode
        where T : class
    {
        RuntimeConfigurator _configurator;
        LeftJoinNode<T, TDiscard> _node;

        public ConditionAlphaNode(RuntimeConfigurator configurator)
        {
            _configurator = configurator;
            _node = configurator.Left<T, TDiscard>(configurator.Constant<T>());
        }

        public void Select<TSelect>(Action<ActivationNode<TSelect>> callback)
            where TSelect : class
        {
            var node = _node as ActivationNode<TSelect>;
            if (node != null)
            {
                callback(node);
                return;
            }
        }

        public Activation<TSelect> Node<TSelect>() where TSelect : class
        {
            var result = _node as Activation<TSelect>;
            if(result == null)
                throw new RulesEngineException("Unable to cast " + typeof (T) + " to " + typeof (TSelect));

            return result;
        }
    }


    public class ConditionAlphaNode<T> :
        ConditionAlphaNode
        where T : class
    {
        readonly RuntimeConfigurator _configurator;
        readonly AlphaNode<T> _node;
        ConditionAlphaNode _parent;

        public ConditionAlphaNode(RuntimeConfigurator configurator, AlphaNode<T> node)
        {
            _node = node;
            _configurator = configurator;
        }

        public void Select<TSelect>(Action<ActivationNode<TSelect>> callback)
            where TSelect : class
        {
            var node = _node as ActivationNode<TSelect>;
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
                    _configurator);
            }

            _parent.Select<TSelect>(x =>
                {
                    _node.AddActivation(_parent.Node<T>());

                    callback(x);
                });
        }

        public void AddLeftJoin<TOutput, TDiscard>(AlphaNode<Token<TOutput, TDiscard>> previousNode)
            where TOutput : class
        {
            ConstantNode<TOutput> constantNode = _configurator.Constant<TOutput>();
            LeftJoinNode<TOutput, TDiscard> left = _configurator.Left<TOutput, TDiscard>(constantNode);

            var self = this as ConditionAlphaNode<TOutput>;
            if (self == null)
                throw new InvalidOperationException("I'm stupid, but it should always work");

            left.AddActivation(self._node);

            Activation<Token<TOutput, TDiscard>> x = left;

            previousNode.AddActivation(x);
        }

        public Activation<TSelect> Node<TSelect>() where TSelect : class
        {
            var result = _node as Activation<TSelect>;
            if (result == null)
                throw new RulesEngineException("Unable to cast " + typeof(T) + " to " + typeof(TSelect));

            return result;
        }
    }
}