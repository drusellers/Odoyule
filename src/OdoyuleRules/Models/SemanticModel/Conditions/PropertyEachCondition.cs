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
namespace OdoyuleRules.Models.SemanticModel
{
    using System;
    using System.Collections;
    using System.Linq.Expressions;
    using System.Reflection;

    public class PropertyEachCondition<T, TProperty, TElement> :
        PropertyCondition<T, TProperty>,
        RuleCondition<T>,
        IEquatable<PropertyEachCondition<T, TProperty, TElement>>
        where T : class
        where TProperty : class, IEnumerable
    {
        public PropertyEachCondition(PropertyInfo propertyInfo, Expression<Func<T, TProperty>> propertyExpression)
            : base(propertyInfo, propertyExpression)
        {
        }

        public bool Equals(PropertyEachCondition<T, TProperty, TElement> other)
        {
            return true;
        }

        public bool Accept(SemanticModelVisitor visitor)
        {
            return visitor.Visit(this, x => true);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as PropertyEachCondition<T, TProperty, TElement>);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}