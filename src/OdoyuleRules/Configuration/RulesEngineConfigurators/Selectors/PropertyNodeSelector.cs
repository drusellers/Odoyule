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
    using System.Reflection;
    using Models.RuntimeModel;
    using Util;
    using Visualization;

    public class PropertyNodeSelector<T, TProperty> :
        RuntimeModelVisitorImpl,
        NodeSelector
        where T : class
    {
        readonly NodeSelector _next;
        RuntimeConfigurator _configurator;
        PropertyNode<T, TProperty> _node;

        public PropertyNodeSelector(NodeSelector next, RuntimeConfigurator configurator, PropertyInfo property)
        {
            _next = next;
            _configurator = configurator;

            Property = property;
        }

        public PropertyInfo Property { get; private set; }

        public NodeSelector Next
        {
            get { return _next; }
        }

        public void Select()
        {
            throw new NotImplementedException();
        }

        public void Select<TNode>(Node<TNode> node) where TNode : class
        {
            _node = null;
            node.Accept(this);

            if (_node == null)
            {
                PropertyNode<T, TProperty> propertyNode = _configurator.Property<T, TProperty>(Property);

                var parentNode = node as Node<T>;
                if (parentNode == null)
                    throw new ArgumentException("Expected propertyNode, but was " + node.GetType().Name);

                parentNode.AddActivation(propertyNode);

                _node = propertyNode;
            }

            _next.Select(_node);
        }

        public void Select<TNode>(MemoryNode<TNode> node) where TNode : class
        {
            _node = null;

            node.Accept(this);

            if (_node == null)
            {
                PropertyNode<T, TProperty> propertyNode = _configurator.Property<T, TProperty>(Property);

                var parentNode = node as MemoryNode<T>;
                if (parentNode == null)
                    throw new ArgumentException("Expected propertyNode, but was " + node.GetType().Name);

                parentNode.AddActivation(propertyNode);

                _node = propertyNode;
            }

            _next.Select(_node);
        }

        public override string ToString()
        {
            return string.Format("Property: [{0}], {1} => {2}", typeof (T).Tokens(), Property.Name,
                                 typeof (TProperty).Name);
        }

        public override bool Visit<TT, TTProperty>(PropertyNode<TT, TTProperty> node,
                                                   Func<RuntimeModelVisitor, bool> next)
        {
            var locator = this as PropertyNodeSelector<TT, TTProperty>;
            if (locator != null)
            {
                if (locator.Property.Equals(node.PropertyInfo))
                {
                    locator._node = node;
                    return false;
                }
            }

            return base.Visit(node, next);
        }
    }

    public class PropertyNodeSelector<T1, T2, TProperty> :
        RuntimeModelVisitorImpl,
        NodeSelector
        where T1 : class
    {
        readonly NodeSelector _next;
        readonly RuntimeConfigurator _configurator;
        PropertyNode<Token<T1,T2>, TProperty> _node;

        public PropertyNodeSelector(NodeSelector next, RuntimeConfigurator configurator, PropertyInfo property)
        {
            _next = next;
            _configurator = configurator;

            Property = property;
        }

        public PropertyInfo Property { get; private set; }

        public NodeSelector Next
        {
            get { return _next; }
        }

        public void Select()
        {
            throw new NotImplementedException();
        }

        public void Select<TNode>(Node<TNode> node) where TNode : class
        {
            _node = null;
            node.Accept(this);

            if (_node == null)
            {
                var fastProperty = new FastProperty<T2, TProperty>(Property);

                PropertyNode<Token<T1, T2>, TProperty> propertyNode =
                    _configurator.Property<Token<T1, T2>, TProperty>(Property, (x, next) => next(fastProperty.Get(x.Item2)));

                var parentNode = node as Node<Token<T1,T2>>;
                if (parentNode == null)
                    throw new ArgumentException("Expected propertyNode, but was " + node.GetType().Name);

                parentNode.AddActivation(propertyNode);

                _node = propertyNode;
            }

            _next.Select(_node);
        }

        public void Select<TNode>(MemoryNode<TNode> node) where TNode : class
        {
            _node = null;

            node.Accept(this);

            if (_node == null)
            {
                var fastProperty = new FastProperty<T2, TProperty>(Property);

                PropertyNode<Token<T1, T2>, TProperty> propertyNode =
                    _configurator.Property<Token<T1, T2>, TProperty>(Property, (x, next) => next(fastProperty.Get(x.Item2)));

                var parentNode = node as MemoryNode<Token<T1, T2>>;
                if (parentNode == null)
                    throw new ArgumentException("Expected propertyNode, but was " + node.GetType().Name);

                parentNode.AddActivation(propertyNode);

                _node = propertyNode;
            }

            _next.Select(_node);
        }

        public override string ToString()
        {
            return string.Format("Property: [{0}], {1} => {2}", typeof (Token<T1,T2>).Tokens(), Property.Name,
                                 typeof (TProperty).Name);
        }

        public override bool Visit<TT, TTProperty>(PropertyNode<TT, TTProperty> node,
                                                   Func<RuntimeModelVisitor, bool> next)
        {
            if(typeof(TT).IsGenericType && typeof(TT).GetGenericTypeDefinition() == typeof(Token<,>))
            {
                var arguments = typeof (TT).GetGenericArguments();

                bool keepGoing = (bool)GetType()
                    .GetMethod("VisitTokenPropertyNode", BindingFlags.NonPublic | BindingFlags.Instance)
                    .MakeGenericMethod(arguments[0], arguments[1], typeof (TProperty))
                    .Invoke(this, new object[] {node, next});

                return keepGoing || base.Visit(node, next);
            }

            return base.Visit(node, next);
        }

        bool VisitTokenPropertyNode<TT1,TT2,TTProperty>(PropertyNode<Token<TT1,TT2>,TTProperty> node, Func<RuntimeModelVisitor, bool> next)
            where TT1 : class
        {
            var locator = this as PropertyNodeSelector<TT1,TT2, TTProperty>;
            if (locator != null)
            {
                if (locator.Property.Equals(node.PropertyInfo))
                {
                    locator._node = node;
                    return false;
                }
            }

            return true;
        }
    }
}