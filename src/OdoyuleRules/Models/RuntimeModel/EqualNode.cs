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
    using Util.Caching;

    public class EqualNode<T, TProperty> :
        Node<Token<T, TProperty>>,
        Activation<Token<T, TProperty>>
        where T : class
    {
        readonly Cache<TProperty, ActivationList<Token<T, TProperty>>> _values;

        public EqualNode()
        {
            _values = new DictionaryCache<TProperty, ActivationList<Token<T, TProperty>>>(CreateActivationList);
        }

        public override void Activate(ActivationContext<Token<T, TProperty>> context)
        {
            TProperty value = context.Fact.Item2;

            _values.WithValue(value, list => list.All(activation => activation.Activate(context)));
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, Successors);
        }

        static ActivationList<Token<T, TProperty>> CreateActivationList(TProperty value)
        {
            return new ActivationList<Token<T, TProperty>>();
        }

        public void AddActivation(TProperty value, Activation<Token<T, TProperty>> activation)
        {
            _values[value].Add(activation);
        }

        public void RemoveActivation(TProperty value, Activation<Token<T, TProperty>> activation)
        {
            _values[value].Remove(activation);
        }
    }
}