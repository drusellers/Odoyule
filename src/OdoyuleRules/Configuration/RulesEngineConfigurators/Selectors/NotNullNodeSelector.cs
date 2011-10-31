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
namespace OdoyuleRules.Configuration.RulesEngineConfigurators.Selectors
{
    using System;
    using Models.RuntimeModel;
    using Visualization;

    public class NotNullNodeSelector<T, TProperty> :
        RuntimeModelVisitorImpl,
        NodeSelector
        where T : class
        where TProperty : class
    {
        readonly RuntimeConfigurator _configurator;
        readonly NodeSelector _next;
        NotNullNode<T, TProperty> _node;

        public NotNullNodeSelector(NodeSelector next,
                                   RuntimeConfigurator configurator)
        {
            _next = next;
            _configurator = configurator;
        }

        public NodeSelector Next
        {
            get { return _next; }
        }

        public void Select()
        {
            throw new NotImplementedException("An input node is required");
        }

        public void Select<TNode>(Node<TNode> node)
            where TNode : class
        {
            _node = null;
            node.Accept(this);

            if (_node == null)
            {
                NotNullNode<T, TProperty> compareNode = _configurator.NotNull<T, TProperty>();

                var parentNode = node as Node<Token<T, TProperty>>;
                if (parentNode == null)
                    throw new ArgumentException("Expected " + typeof (T).Tokens() + ", but was "
                                                + typeof (TNode).Tokens());

                parentNode.AddActivation(compareNode);

                _node = compareNode;
            }

            _next.Select(_node);
        }

        public void Select<TNode>(MemoryNode<TNode> node) where TNode : class
        {
            throw new NotImplementedException("MemoryNode is not supported for not null");
        }

        public override bool Visit<TT, TTProperty>(NotNullNode<TT, TTProperty> node,
                                                   Func<RuntimeModelVisitor, bool> next)
        {
            var locator = this as NotNullNodeSelector<TT, TTProperty>;
            if (locator != null)
            {
                locator._node = node;
                return false;
            }

            return base.Visit(node, next);
        }

        public override string ToString()
        {
            return string.Format("NotNull Node: [{0}]", typeof (T).Tokens());
        }
    }
}