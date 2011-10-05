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
namespace OdoyuleRules.Models.SemanticModel
{
    using System.Collections.Generic;
    using System.Linq;

    public class OdoyuleRule :
        Rule
    {
        readonly string _ruleName;
        IList<RuleCondition> _conditions;
        IList<RuleConsequence> _consequences;

        public OdoyuleRule(string ruleName,
                           IEnumerable<RuleCondition> conditions,
                           IEnumerable<RuleConsequence> consequences)
        {
            _ruleName = ruleName;
            _conditions = conditions.ToList();
            _consequences = consequences.ToList();
        }

        public string RuleName
        {
            get { return _ruleName; }
        }

        public IEnumerable<RuleCondition> Conditions
        {
            get { return _conditions; }
        }

        public IEnumerable<RuleConsequence> Consequences
        {
            get { return _consequences; }
        }

        public bool Accept(SemanticModelVisitor visitor)
        {
            return visitor.Visit(this, x => _conditions.All(y => y.Accept(x)) && _consequences.All(y => y.Accept(x)));
        }
    }
}