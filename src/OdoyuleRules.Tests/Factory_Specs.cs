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
    public class When_creating_a_rules_engine_using_the_factory
    {
        [Test]
        public void Should_be_able_to_create_a_session()
        {
            using (StatefulSession session = _engine.CreateSession())
            {
                session.Add(new A());

                session.Run();
            }
        }

        [Test]
        public void Should_create_a_rules_engine_instance()
        {
            Assert.IsNotNull(_engine);
        }

        RulesEngine _engine;

        [TestFixtureSetUp]
        public void Setup()
        {
            _engine = RulesEngineFactory.New(x => { });
        }

        class A
        {
        }
    }
}