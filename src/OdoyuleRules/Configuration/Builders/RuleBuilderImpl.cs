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
namespace OdoyuleRules.Configuration.Builders
{
    using System.Collections.Generic;
    using Models.SemanticModel;

    public class RuleBuilderImpl :
        RuleBuilder
    {
        readonly string _ruleName;
        List<RuleCondition> _conditions;
        List<RuleConsequence> _consequences;

        public RuleBuilderImpl(string ruleName)
        {
            _ruleName = ruleName;
            _conditions = new List<RuleCondition>();
            _consequences = new List<RuleConsequence>();
        }

        public string RuleName
        {
            get { return _ruleName; }
        }

        public void AddCondition<T>(RuleCondition<T> condition)
        {
            _conditions.Add(condition);
        }

        public void AddConsequence<T>(RuleConsequence<T> consequence)
        {
            _consequences.Add(consequence);
        }

        public Rule Build()
        {
            return new OdoyuleRule(_ruleName, _conditions, _consequences);
        }
    }
}