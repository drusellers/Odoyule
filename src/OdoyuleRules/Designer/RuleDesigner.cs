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
    using Configuration.RuleConfigurators;
    using Models.SemanticModel;

    public abstract class RuleDesigner
    {
        RuleConfigurator _ruleConfigurator;

        protected RuleDesigner()
        {
            _ruleConfigurator = new RuleConfiguratorImpl(GetType().Name.Replace("Rule", ""));
        }

        protected Binding<T> Fact<T>() 
            where T : class
        {
            var fact = new BindingImpl<T>(this);

            return fact;
        }

        public Rule Build()
        {
            throw new NotImplementedException();
        }
    }
}