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
namespace OdoyuleRules.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public interface PropertyExpressionVisitor
    {
        NodeSelector CreateSelector(Expression expression);
    }


    public class PropertyExpressionVisitor<T> :
        ExpressionVisitor,
        PropertyExpressionVisitor
        where T : class
    {
        NodeSelectorFactory _factory;
        NodeSelector _selector;

        public PropertyExpressionVisitor()
        {
        }

        public PropertyExpressionVisitor(NodeSelectorFactory factory)
        {
            _factory = factory;
        }

        public NodeSelector Selector
        {
            get { return _selector; }
        }

        public NodeSelector CreateSelector(Expression expression)
        {
            Visit(expression);

            return _selector;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.MemberType == MemberTypes.Property)
            {
                return VisitProperty(node);
            }
            
            return base.VisitMember(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.ArrayIndex && node.Right.NodeType == ExpressionType.Constant)
            {
                var constantExpression = node.Right as ConstantExpression;

                Type factoryType = typeof (ArrayNodeSelectorFactory<>).MakeGenericType(node.Type);

                var nodeSelectorFactory =
                    (NodeSelectorFactory) Activator.CreateInstance(factoryType, _factory, constantExpression.Value);

                Type visitorType = typeof (PropertyExpressionVisitor<>).MakeGenericType(node.Left.Type);
                var visitor = (PropertyExpressionVisitor) Activator.CreateInstance(visitorType, nodeSelectorFactory);
                _selector = visitor.CreateSelector(node.Left);

                return node;
            }

            return base.VisitBinary(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            var factory = new TypeNodeSelectorFactory(_factory);

            _selector = factory.Create<T>();

            return base.VisitParameter(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.IsSpecialName && node.Method.Name.Equals("get_Item") && node.Object != null
                && node.Object.Type.IsGenericType)
            {
                Type typeDefinition = node.Object.Type.GetGenericTypeDefinition();
                if (typeDefinition == typeof (IList<>) || typeDefinition == typeof (List<>))
                {
                    var indexExpression = node.Arguments[0] as ConstantExpression;
                    if (indexExpression != null)
                    {
                        Type factoryType = typeof (ListNodeSelectorFactory<>).MakeGenericType(node.Type);

                        var nodeSelectorFactory =
                            (NodeSelectorFactory) Activator.CreateInstance(factoryType, _factory, indexExpression.Value);

                        Type visitorType = typeof (PropertyExpressionVisitor<>).MakeGenericType(node.Object.Type);
                        var visitor =
                            (PropertyExpressionVisitor) Activator.CreateInstance(visitorType, nodeSelectorFactory);
                        _selector = visitor.CreateSelector(node.Object);

                        return node;
                    }
                }
            }

            return base.VisitMethodCall(node);
        }

        Expression VisitProperty(MemberExpression memberExpression)
        {
            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException("Expected a property expression");

            Type factoryType = typeof (PropertyNodeSelectorFactory<>).MakeGenericType(propertyInfo.PropertyType);

            var nodeSelectorFactory =
                (NodeSelectorFactory) Activator.CreateInstance(factoryType, _factory, propertyInfo);

            Type visitorType = typeof (PropertyExpressionVisitor<>).MakeGenericType(memberExpression.Expression.Type);
            var visitor = (PropertyExpressionVisitor) Activator.CreateInstance(visitorType, nodeSelectorFactory);
            _selector = visitor.CreateSelector(memberExpression.Expression);

            return memberExpression;
        }
    }
}