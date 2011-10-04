namespace OdoyuleRules.Tests.Execution
{
    using Configuration.RulesEngineConfigurators;
    using Models.RuntimeModel;
    using NUnit.Framework;

    [TestFixture]
    public class When_building_a_production_network
    {
        A _called;

        [TestFixtureSetUp]
        public void Setup()
        {
            _called = null;

            var productionNode = new DelegateProductionNode<A>(x => _called = x);

            var constantNode = new ConstantNode<A>();

            var joinNode = new JoinNode<A>(69, constantNode);
            joinNode.AddActivation(productionNode);

            var engine = new OdoyuleRulesEngine(new RuntimeConfiguratorImpl());
            
            var alphaNode = engine.GetAlphaNode<A>();
            alphaNode.AddActivation(joinNode);

            using(var session = engine.CreateSession())
            {
                session.Add(new A());
                session.Run();
            }
        }

        [Test]
        public void Should_invoke_the_production_delegate()
        {
            Assert.IsNotNull(_called);
        }
    }

    public class A
    {
    }
}
