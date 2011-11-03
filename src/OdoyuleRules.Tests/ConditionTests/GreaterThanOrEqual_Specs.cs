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
namespace OdoyuleRules.Tests.ConditionTests
{
    using Models.SemanticModel;
    using NUnit.Framework;

    [TestFixture]
    public class Conditions_using_greater_than_or_equal
    {
        [Test]
        public void Should_match_equal_values()
        {
            _result = null;

            using (StatefulSession session = _engine.CreateSession())
            {
                session.Add(new Order {Amount = 10000.0m});
                session.Run();
            }

            Assert.IsNotNull(_result);
        }

        [Test]
        public void Should_match_greater_values()
        {
            _result = null;

            using (StatefulSession session = _engine.CreateSession())
            {
                session.Add(new Order {Amount = 10001.0m});
                session.Run();
            }

            Assert.IsNotNull(_result);
        }

        [Test]
        public void Should_not_match_less_than_values()
        {
            _result = null;

            using (StatefulSession session = _engine.CreateSession())
            {
                session.Add(new Order {Amount = 9999.9m});
                session.Run();
            }

            Assert.IsNull(_result);
        }

        Order _result;
        RulesEngine _engine;

        [TestFixtureSetUp]
        public void Define_rule()
        {
            var conditions = new RuleCondition[]
                {
                    Conditions.GreaterThanOrEqual((Order x) => x.Amount, 10000.0m),
                };

            var consequences = new RuleConsequence[]
                {
                    Consequences.Delegate<Order>((session,x) => { _result = x; }),
                };

            Rule rule = new OdoyuleRule("RuleA", conditions, consequences);

            _engine = RulesEngineFactory.New(x => x.Add(rule));
        }

        class Order
        {
            public decimal Amount { get; set; }
        }
    }
}