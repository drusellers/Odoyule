namespace OdoyuleRules.Tests
{
    using System;
    using NUnit.Framework;
    using Parsing;

    [TestFixture]
    public class Parser_specs
    {
        [Test]
        public void TestCase()
        {
            string text = " rule \"MyRule\" when then end";

            Console.WriteLine(text);

            var parser = new StringRuleParser();

            Result<string, RuleDefinition> result = parser.Rule(text);

            if (result == null)
            {
                Console.WriteLine("No match");
                return;
            }

            Console.WriteLine("Result: " + result.Value);
            Console.WriteLine("Rest: " + result.Rest);
        }

        [Test]
        public void TestCase2()
        {
            string text = " rule YourRule when then end";

            Console.WriteLine(text);

            var parser = new StringRuleParser();

            Result<string, RuleDefinition> result = parser.Rule(text);

            if (result == null)
            {
                Console.WriteLine("No match");
                return;
            }


            Console.WriteLine("Result: " + result.Value);
            Console.WriteLine("Rest: " + result.Rest);
        }

        [Test]
        public void TestCase3()
        {
            string text = @"
rule YourRule
    when 
        Name == 'Mary'
        City == 'Tulsa'
        ZipCode == '12345'
    then
end";

            Console.WriteLine(text);

            var parser = new StringRuleParser();

            Result<string, RuleDefinition> result = parser.Rule(text);

            if (result == null)
            {
                Console.WriteLine("No match");
                return;
            }


            Console.WriteLine("Result: " + result.Value);
            Console.WriteLine("Rest: " + result.Rest);
        }


        [Test]
        public void TestCase4()
        {
            string text = @"
rule YourRule
    when 
        MyClass(Name == 'Mary', City == 'Tulsa')
    then
end";

            Console.WriteLine(text);

            var parser = new StringRuleParser();

            Result<string, RuleDefinition> result = parser.Rule(text);

            if (result == null)
            {
                Console.WriteLine("No match");
                return;
            }


            Console.WriteLine("Result: " + result.Value);
            Console.WriteLine("Rest: " + result.Rest);
        }     
        
        [Test]
        public void A_rule_with_a_named_match()
        {
            string text = @"
rule YourRule
    when 
        $name : MyClass(Name == 'Mary', City == 'Tulsa')
    then
end";

            Console.WriteLine(text);

            var parser = new StringRuleParser();

            Result<string, RuleDefinition> result = parser.Rule(text);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);

            RuleDefinition rule = result.Value;

            Assert.AreEqual("YourRule", rule.Name);

            Assert.AreEqual(1, rule.Conditions.Length);

            RuleConditionImpl condition = rule.Conditions[0];
            Assert.IsNotNull(condition);

            var namedCondition = condition as AssignedRuleCondition;
            Assert.IsNotNull(namedCondition);

            Assert.AreEqual("name", namedCondition.Variable.Name);
        }
    }
}