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
    using System;
    using System.Linq;
    using Models.SemanticModel;
    using NUnit.Framework;
    using Visualization;
    using Visualizer;

    [TestFixture]
    public class Defining_a_rule_using_the_semantic_model
    {
        [Test]
        public void Should_compile_and_execute()
        {
            _result = null;

            RulesEngine rulesEngine = RulesEngineFactory.New(x => { x.Add(_rule); });

            using (StatefulSession session = rulesEngine.CreateSession())
            {
                session.Add(new Order {Name = "JOE", Amount = 10001.0m});
                session.Run();
            }

            var visitor = new TextRuntimeModelVisitor();
            rulesEngine.Accept(visitor);

            Console.WriteLine(visitor);

            Assert.IsNotNull(_result);
            Assert.IsNotNull(_resultB);
        }

        [Test]
        public void Should_not_activate_for_only_one_side()
        {
            _result = null;

            RulesEngine rulesEngine = RulesEngineFactory.New(x => { x.Add(_rule); });

            using (StatefulSession session = rulesEngine.CreateSession())
            {
                session.Add(new Order {Name = "JOE", Amount = 9999.0m});
                session.Run();
            }

            var visitor = new TextRuntimeModelVisitor();
            rulesEngine.Accept(visitor);

            Console.WriteLine(visitor);

            Assert.IsNull(_result);
        }

        [Test]
        public void Should_not_activate_for_only_other_side()
        {
            _result = null;

            RulesEngine rulesEngine = RulesEngineFactory.New(x => { x.Add(_rule); });

            using (StatefulSession session = rulesEngine.CreateSession())
            {
                session.Add(new Order {Name = "MAMA", Amount = 10001.0m});
                session.Run();
            }

            var visitor = new TextRuntimeModelVisitor();
            rulesEngine.Accept(visitor);

            Console.WriteLine(visitor);

            Assert.IsNull(_result);
        }

        [Test]
        public void Should_have_the_proper_condition_count()
        {
            Assert.AreEqual(2, _rule.Conditions.Count());
        }

        [Test]
        public void Should_have_the_proper_consequence_count()
        {
            Assert.AreEqual(2, _rule.Consequences.Count());
        }

        [Test]
        [Explicit]
        public void Show_me_the_goods()
        {
            RulesEngine rulesEngine = RulesEngineFactory.New(x =>
                {
                    x.Add(_rule);
                });

            rulesEngine.ShowVisualizer();
        }

        [Test]
        [Explicit]
        public void Show_me_the_goods_x2()
        {
            RulesEngine rulesEngine = RulesEngineFactory.New(x =>
                {
                    x.Add(_rule);
                    x.Add(_rule2);
                    x.Add(_rule3);
                });

            rulesEngine.ShowVisualizer();
        }

        Order _result;
        Order _resultB;
        Rule _rule;
        Rule _rule2;
        Rule _rule3;

        [TestFixtureSetUp]
        public void Define_rule()
        {
            var conditions = new RuleCondition[]
                {
                    Conditions.Equal((Order x) => x.Name, "JOE"),
                    Conditions.GreaterThan((Order x) => x.Amount, 10000.0m),
                };

            var consequences = new RuleConsequence[]
                {
                    Consequences.Delegate<Order>((session,x) => { _result = x; }),
                    Consequences.Delegate<Order>((session,x) => { _resultB = x; }),
                };

            _rule = new OdoyuleRule("RuleA", conditions, consequences);
            _rule2 = new OdoyuleRule("RuleB", conditions, consequences);

            conditions = new RuleCondition[]
                {
                    Conditions.Equal((Account a) => a.Name, "JOE"),
                };

            consequences = new RuleConsequence[]
                {
                    Consequences.Delegate((Session session, Account a) => { }),
                };

            _rule3 = new OdoyuleRule("RuleC", conditions, consequences);
        }

        class Order
        {
            public string Name { get; set; }
            public decimal Amount { get; set; }
        }

        class Account
        {
            public string Name { get; set; }
            public decimal CreditLimit { get; set; }
        }
    }
}