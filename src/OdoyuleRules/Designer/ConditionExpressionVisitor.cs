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
namespace OdoyuleRules.Designer
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Models.SemanticModel;
    using Visualization;

    public class ConditionExpressionVisitor<TFact> :
        ExpressionVisitor
        where TFact : class
    {
        readonly IList<RuleCondition> _conditions;
        ParameterExpression _parameter;

        public ConditionExpressionVisitor()
        {
            _conditions = new List<RuleCondition>();
        }

        public IEnumerable<RuleCondition> Conditions
        {
            get { return _conditions; }
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (node.Parameters.Count == 1)
            {
                if (node.Body.Type != typeof (bool))
                    throw new ArgumentException("A when condition must evaluate to a boolean result");

                if (node.Parameters[0].Type != typeof (TFact))
                    throw new ArgumentException("The fact type did not match the expression: "
                                                + typeof (TFact).GetShortName());

                _parameter = node.Parameters[0];
            }

            return base.VisitLambda(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    return ParseBinaryCondition(node, typeof (PropertyEqualCondition<,>));

                case ExpressionType.LessThan:
                    return ParseBinaryCondition(node, typeof(PropertyLessThanCondition<,>));

                case ExpressionType.LessThanOrEqual:
                    return ParseBinaryCondition(node, typeof(PropertyLessThanOrEqualCondition<,>));

                case ExpressionType.GreaterThan:
                    return ParseBinaryCondition(node, typeof(PropertyGreaterThanCondition<,>));

                case ExpressionType.GreaterThanOrEqual:
                    return ParseBinaryCondition(node, typeof (PropertyGreaterThanOrEqualCondition<,>));
            }

            return base.VisitBinary(node);
        }

        Expression ParseBinaryCondition(BinaryExpression node, Type genericConditionType)
        {
            if (node.Left.NodeType == ExpressionType.MemberAccess
                && node.Right.NodeType == ExpressionType.Constant)
            {
                var memberAccess = node.Left as MemberExpression;
                if (memberAccess == null)
                    throw new ArgumentException();

                var propertyInfo = memberAccess.Member as PropertyInfo;
                if (propertyInfo == null)
                    throw new ArgumentException();

                var constant = node.Right as ConstantExpression;
                if (constant == null)
                    throw new ArgumentException();

                if (_parameter == null)
                    throw new ArgumentException("The fact was not an input parameter to the expression");

                var args = new[]
                    {
                        propertyInfo,
                        Expression.Lambda(node.Left, _parameter),
                        constant.Value,
                    };

                Type conditionType = genericConditionType.MakeGenericType(typeof (TFact),propertyInfo.PropertyType);

                var condition = (RuleCondition) Activator.CreateInstance(conditionType, args);

                _conditions.Add(condition);

                return node;
            }

            return base.VisitBinary(node);
        }
    }
}