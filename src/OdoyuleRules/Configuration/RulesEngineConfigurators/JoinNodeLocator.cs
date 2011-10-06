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

    public class JoinNodeLocator<T> :
        RuntimeModelVisitorImpl
        where T : class
    {
        readonly MemoryNode<T> _left;
        readonly Func<RightActivation<T>, bool> _matchRight;
        readonly Func<RightActivation<T>> _rightActivation;
        readonly RuntimeConfigurator _configurator;
        JoinNode<T> _node;

        public JoinNodeLocator(RuntimeConfigurator runtimeConfigurator, MemoryNode<T> left)
        {
            _configurator = runtimeConfigurator;
            _left = left;
            _matchRight = MatchConstantNode;
            _rightActivation = _configurator.Constant<T>;
        }

        public JoinNodeLocator(RuntimeConfigurator runtimeConfigurator, MemoryNode<T> left, MemoryNode<T> right)
        {
            _configurator = runtimeConfigurator;
            _left = left;
            _matchRight = node => MatchNode(node, right);
            _rightActivation = () => right as RightActivation<T>;
        }

        public void Find(Action<JoinNode<T>> callback)
        {
            if (_node == null)
            {
                var joinNode = _left.Successors
                    .OfType<JoinNode<T>>()
                    .Where(node => _matchRight(node.RightActivation))
                    .FirstOrDefault();

                if(joinNode != null)
                    _node = joinNode;
                else
                {
                    _node = _configurator.Join(_rightActivation());
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
            return x.GetType().IsGenericType && x.GetType().GetGenericTypeDefinition() == typeof (ConstantNode<T>);
        }
    }
}