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
namespace OdoyuleRules
{
    using System;
    using Configuration.RulesEngineConfigurators;
    using Designer;
    using Models.SemanticModel;

    public static class DesignerExtensions
    {
        public static void Rule<TRule>(this RulesEngineConfigurator configurator)
            where TRule : RuleDesigner, new()
        {
            Rule(configurator, () => new TRule());
        }

        public static void Rule<TRule>(this RulesEngineConfigurator configurator, Func<TRule> ruleFactory)
            where TRule : RuleDesigner
        {
            TRule ruleDesigner = ruleFactory();

            Rule rule = ruleDesigner.Build();

            configurator.Add(rule);
        }
    }
}