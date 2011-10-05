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
namespace OdoyuleRules.Configuration.RulesEngineConfigurators
{
    using System;
    using System.Collections.Generic;
    using Configurators;
    using Models.SemanticModel;
    using RuleConfigurators;

    class RulesEngineConfiguratorImpl :
        RulesEngineConfigurator,
        Configurator
    {
        IList<RulesEngineBuilderConfigurator> _ruleConfigurators;

        public RulesEngineConfiguratorImpl()
        {
            _ruleConfigurators = new List<RulesEngineBuilderConfigurator>();
        }

        public RulesEngine Create()
        {
            RuntimeConfigurator runtimeConfigurator = new RuntimeConfiguratorImpl();

            return new OdoyuleRulesEngine(runtimeConfigurator);
        }

        public IEnumerable<ValidationResult> ValidateConfiguration()
        {
            yield break;
        }

        public void Rule(string name, Action<RuleConfigurator> configureCallback)
        {
            var configurator = new RuleConfiguratorImpl(name);

            configureCallback(configurator);

            _ruleConfigurators.Add(configurator);

        }

        public void Add(params Rule[] rules)
        {
            foreach (var rule in rules)
            {
                var configurator = new SemanticModelRuleConfigurator(rule);

                _ruleConfigurators.Add(configurator);
            }
        }
    }
}