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
namespace OdoyuleRules.Tests.InternalDSL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Designer;
    using Models.SemanticModel;
    using NUnit.Framework;

    [TestFixture]
    public class When_parsing_predicate_expressions
    {
        [Test]
        public void Should_parse_a_boolean_member_condition()
        {
            Expression<Func<A, bool>> expression = x => x.Available;

            List<RuleCondition> conditions = expression.ParseConditions().ToList();

            Assert.AreEqual(1, conditions.Count);

            Assert.IsInstanceOf<PropertyEqualCondition<A, bool>>(conditions[0]);
        }

        [Test]
        public void Should_parse_a_char_equals_condition()
        {
            Expression<Func<B, bool>> expression = x => x.Code == 'A';

            List<RuleCondition> conditions = expression.ParseConditions().ToList();

            Assert.AreEqual(1, conditions.Count);

            Assert.IsInstanceOf<PropertyEqualCondition<B, char>>(conditions[0]);
        }

        [Test]
        public void Should_parse_a_greater_than_condition()
        {
            Expression<Func<B, bool>> expression = x => x.Value > 0;

            List<RuleCondition> conditions = expression.ParseConditions().ToList();

            Assert.AreEqual(1, conditions.Count);
            Assert.IsInstanceOf<PropertyGreaterThanCondition<B, int>>(conditions[0]);
        }

        [Test]
        public void Should_parse_a_greater_than_or_equal_condition()
        {
            Expression<Func<B, bool>> expression = x => x.Value >= 0;

            List<RuleCondition> conditions = expression.ParseConditions().ToList();

            Assert.AreEqual(1, conditions.Count);
            Assert.IsInstanceOf<PropertyGreaterThanOrEqualCondition<B, int>>(conditions[0]);
        }

        [Test]
        public void Should_parse_a_less_than_condition()
        {
            Expression<Func<B, bool>> expression = x => x.Value < 0;

            List<RuleCondition> conditions = expression.ParseConditions().ToList();

            Assert.AreEqual(1, conditions.Count);
            Assert.IsInstanceOf<PropertyLessThanCondition<B, int>>(conditions[0]);
        }

        [Test]
        public void Should_parse_a_less_than_or_equal_condition()
        {
            Expression<Func<B, bool>> expression = x => x.Value <= 0;

            List<RuleCondition> conditions = expression.ParseConditions().ToList();

            Assert.AreEqual(1, conditions.Count);
            Assert.IsInstanceOf<PropertyLessThanOrEqualCondition<B, int>>(conditions[0]);
        }

        [Test]
        public void Should_parse_an_equal_condition()
        {
            Expression<Func<A, bool>> expression = x => x.Name == "bob";

            List<RuleCondition> conditions = expression.ParseConditions().ToList();

            Assert.AreEqual(1, conditions.Count);

            Assert.IsInstanceOf<PropertyEqualCondition<A, string>>(conditions[0]);
        }

        [Test]
        public void Should_parse_two_equal_conditions()
        {
            Expression<Func<A, bool>> expression = x => x.Name == "bob" && x.Gender == "M";

            List<RuleCondition> conditions = expression.ParseConditions().ToList();

            Assert.AreEqual(2, conditions.Count);

            Assert.IsInstanceOf<PropertyEqualCondition<A, string>>(conditions[0]);
            Assert.IsInstanceOf<PropertyEqualCondition<A, string>>(conditions[1]);
        }

        [Test]
        public void Should_support_basic_math_functions()
        {
            Expression<Func<B, bool>> expression = x => x.Value > 3 + 4 && x.Value == 16/2;

            List<RuleCondition> conditions = expression.ParseConditions().ToList();

            Assert.AreEqual(2, conditions.Count);

            Assert.IsInstanceOf<PropertyGreaterThanCondition<B, int>>(conditions[0]);
            Assert.AreEqual(7, ((PropertyGreaterThanCondition<B, int>) conditions[0]).Value);
            Assert.IsInstanceOf<PropertyEqualCondition<B, int>>(conditions[1]);
            Assert.AreEqual(8, ((PropertyEqualCondition<B, int>) conditions[1]).Value);
        }

        [Test]
        public void Should_support_or_at_some_point_but_not_now()
        {
            Expression<Func<B, bool>> expression = x => x.Value > 3 + 4 || x.Value == 16/2;

            Assert.Throws<ArgumentException>(() => { expression.ParseConditions().ToList(); });
        }


        class A
        {
            public string Name { get; set; }
            public string Gender { get; set; }
            public bool Available { get; set; }
        }

        class B
        {
            public char Code { get; set; }
            public int Value { get; set; }
        }
    }
}