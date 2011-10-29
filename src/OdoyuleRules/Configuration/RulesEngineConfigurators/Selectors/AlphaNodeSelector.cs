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

    public class AlphaNodeSelector<T> :
        RuntimeModelVisitorImpl,
        NodeSelector
        where T : class
    {
        readonly RuntimeConfigurator _configurator;
        readonly NodeSelector _next;
        AlphaNode<T> _node;

        public AlphaNodeSelector(NodeSelector next, RuntimeConfigurator configurator)
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
            _node = _configurator.GetAlphaNode<T>();

            _next.Select(_node);
        }

        public void Select<TNode>(Node<TNode> node) where TNode : class
        {
            _node = null;
            node.Accept(this);

            if (_node == null)
            {
                AlphaNode<T> alphaNode = _configurator.Alpha<T>();

                var parentNode = node as Node<T>;
                if (parentNode == null)
                    throw new ArgumentException("Expected " + typeof(T).Tokens() + ", but was "
                                                + typeof(TNode).Tokens());

                parentNode.AddActivation(alphaNode);

                _node = alphaNode;
            }

            _next.Select(_node);
        }

        public void Select<TNode>(MemoryNode<TNode> node) where TNode : class
        {
            _node = null;
            node.Accept(this);

            if (_node == null)
            {
                AlphaNode<T> alphaNode = _configurator.Alpha<T>();

                var parentNode = node as MemoryNode<T>;
                if (parentNode == null)
                    throw new ArgumentException("Expected " + typeof (Node<T>).FullName + ", but was "
                                                + node.GetType().FullName);

                parentNode.AddActivation(alphaNode);

                _node = alphaNode;
            }

            _next.Select(_node);
        }

        public override bool Visit<TT>(AlphaNode<TT> node, Func<RuntimeModelVisitor, bool> next)
        {
            var locator = this as AlphaNodeSelector<TT>;
            if (locator != null)
            {
                locator._node = node;
                return false;
            }

            return base.Visit(node, next);
        }

        public override string ToString()
        {
            return string.Format("Alpha Node: [{0}]", typeof (T).Tokens());
        }
    }
}