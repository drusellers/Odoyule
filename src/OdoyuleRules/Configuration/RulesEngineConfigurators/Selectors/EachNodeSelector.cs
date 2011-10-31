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
    using System.Collections;
    using Models.RuntimeModel;
    using Visualization;

    public abstract class EachNodeSelector<T, TProperty, TElement> :
        RuntimeModelVisitorImpl,
        NodeSelector
        where T : class
        where TProperty : class, IEnumerable
    {
        readonly RuntimeConfigurator _configurator;
        readonly NodeSelector _next;
        EachNode<T, TProperty, TElement> _node;

        protected EachNodeSelector(NodeSelector next, RuntimeConfigurator configurator)
        {
            _next = next;
            _configurator = configurator;
        }

        protected RuntimeConfigurator Configurator
        {
            get { return _configurator; }
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
                EachNode<T, TProperty, TElement> eachNode = CreateNode();

                var parentNode = node as Node<Token<T, TProperty>>;
                if (parentNode == null)
                    throw new ArgumentException("Expected " + typeof (T).Tokens() + ", but was "
                                                + typeof (TNode).Tokens());

                parentNode.AddActivation(eachNode);

                _node = eachNode;
            }

            _next.Select(_node);
        }

        public void Select<TNode>(MemoryNode<TNode> node) where TNode : class
        {
            throw new NotImplementedException("MemoryNode is not supported for each");
        }

        protected abstract EachNode<T, TProperty, TElement> CreateNode();

        public override bool Visit<TT, TTProperty, TTElement>(EachNode<TT, TTProperty, TTElement> node,
                                                              Func<RuntimeModelVisitor, bool> next)
        {
            var locator = this as EachNodeSelector<TT, TTProperty, TTElement>;
            if (locator != null)
            {
                locator._node = node;
                return false;
            }

            return base.Visit(node, next);
        }

        public override string ToString()
        {
            return string.Format("Each Node: [{0}]", typeof (T).Tokens());
        }
    }
}