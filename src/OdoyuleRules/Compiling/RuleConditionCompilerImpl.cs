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
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Configuration.RuleConfigurators;
    using Configuration.RulesEngineConfigurators;
    using Configuration.RulesEngineConfigurators.Selectors;
    using Models.RuntimeModel;
    using Models.SemanticModel;

    public class RuleConditionCompilerImpl :
        SemanticModelVisitorImpl,
        RuleConditionCompiler
    {
        readonly IList<ConditionAlphaNode> _alphaNodes;
        readonly RuntimeConfigurator _configurator;

        public RuleConditionCompilerImpl(RuntimeConfigurator configurator)
        {
            _configurator = configurator;
            _alphaNodes = new List<ConditionAlphaNode>();
        }

        public override bool Visit<T, TProperty>(PropertyEqualCondition<T, TProperty> condition,
                                                 Func<SemanticModelVisitor, bool> next)
        {
            var conditionFactory = new ConditionAlphaNodeSelectorFactory(_configurator, node => _alphaNodes.Add(node));

            var alphaFactory = new AlphaNodeSelectorFactory(conditionFactory, _configurator);

            var equalFactory = new EqualNodeSelectorFactory<TProperty>(alphaFactory, _configurator, condition.Value);

            var visitor = new PropertyExpressionVisitor<T>(equalFactory, _configurator);
            NodeSelector selector = visitor.CreateSelector(condition.PropertyExpression.Body);

            selector.Select();

            return base.Visit(condition, next);
        }

        public override bool Visit<T, TProperty>(PropertyGreaterThanCondition<T, TProperty> condition,
                                                 Func<SemanticModelVisitor, bool> next)
        {
            CompareNode<T, TProperty> compareNode = _configurator.GreaterThan<T, TProperty>(condition.Value);

            AddCompareCondition(condition.PropertyExpression, compareNode);

            return base.Visit(condition, next);
        }

        public override bool Visit<T, TProperty>(PropertyGreaterThanOrEqualCondition<T, TProperty> condition,
                                                 Func<SemanticModelVisitor, bool> next)
        {
            CompareNode<T, TProperty> compareNode = _configurator.GreaterThanOrEqual<T, TProperty>(condition.Value);

            AddCompareCondition(condition.PropertyExpression, compareNode);

            return base.Visit(condition, next);
        }

        public override bool Visit<T, TProperty>(PropertyLessThanCondition<T, TProperty> condition,
                                                 Func<SemanticModelVisitor, bool> next)
        {
            CompareNode<T, TProperty> compareNode = _configurator.LessThan<T, TProperty>(condition.Value);

            AddCompareCondition(condition.PropertyExpression, compareNode);

            return base.Visit(condition, next);
        }

        public override bool Visit<T, TProperty>(PropertyLessThanOrEqualCondition<T, TProperty> condition,
                                                 Func<SemanticModelVisitor, bool> next)
        {
            CompareNode<T, TProperty> compareNode = _configurator.LessThanOrEqual<T, TProperty>(condition.Value);

            AddCompareCondition(condition.PropertyExpression, compareNode);

            return base.Visit(condition, next);
        }

        public void MatchJoinNode<T>(Action<JoinNode<T>> callback)
            where T : class
        {
            if (_alphaNodes.Count == 0)
                return;

            _alphaNodes[0].Select<T>(alpha =>
                {
                    if (_alphaNodes.Count == 1)
                    {
                        _configurator.MatchJoinNode(alpha, callback);
                        return;
                    }

                    MemoryNode<T> left = alpha;

                    for (int i = 1; i < _alphaNodes.Count; i++)
                    {
                        _alphaNodes[i].Select<T>(right =>
                            {
                                _configurator.MatchJoinNode(left, right, join =>
                                    {
                                        if (i + 1 < _alphaNodes.Count)
                                        {
                                            left = join;
                                        }
                                        else
                                        {
                                            callback(join);
                                        }
                                    });
                            });
                    }
                });
        }

        void AddCompareCondition<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression,
                                               CompareNode<T, TProperty> compareNode)
            where T : class
        {
            var conditionFactory = new ConditionAlphaNodeSelectorFactory(_configurator, node => _alphaNodes.Add(node));

            var alphaFactory = new AlphaNodeSelectorFactory(conditionFactory, _configurator);

            var compareFactory = new CompareNodeSelectorFactory<TProperty>(alphaFactory, _configurator,
                                                                           compareNode.Comparator,
                                                                           compareNode.Value);

            new PropertyExpressionVisitor<T>(compareFactory, _configurator)
                .CreateSelector(propertyExpression.Body)
                .Select();
        }
    }
}