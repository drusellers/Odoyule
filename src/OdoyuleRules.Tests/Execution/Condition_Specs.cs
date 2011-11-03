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
    public class When_building_a_network_with_a_condition_node
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

            var engine = configurator.RulesEngine;

            PropertyNode<A, decimal> propertyNode = configurator.Property<A, decimal>(x => x.Amount);

            ConditionNode<Token<A, decimal>> conditionNode = configurator.Condition<A, decimal>(x => x > 10000.0m);
            propertyNode.AddActivation(conditionNode);

            AlphaNode<Token<A, decimal>> edgeAlpha = configurator.Alpha<A, decimal>();
            conditionNode.AddActivation(edgeAlpha);

            AlphaNode<A> alphaNode = configurator.GetAlphaNode<A>();
            alphaNode.AddActivation(propertyNode);

            JoinNode<A> joinNode = configurator.Join(alphaNode);
            
            DelegateProductionNode<A> productionNode = configurator.Delegate<A>((session,x) => _called = x);
            joinNode.AddActivation(productionNode);

            LeftJoinNode<A, decimal> leftNode = configurator.Left<A, decimal>(alphaNode);
            leftNode.AddActivation(joinNode);

            edgeAlpha.AddActivation(leftNode);

            using (StatefulSession session = engine.CreateSession())
            {
                session.Add(new A(10001.0m));
                session.Run();
            }
        }

        class A
        {
            public A(decimal amount)
            {
                Amount = amount;
            }

            public decimal Amount { get; private set; }
        }
    }
}