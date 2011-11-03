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
namespace OdoyuleRules.Tests.Execution
{
    using Configuration.RulesEngineConfigurators;
    using Models.RuntimeModel;
    using NUnit.Framework;

    [TestFixture]
    public class When_building_a_production_network
    {
        [Test]
        public void Should_invoke_the_production_delegate()
        {
            Assert.IsNotNull(_called);
        }

        A _called;

        [TestFixtureSetUp]
        public void Setup()
        {
            _called = null;

            var productionNode = new DelegateProductionNode<A>(16, (session,x) => _called = x);

            var constantNode = new ConstantNode<A>(42);

            var joinNode = new JoinNode<A>(69, constantNode);
            joinNode.AddActivation(productionNode);

            var engine = new OdoyuleRulesEngine(new RuntimeConfiguratorImpl());

            AlphaNode<A> alphaNode = engine.GetAlphaNode<A>();
            alphaNode.AddActivation(joinNode);

            using (StatefulSession session = engine.CreateSession())
            {
                session.Add(new A());
                session.Run();
            }
        }

        class A
        {
        }
    }

    [TestFixture]
    public class When_building_a_production_network_with_a_split_join
    {
        [Test]
        public void Should_invoke_the_production_delegate()
        {
            Assert.IsNotNull(_called);
        }

        A _called;

        [TestFixtureSetUp]
        public void Setup()
        {
            _called = null;

            var configurator = new RuntimeConfiguratorImpl();

            var productionNode = new DelegateProductionNode<A>(16, (session,x) => _called = x);

            var constantNode = new ConstantNode<A>(42);

            var joinNode = configurator.CreateNode(id => new JoinNode<A>(id, constantNode));

            var constantNode2 = new ConstantNode<A>(27);

            var joinNode2 = configurator.CreateNode(id => new JoinNode<A>(id, constantNode2));
            joinNode2.AddActivation(productionNode);

            joinNode.AddActivation(joinNode2);

            var engine = new OdoyuleRulesEngine(configurator);

            AlphaNode<A> alphaNode = engine.GetAlphaNode<A>();
            alphaNode.AddActivation(joinNode);

            using (StatefulSession session = engine.CreateSession())
            {
                session.Add(new A());
                session.Run();
            }
        }

        class A
        {
        }
    }
}