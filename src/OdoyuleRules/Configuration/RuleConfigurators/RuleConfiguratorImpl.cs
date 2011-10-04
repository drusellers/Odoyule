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
namespace OdoyuleRules.Configuration.RuleConfigurators
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Models.SemanticModel;

    class RuleConfiguratorImpl :
        RuleConfigurator
    {
        readonly IList<RuleConditionConfigurator> _conditionConfigurators;
        string _ruleName;

        public RuleConfiguratorImpl(string ruleName)
        {
            _conditionConfigurators = new List<RuleConditionConfigurator>();

            _ruleName = ruleName;
        }

        public RuleConditionConfigurator<T> When<T>()
            where T : class
        {
            var configurator = new RuleConditionConfiguratorImpl<T>();

            _conditionConfigurators.Add(configurator);

            return configurator;
        }

        public RuleConditionConfigurator<T> When<T>(Expression<Func<T, bool>> predicate) 
            where T : class
        {
            var configurator = new RuleConditionConfiguratorImpl<T>();

            var condition = new PredicateCondition<T>(predicate);

            configurator.AddCondition(condition);

            _conditionConfigurators.Add(configurator);

            return configurator;            
        }
    }
}