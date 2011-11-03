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
    using Builders;
    using Configurators;
    using Models.SemanticModel;

    class RuleConditionConfiguratorImpl<T> :
        RuleConditionConfigurator<T>,
        RuleBuilderConfigurator
        where T : class
    {
        readonly RuleCondition<T> _condition;

        public RuleConditionConfiguratorImpl(RuleCondition condition)
        {
            _condition = condition as RuleCondition<T>;
            if (_condition == null)
                throw new ArgumentException("The condition fact type must match the configurator type");
        }

        public RuleBuilder Configure(RuleBuilder builder)
        {
            builder.AddCondition(_condition);

            return builder;
        }

        public IEnumerable<ValidationResult> ValidateConfiguration()
        {
            if (_condition == null)
                yield return this.Failure("Condition", "must not be null");
        }
    }
}