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
namespace OdoyuleRules.Parsing
{
    public class RuleCondition
    {
        public RuleCondition(string name, Comparator op, string val)
        {
            Name = name;
            Operator = op;
            Value = val;
        }

        public string Name { get; set; }
        public Comparator Operator { get; set; }
        public string Value { get; set; }
    }
}