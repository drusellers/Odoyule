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
    using System.Reflection;

    public class PropertyGreaterThanOrEqualCondition<T, TProperty> :
        PropertyCondition<T, TProperty>,
        RuleCondition<T>, 
        IEquatable<PropertyGreaterThanOrEqualCondition<T, TProperty>>
        where T : class
    {
        readonly TProperty _value;

        public PropertyGreaterThanOrEqualCondition(PropertyInfo propertyInfo, TProperty value)
            : base(propertyInfo)
        {
            _value = value;
        }

        public TProperty Value
        {
            get { return _value; }
        }

        public bool Equals(PropertyGreaterThanOrEqualCondition<T, TProperty> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other._value, _value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as PropertyGreaterThanOrEqualCondition<T, TProperty>);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ _value.GetHashCode();
            }
        }

        public bool Accept(SemanticModelVisitor visitor)
        {
            return visitor.Visit(this, x => true);
        }


        public static bool operator ==(
            PropertyGreaterThanOrEqualCondition<T, TProperty> left,
            PropertyGreaterThanOrEqualCondition<T, TProperty> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(
            PropertyGreaterThanOrEqualCondition<T, TProperty> left,
            PropertyGreaterThanOrEqualCondition<T, TProperty> right)
        {
            return !Equals(left, right);
        }
    }
}