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
    using Conditionals;
    using NUnit.Framework;

    [TestFixture]
    public class When_using_comparators_to_compare_values
    {
        [Test]
        public void Should_match_a_string_to_an_integer()
        {
            Value<int> left = Conditional.Constant(42);
            Value<string> right = Conditional.Constant("42");

            var converter = new ValueTypeConverter<int, string>(right);

            var comparator = new ValueEqualComparator<int>();

            int xx = 0;
            int yy = 0;

            comparator.Match(left, converter, (x, y) =>
                {
                    xx = x;
                    yy = y;
                });

            Assert.AreEqual(42, xx);
            Assert.AreEqual(42, yy);
        }
    }
}