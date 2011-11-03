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
    using System.Collections.Generic;
    using Configuration.Builders;
    using Configuration.Configurators;
    using Configuration.RuleConfigurators;
    using Models.SemanticModel;

    public class RuleConsequenceConfiguratorImpl<T> :
        RuleConsequenceConfigurator<T>,
        RuleBuilderConfigurator
        where T : class
    {
        readonly RuleConsequence<T> _consequence;

        public RuleConsequenceConfiguratorImpl(RuleConsequence<T> consequence)
        {
            _consequence = consequence;
        }

        public IEnumerable<ValidationResult> ValidateConfiguration()
        {
            if (_consequence == null)
                yield return this.Failure("Consequence", "must not be null");
        }

        public RuleBuilder Configure(RuleBuilder builder)
        {
            builder.AddConsequence(_consequence);

            return builder;
        }
    }
}