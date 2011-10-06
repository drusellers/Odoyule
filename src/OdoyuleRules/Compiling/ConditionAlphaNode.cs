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
        void Select<T>(Action<AlphaNode<T>> callback)
            where T : class;

        void AddLeftJoin<TOutput,TDiscard>(AlphaNode<Token<TOutput, TDiscard>> previousNode) 
            where TOutput : class;
    }

    public class ConditionAlphaNode<T> :
        ConditionAlphaNode
        where T : class
    {
        AlphaNode<T> _node;
        readonly RuntimeConfigurator _configurator;
        ConditionAlphaNode _parent;

        public ConditionAlphaNode(RuntimeConfigurator configurator)
        {
            _configurator = configurator;
            _node = _configurator.CreateNode(id => new AlphaNode<T>(id));
        }

        public ConditionAlphaNode(RuntimeConfigurator configurator, AlphaNode<T> node)
        {
            _node = node;
            _configurator = configurator;
        }

        public void Select<TSelect>(Action<AlphaNode<TSelect>> callback)
            where TSelect : class
        {
            var self = this as ConditionAlphaNode<TSelect>;
            if (self != null)
            {
                callback(self._node);
                return;
            }

            SearchTokenChain(callback);
        }

        public void SearchTokenChain<TSelect>(Action<AlphaNode<TSelect>> callback) 
            where TSelect : class
        {
            if (!typeof (T).IsGenericType || typeof (T).GetGenericTypeDefinition() != typeof (Token<,>)) 
                return;

            if (_parent != null)
            {
                _parent.Select(callback);
                return;
            }
            
            Type[] arguments = typeof (T).GetGenericArguments();
            Type parentType = arguments[0];

            var parent =
                (ConditionAlphaNode)
                Activator.CreateInstance(typeof (ConditionAlphaNode<>).MakeGenericType(parentType),
                                         _configurator);

            parent.Select<TSelect>(alphaNode =>
                {
                    typeof (ConditionAlphaNode).GetMethod("AddLeftJoin")
                        .MakeGenericMethod(arguments)
                        .Invoke(parent, new[]{_node});

                    _parent = parent;

                    callback(alphaNode);
                });
        }

        public void AddLeftJoin<TOutput,TDiscard>(AlphaNode<Token<TOutput, TDiscard>> previousNode) 
            where TOutput : class
        {
            var constantNode = _configurator.Constant<TOutput>();
            LeftJoinNode<TOutput, TDiscard> left = _configurator.Left<TOutput, TDiscard>(constantNode);

            var self = this as ConditionAlphaNode<TOutput>;
            if(self == null)
                throw new InvalidOperationException("I'm stupid, but it should always work");

            left.AddActivation(self._node);

            Activation<Token<TOutput,TDiscard>> x = left;

               previousNode.AddActivation(x);
        }
    }
}