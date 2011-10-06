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
namespace OdoyuleRules.Models.RuntimeModel
{
    using System.Linq;
    using Configuration.RulesEngineConfigurators;
    using Util.Caching;


    public class EqualNode<T, TProperty> :
        Activation<Token<T, TProperty>>
        where T : class
    {
        readonly int _id;
        readonly RuntimeConfigurator _configurator;
        readonly Cache<TProperty, ValueNode<T, TProperty>> _values;

        public int Id
        {
            get { return _id; }
        }

        public EqualNode(int id, RuntimeConfigurator configurator)
        {
            _id = id;
            _configurator = configurator;
            _values = new DictionaryCache<TProperty, ValueNode<T, TProperty>>(CreateValueNode);
        }

        public void Activate(ActivationContext<Token<T, TProperty>> context)
        {
            TProperty value = context.Fact.Item2;

            _values.WithValue(value, node => node.Activate(context));
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, x => _values.All(value => value.Accept(x)));
        }

        ValueNode<T, TProperty> CreateValueNode(TProperty value)
        {
            return _configurator.CreateNode(id => new ValueNode<T, TProperty>(id,value));
        }

        public void AddActivation(TProperty value, Activation<Token<T, TProperty>> activation)
        {
            _values[value].AddActivation(activation);
        }

        public void RemoveActivation(TProperty value, Activation<Token<T, TProperty>> activation)
        {
            _values[value].RemoveActivation(activation);
        }
    }
}