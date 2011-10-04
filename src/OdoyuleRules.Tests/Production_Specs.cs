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
namespace OdoyuleRules.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Submitting_a_context_matching_a_production
    {
        [Test]
        public void Should_invoke_the_callback()
        {
            var order = new Order(10001.0m);

            using (StatefulSession session = _engine.CreateSession())
            {
                session.Add(order);
                session.Run();
            }

            Assert.IsNotNull(_matchedOrder);
        }

        RulesEngine _engine;
        Order _matchedOrder;

        [TestFixtureSetUp]
        public void Setup()
        {
            _matchedOrder = null;
            _engine = RulesEngineFactory.New(x =>
                {
                    x.Rule("Order", r =>
                        {
                            r.When<Order>(order => order.Amount > 10000.0m)
                                .Then(order => _matchedOrder = order);
                        });
                });
        }

        class Order
        {
            public Order(decimal amount)
            {
                Amount = amount;
            }

            public decimal Amount { get; private set; }
        }
    }
}