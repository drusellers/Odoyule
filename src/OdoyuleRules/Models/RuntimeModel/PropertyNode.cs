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
    using System;
    using System.Linq;

    /// <summary>
    /// A property node matches a property on a fact and activates successors
    /// with a tuple of the fact and the property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public class PropertyNode<T, TProperty> :
        Activation<T>
        where T : class
    {
        readonly int _id;
        readonly Action<T, Action<TProperty>> _propertyMatch;
        readonly ActivationList<Tuple<T, TProperty>> _successors;

        public PropertyNode(int id, Action<T, Action<TProperty>> propertyMatch)
        {
            _id = id;
            _propertyMatch = propertyMatch;

            _successors = new ActivationList<Tuple<T, TProperty>>();
        }

        public int Id
        {
            get { return _id; }
        }

        public void Activate(ActivationContext<T> context)
        {
            _propertyMatch(context.Fact, property =>
                {
                    ActivationContext<Tuple<T, TProperty>> propertyContext =
                        context.CreateContext(new Tuple<T, TProperty>(context.Fact, property));

                    _successors.All(x => x.Activate(propertyContext));
                });
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, x => Enumerable.All(_successors, activation => activation.Accept(x)));
        }

        public void AddActivation(Activation<Tuple<T, TProperty>> activation)
        {
            _successors.Add(activation);
        }

        public void RemoveActivation(Activation<Tuple<T, TProperty>> activation)
        {
            _successors.Remove(activation);
        }
    }
}