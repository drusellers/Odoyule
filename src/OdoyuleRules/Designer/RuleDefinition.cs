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
    using Configuration.Builders;
    using Configuration.RuleConfigurators;
    using Models.SemanticModel;

    /// <summary>
    /// A RuleDefinition is the base class used to create a class that uses
    /// the internal rule DSL to define a rule. Once defined, the RuleDefinition
    /// builds the semantic model for the rule, which can be added to the rules
    /// engine.
    /// </summary>
    public abstract class RuleDefinition :
        RuleDefinitionConfigurator
    {
        readonly RuleConfiguratorImpl _ruleConfigurator;

        protected RuleDefinition()
        {
            _ruleConfigurator = new RuleConfiguratorImpl();
            _ruleConfigurator.SetName(GenerateDefaultRuleName());
        }

        string GenerateDefaultRuleName()
        {
            return GetType().Name.Replace("Rule", "");
        }

        protected Binding<T> Fact<T>() 
            where T : class
        {
            var binding = _ruleConfigurator.Binding<T>();

            return binding;
        }

        public Rule Build()
        {
            RuleBuilder builder = new RuleBuilderImpl();

            return _ruleConfigurator.Configure();
        }
    }
}