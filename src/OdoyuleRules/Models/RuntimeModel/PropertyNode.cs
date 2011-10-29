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
    using System.Reflection;
    using Util;

    /// <summary>
    /// A property node matches a property on a fact and activates successors
    /// with a tuple of the fact and the property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public class PropertyNode<T, TProperty> :
        NodeImpl<Token<T, TProperty>>,
        Node<Token<T, TProperty>>,
        Activation<T>
        where T : class
    {
        readonly Action<T, Action<TProperty>> _propertyMatch;
        readonly PropertyInfo _property;

        public PropertyNode(int id, PropertyInfo propertyInfo)
            : base(id)
        {
            _property = propertyInfo;

            var fastProperty = new FastProperty<T, TProperty>(propertyInfo);

            _propertyMatch = (x, next) =>
                {
                    if(x != null)
                        next(fastProperty.Get(x));
                };
        }

        public PropertyNode(int id, PropertyInfo propertyInfo, Action<T,Action<TProperty>> propertyMatch)
            : base(id)
        {
            _property = propertyInfo;
            _propertyMatch = propertyMatch;
        }

        public PropertyInfo PropertyInfo
        {
            get { return _property; }
        }

        public void Activate(ActivationContext<T> context)
        {
            _propertyMatch(context.Fact, property =>
                {
                    ActivationContext<Token<T, TProperty>> propertyContext =
                        context.CreateContext(new Token<T, TProperty>(context, property));

                    base.Activate(propertyContext);
                });
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, Successors);
        }
    }
}