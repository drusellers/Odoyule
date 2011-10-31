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

    public class LessThanValueComparator<T> :
        ValueComparator<T>,
        IEquatable<LessThanValueComparator<T>>
        where T : IComparable<T>
    {
        public bool Equals(LessThanValueComparator<T> other)
        {
            return !ReferenceEquals(null, other);
        }

        protected override bool Compare(T left, T right)
        {
            return left.CompareTo(right) < 0;
        }

        public override string ToString()
        {
            return "<";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (LessThanValueComparator<T>)) return false;
            return Equals((LessThanValueComparator<T>) obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}