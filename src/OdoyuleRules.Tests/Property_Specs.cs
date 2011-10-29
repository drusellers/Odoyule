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
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Configuration.RuleConfigurators;
    using Configuration.RulesEngineConfigurators.Selectors;
    using Models.RuntimeModel;
    using NUnit.Framework;

    [TestFixture]
    public class When_accessing_a_property
    {
        [Test]
        public void Should_access_no_level_property()
        {
            Expression<Func<A, A>> propertyExpression = (A a) => a;

            PropertyExpressionVisitor visitor = new PropertyExpressionVisitor<A>(null);

            NodeSelector selector = visitor.CreateSelector(propertyExpression.Body);

            selector.ConsoleWriteLine();

            Assert.IsInstanceOf<TypeNodeSelector<A>>(selector);
            Assert.IsNull(selector.Next);
        }

        [Test]
        public void Should_access_first_level_property()
        {
            Expression<Func<A, B>> propertyExpression = (A a) => a.TheB;

            PropertyExpressionVisitor visitor = new PropertyExpressionVisitor<A>(null);

            NodeSelector selector = visitor.CreateSelector(propertyExpression.Body);

            selector.ConsoleWriteLine();

            Assert.IsInstanceOf<TypeNodeSelector<A>>(selector);
            Assert.IsInstanceOf<PropertyNodeSelector<A, B>>(selector.Next);
            Assert.IsNull(selector.Next.Next);
        }

        [Test]
        public void Should_access_fourth_level_property()
        {
            Expression<Func<A, int>> propertyExpression = (A a) => a.TheB.TheC.Value.Length;

            var visitor = new PropertyExpressionVisitor<A>(null);

            NodeSelector selector = visitor.CreateSelector(propertyExpression.Body);

            selector.ConsoleWriteLine();

            Assert.IsInstanceOf<TypeNodeSelector<A>>(selector);
            Assert.IsInstanceOf<PropertyNodeSelector<A, B>>(selector.Next);
            Assert.IsInstanceOf<PropertyNodeSelector<A, B, C>>(selector.Next.Next);
            Assert.IsInstanceOf<PropertyNodeSelector<Token<A, B>, C, string>>(selector.Next.Next.Next);
            Assert.IsInstanceOf<PropertyNodeSelector<Token<Token<A, B>, C>, string, int>>(
                selector.Next.Next.Next.Next);
            Assert.IsNull(selector.Next.Next.Next.Next.Next);
        }


        [Test]
        public void Should_access_second_level_property()
        {
            Expression<Func<A, C>> propertyExpression = (A a) => a.TheB.TheC;

            var visitor = new PropertyExpressionVisitor<A>(null);

            NodeSelector selector = visitor.CreateSelector(propertyExpression.Body);

            selector.ConsoleWriteLine();

            Assert.IsInstanceOf<TypeNodeSelector<A>>(selector);
            Assert.IsInstanceOf<PropertyNodeSelector<A, B>>(selector.Next);
            Assert.IsInstanceOf<PropertyNodeSelector<A, B, C>>(selector.Next.Next);
            Assert.IsNull(selector.Next.Next.Next);
        }

        [Test]
        public void Should_access_third_level_property()
        {
            Expression<Func<A, string>> propertyExpression = (A a) => a.TheB.TheC.Value;

            var visitor = new PropertyExpressionVisitor<A>(null);

            NodeSelector selector = visitor.CreateSelector(propertyExpression.Body);

            selector.ConsoleWriteLine();

            Assert.IsInstanceOf<TypeNodeSelector<A>>(selector);
            Assert.IsInstanceOf<PropertyNodeSelector<A, B>>(selector.Next);
            Assert.IsInstanceOf<PropertyNodeSelector<A, B, C>>(selector.Next.Next);
            Assert.IsInstanceOf<PropertyNodeSelector<Token<A, B>, C, string>>(selector.Next.Next.Next);
            Assert.IsNull(selector.Next.Next.Next.Next);
        }

        [Test]
        public void Should_get_indexers()
        {
            Expression<Func<A, int>> propertyExpression = (A a) => a.TheB.Values[1];

            var visitor = new PropertyExpressionVisitor<A>(null);

            NodeSelector selector = visitor.CreateSelector(propertyExpression.Body);

            selector.ConsoleWriteLine();

            Assert.IsInstanceOf<TypeNodeSelector<A>>(selector);
            Assert.IsInstanceOf<PropertyNodeSelector<A, B>>(selector.Next);
            Assert.IsInstanceOf<PropertyNodeSelector<A, B, int[]>>(selector.Next.Next);
            Assert.IsInstanceOf<ArrayNodeSelector<Token<Token<A, B>, int[]>, int>>(selector.Next.Next.Next);
            Assert.IsNull(selector.Next.Next.Next.Next);
        }

        [Test]
        public void Should_get_list_getters()
        {
            Expression<Func<A, double>> propertyExpression = (A a) => a.Amounts[1];

            var visitor = new PropertyExpressionVisitor<A>(null);

            NodeSelector selector = visitor.CreateSelector(propertyExpression.Body);

            selector.ConsoleWriteLine();

            Assert.IsInstanceOf<TypeNodeSelector<A>>(selector);
            Assert.IsInstanceOf<PropertyNodeSelector<A, IList<double>>>(selector.Next);
            Assert.IsInstanceOf<ListNodeSelector<Token<A, IList<double>>, double>>(selector.Next.Next);
            Assert.IsNull(selector.Next.Next.Next);
        }

        class A
        {
            public B TheB { get; private set; }
            public IList<double> Amounts { get; private set; }
        }

        class B
        {
            public C TheC { get; private set; }
            public int[] Values { get; private set; }
        }

        class C
        {
            public string Value { get; set; }
        }
    }
}