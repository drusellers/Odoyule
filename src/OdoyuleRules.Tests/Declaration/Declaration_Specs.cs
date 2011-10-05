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
namespace OdoyuleRules.Tests.Declaration
{
    using System.Linq;
    using Models.SemanticModel;
    using NUnit.Framework;

    [TestFixture]
    public class Defining_a_rule_using_the_semantic_model
    {
        Rule _rule;

        [TestFixtureSetUp]
        public void Define_rule()
        {
            var conditions = new RuleCondition[]
                {
                    Conditions.Equal((A x) => x.Name, "JOE"),
                    Conditions.GreaterThan((A x) => x.Amount, 10000.0m),
                };

            var consequences = new RuleConsequence[]
                {
                    Consequences.Delegate<A>(x => { }),
                };

            _rule = new OdoyuleRule(conditions, consequences);
        }

        class A
        {
            public string Name { get; set; }
            public decimal Amount { get; set; }
        }

        [Test]
        public void Should_have_the_proper_condition_count()
        {
            Assert.AreEqual(2, _rule.Conditions.Count());
        }

        [Test]
        public void Should_have_the_proper_consequence_count()
        {
            Assert.AreEqual(1, _rule.Consequences.Count());
        }
    }
}