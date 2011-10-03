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


            Console.WriteLine("Result: " + result.Value.Name);
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


            Console.WriteLine("Result: " + result.Value.Name);
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


            Console.WriteLine("Result: " + result.Value.Name);

            foreach (RuleCondition condition in result.Value.Conditions)
            {
                Console.WriteLine("Condition: " + condition.Name + condition.Operator + condition.Value);
            }
            Console.WriteLine("Rest: " + result.Rest);
        }


        [Test]
        public void TestCase4()
        {
        }
    }
}