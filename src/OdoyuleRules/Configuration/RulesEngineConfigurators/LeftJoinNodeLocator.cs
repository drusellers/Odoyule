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
namespace OdoyuleRules.Configuration.RulesEngineConfigurators
{
    using System;
    using System.Linq;
    using Models.RuntimeModel;

    public class LeftJoinNodeLocator<T, TDiscard> :
        RuntimeModelVisitorImpl
        where T : class
    {
        readonly RuntimeConfigurator _configurator;
        readonly MemoryNode<Token<T, TDiscard>> _left;
        readonly Func<RightActivation<T>, bool> _matchRight;
        readonly Func<RightActivation<T>> _rightActivation;
        LeftJoinNode<T, TDiscard> _node;

        public LeftJoinNodeLocator(RuntimeConfigurator runtimeConfigurator, MemoryNode<Token<T, TDiscard>> left)
        {
            _configurator = runtimeConfigurator;
            _left = left;
            _matchRight = MatchConstantNode;
            _rightActivation = _configurator.Constant<T>;
        }

        public void Find(Action<LeftJoinNode<T, TDiscard>> callback)
        {
            if (_node == null)
            {
                LeftJoinNode<T,TDiscard> joinNode = _left.Successors
                    .OfType<LeftJoinNode<T,TDiscard>>()
                    .Where(node => _matchRight(node.RightActivation))
                    .FirstOrDefault();

                if (joinNode != null)
                    _node = joinNode;
                else
                {
                    RightActivation<T> rightActivation = _rightActivation();
                    _node = _configurator.Left<T,TDiscard>(rightActivation);
                    _left.AddActivation(_node);
                }
            }

            if (_node != null)
                callback(_node);
        }

        static bool MatchNode(RightActivation<T> node, MemoryNode<T> right)
        {
            return right.Equals(node);
        }

        static bool MatchConstantNode(RightActivation<T> x)
        {
            return x.GetType().IsGenericType && x.GetType() == typeof (ConstantNode<T>);
        }
    }
}