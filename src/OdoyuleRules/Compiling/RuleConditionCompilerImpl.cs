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
        RuleConditionCompiler
    {
        RuntimeConfigurator _configurator;
        IList<object> _alphaNodes;

        public RuleConditionCompilerImpl(RuntimeConfigurator configurator)
        {
            _configurator = configurator;
            _alphaNodes = new List<object>();

        }

        public bool Visit(Rule rule, Func<SemanticModelVisitor, bool> next)
        {
            return true;
        }

        public bool Visit<T, TProperty>(PropertyEqualCondition<T, TProperty> condition,
                                        Func<SemanticModelVisitor, bool> next) where T : class
        {
            _configurator.MatchEqualNode<T, TProperty>(condition.PropertyInfo, node =>
                {
                    var alpha = _configurator.Alpha<T,TProperty>();
                    _alphaNodes.Add(alpha);

                    node.AddActivation(condition.Value, alpha);
                });

            return true;
        }

        public bool Visit<T, TProperty>(PropertyNotEqualCondition<T, TProperty> condition,
                                        Func<SemanticModelVisitor, bool> next) where T : class
        {
            throw new NotImplementedException();
        }

        public bool Visit<T, TProperty>(PropertyLessThanCondition<T, TProperty> condition,
                                        Func<SemanticModelVisitor, bool> next) where T : class
        {
            throw new NotImplementedException();
        }

        public bool Visit<T, TProperty>(PropertyLessThanOrEqualCondition<T, TProperty> condition,
                                        Func<SemanticModelVisitor, bool> next) where T : class
        {
            throw new NotImplementedException();
        }

        public bool Visit<T, TProperty>(PropertyGreaterThanCondition<T, TProperty> condition,
                                        Func<SemanticModelVisitor, bool> next) 
            where T : class
            where TProperty : IComparable<TProperty>
        {
            _configurator.MatchPropertyNode<T, TProperty>(condition.PropertyInfo, node =>
            {
                var alpha = _configurator.Alpha<T, TProperty>();
                _alphaNodes.Add(alpha);

                var conditionNode = new ConditionNode<Token<T, TProperty>>((x,accept) =>
                    {
                        if(x.Item2.CompareTo(condition.Value) > 0)
                            accept();
                    });

                node.AddActivation(conditionNode);
            });

            return true;
        }

        public bool Visit<T, TProperty>(PropertyGreaterThanOrEqualCondition<T, TProperty> condition,
                                        Func<SemanticModelVisitor, bool> next) where T : class
        {
            throw new NotImplementedException();
        }

        public bool Visit<T>(PredicateCondition<T> condition, Func<SemanticModelVisitor, bool> next)
        {
            throw new NotImplementedException();
        }

        public bool Visit<T>(DelegateConsequence<T> consequence, Func<SemanticModelVisitor, bool> next)
        {
            throw new NotImplementedException();
        }
    }
}