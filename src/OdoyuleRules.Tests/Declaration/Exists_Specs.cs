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
    using System.Collections.Generic;
    using Models.SemanticModel;
    using NUnit.Framework;
    using Visualizer;

    [TestFixture]
    public class Conditions_on_existing_recurring_members
    {
        [Test]
        public void Should_match_a_list_with_one_item()
        {
            _results.Clear();

            using (StatefulSession session = _engine.CreateSession())
            {
                session.Add(new Order {Lines = new List<OrderLine> {new OrderLine {ItemNumber = "123"}}});
                session.Run();
            }

            Assert.AreEqual(1, _results.Count);
        }

        [Test]
        public void Should_match_a_list_with_two_items()
        {
            _results.Clear();

            using (StatefulSession session = _engine.CreateSession())
            {
                session.Add(new Order
                    {
                        Lines = new List<OrderLine>
                            {
                                new OrderLine {ItemNumber = "123"},
                                new OrderLine {ItemNumber = "123"}
                            }
                    });
                session.Run();
            }

            Assert.AreEqual(1, _results.Count);
        }

        [Test]
        public void Should_not_match_an_empty_list()
        {
            _results.Clear();

            using (StatefulSession session = _engine.CreateSession())
            {
                session.Add(new Order {Lines = new List<OrderLine> {}});
                session.Run();
            }

            Assert.AreEqual(0, _results.Count);
        }

        [Test]
        [Explicit]
        public void Show_me_the_goods()
        {
            _engine.ShowVisualizer();
        }

        IList<Order> _results;
        RulesEngine _engine;

        [TestFixtureSetUp]
        public void Define_rule()
        {
            _results = new List<Order>();

            var conditions = new RuleCondition[]
                {
                    Conditions.Exists((Order x) => x.Lines), //, 
                    //Conditions.Equal((OrderLine x) => x.ItemNumber == "123"));
                };

            var consequences = new RuleConsequence[]
                {
                    Consequences.Delegate<Order>((session,x) => _results.Add(x)),
                };

            Rule rule = new OdoyuleRule("RuleA", conditions, consequences);

            _engine = RulesEngineFactory.New(x => x.Add(rule));
        }

        class Order
        {
            public IList<OrderLine> Lines { get; set; }
        }

        class OrderLine
        {
            public string ItemNumber { get; set; }
        }
    }
}