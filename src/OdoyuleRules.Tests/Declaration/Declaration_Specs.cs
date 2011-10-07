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
                session.Add(new A {Name = "JOE", Amount = 10001.0m});
                session.Run();
            }

            var visitor = new TextRuntimeModelVisitor();
            rulesEngine.Accept(visitor);

            Console.WriteLine(visitor);

            Assert.IsNotNull(_result);
        }

        [Test]
        public void Should_not_activate_for_only_one_side()
        {
            _result = null;

            RulesEngine rulesEngine = RulesEngineFactory.New(x => { x.Add(_rule); });

            using (StatefulSession session = rulesEngine.CreateSession())
            {
                session.Add(new A {Name = "JOE", Amount = 9999.0m});
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
                session.Add(new A {Name = "MAMA", Amount = 10001.0m});
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
            Assert.AreEqual(1, _rule.Consequences.Count());
        }

        [Test]
        [Explicit]
        public void Show_me_the_goods()
        {
            RulesEngine rulesEngine = RulesEngineFactory.New(x => { x.Add(_rule); });

            rulesEngine.ShowVisualizer();
        }

        Rule _rule;
        A _result;

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
                    Consequences.Delegate<A>(x => { _result = x; }),
                };

            _rule = new OdoyuleRule("RuleA", conditions, consequences);
        }

        class A
        {
            public string Name { get; set; }
            public decimal Amount { get; set; }
        }
    }
}