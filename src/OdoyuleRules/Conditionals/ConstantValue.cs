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
namespace OdoyuleRules.Conditionals
{
    using System;

    public class ConstantValue<T> :
        Value<T>, IEquatable<ConstantValue<T>>
        where T : struct
    {
        readonly T _value;

        public ConstantValue(T value)
        {
            _value = value;
        }

        public void Match(Action<T> valueCallback)
        {
            valueCallback(_value);
        }

        public bool Equals(ConstantValue<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._value.Equals(_value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ConstantValue<T>)) return false;
            return Equals((ConstantValue<T>) obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(ConstantValue<T> left, ConstantValue<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ConstantValue<T> left, ConstantValue<T> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}