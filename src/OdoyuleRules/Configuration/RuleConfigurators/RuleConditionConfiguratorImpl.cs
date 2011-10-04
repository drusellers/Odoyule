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
    using Models.SemanticModel;
    using Parsing;

    class RuleConditionConfiguratorImpl<T> :
        RuleConditionConfigurator<T>
        where T : class
    {
        readonly IList<RuleConsequenceConfigurator<T>> _consequenceConfigurators;
        IList<RuleCondition> _conditions;

        public RuleConditionConfiguratorImpl()
        {
            _consequenceConfigurators = new List<RuleConsequenceConfigurator<T>>();
            _conditions = new List<RuleCondition>();
        }

        public void AddCondition(RuleCondition condition)
        {
            _conditions.Add(condition);
        }

        public RuleConsequenceConfigurator<T> Then(Action<T> callback)
        {
            var configurator = new DelegateRuleConsequenceConfigurator<T>(callback);

            _consequenceConfigurators.Add(configurator);

            return configurator;
        }
    }
}