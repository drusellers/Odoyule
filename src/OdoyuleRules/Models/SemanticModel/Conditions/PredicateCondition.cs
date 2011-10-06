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

    /// <summary>
    /// A predicate condition is a very primitive version of a predicate that given an object
    /// returns true. There is no safety checking at this point. It is preferable to build
    /// out a property match node and then have a predicate on that match node instead of 
    /// referencing an object property directly since it might be null.
    /// </summary>
    /// <typeparam name="T">The fact type for the expression</typeparam>
    public class PredicateCondition<T> :
        RuleCondition<T>,
        IEquatable<PredicateCondition<T>>
        where T : class
    {
        readonly Func<T, bool> _predicate;

        public PredicateCondition(Func<T, bool> predicate)
        {
            _predicate = predicate;
        }

        public Func<T, bool> Predicate
        {
            get { return _predicate; }
        }

        public bool Equals(PredicateCondition<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._predicate, _predicate);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (PredicateCondition<T>)) return false;
            return Equals((PredicateCondition<T>) obj);
        }

        public override int GetHashCode()
        {
            return (_predicate != null ? _predicate.GetHashCode() : 0);
        }

        public bool Accept(SemanticModelVisitor visitor)
        {
            return visitor.Visit(this, x => true);
        }

        public static bool operator ==(PredicateCondition<T> left, PredicateCondition<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PredicateCondition<T> left, PredicateCondition<T> right)
        {
            return !Equals(left, right);
        }
    }
}