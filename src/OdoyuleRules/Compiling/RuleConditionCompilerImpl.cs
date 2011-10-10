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
    using Configuration.RulesEngineConfigurators;
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
            _configurator.MatchEqualNode<T, TProperty>(condition.PropertyInfo, node =>
                {
                    var valueNode = node[condition.Value];

                    _configurator.MatchAlphaNode(valueNode, alpha =>
                        {
                            _alphaNodes.Add(new ConditionAlphaNode<Token<T, TProperty>>(_configurator, alpha));
                        });
                });

            return base.Visit(condition, next);
        }

        public override bool Visit<T, TProperty>(PropertyGreaterThanCondition<T, TProperty> condition,
                                                 Func<SemanticModelVisitor, bool> next)
        {
            var compareNode = _configurator.GreaterThan<T, TProperty>(condition.Value);

            _configurator.MatchCompareNode(condition.PropertyInfo, compareNode, node =>
                {
                    _configurator.MatchAlphaNode(node, alpha =>
                    {
                        _alphaNodes.Add(new ConditionAlphaNode<Token<T, TProperty>>(_configurator, alpha));
                    });
                });

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
    }
}