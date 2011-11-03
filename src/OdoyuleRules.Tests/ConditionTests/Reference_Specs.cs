namespace OdoyuleRules.Tests.ConditionTests
{
    using Models.SemanticModel;
    using NUnit.Framework;

    [TestFixture]
    public class Conditions_against_reference_types
    {
        [Test]
        public void Should_match_equal_values()
        {
            _result = null;

            using (StatefulSession session = _engine.CreateSession())
            {
                session.Add(new Order {Customer = new Account{ContactName = "JOE"}});
                session.Run();
            }

            Assert.IsNotNull(_result);
        }

        [Test]
        public void Should_not_match_inequal_values()
        {
            _result = null;

            using (StatefulSession session = _engine.CreateSession())
            {
                session.Add(new Order { Customer = new Account { ContactName = "BOB" } });
                session.Run();
            }

            Assert.IsNull(_result);
        }

        [Test]
        public void Should_not_match_or_crash_on_null_reference_types()
        {
            _result = null;

            using (StatefulSession session = _engine.CreateSession())
            {
                session.Add(new Order());
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
                    Conditions.Equal((Order x) => x.Customer.ContactName, "JOE"),
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
            public Account Customer { get; set; }
        }

        class Account
        {
            public string ContactName { get; set; }
        }
    }
}