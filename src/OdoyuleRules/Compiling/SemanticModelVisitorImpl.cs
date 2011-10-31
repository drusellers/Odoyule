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
namespace OdoyuleRules.Compiling
{
    using System;
    using System.Collections;
    using Models.SemanticModel;

    public class SemanticModelVisitorImpl :
        SemanticModelVisitor
    {
        public virtual bool Visit(Rule rule, Func<SemanticModelVisitor, bool> next)
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(PropertyEqualCondition<T, TProperty> condition,
                                                Func<SemanticModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public bool Visit<T, TProperty>(PropertyCompareCondition<T, TProperty> condition,
                                        Func<SemanticModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(PropertyNotEqualCondition<T, TProperty> condition,
                                                Func<SemanticModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(PropertyLessThanCondition<T, TProperty> condition,
                                                Func<SemanticModelVisitor, bool> next)
            where T : class
            where TProperty : IComparable<TProperty>
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(PropertyLessThanOrEqualCondition<T, TProperty> condition,
                                                Func<SemanticModelVisitor, bool> next)
            where T : class
            where TProperty : IComparable<TProperty>
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(PropertyGreaterThanCondition<T, TProperty> condition,
                                                Func<SemanticModelVisitor, bool> next)
            where T : class
            where TProperty : IComparable<TProperty>
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(PropertyGreaterThanOrEqualCondition<T, TProperty> condition,
                                                Func<SemanticModelVisitor, bool> next)
            where T : class
            where TProperty : IComparable<TProperty>
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(PropertyNotNullCondition<T, TProperty> condition,
                                                Func<SemanticModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(PropertyExistsCondition<T, TProperty> condition,
                                                Func<SemanticModelVisitor, bool> next)
            where T : class
            where TProperty : class, IEnumerable
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty, TElement>(PropertyEachCondition<T, TProperty, TElement> condition,
                                                          Func<SemanticModelVisitor, bool> next)
            where T : class
            where TProperty : class, IEnumerable
        {
            return next(this);
        }

        public virtual bool Visit<T>(PredicateCondition<T> condition, Func<SemanticModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T>(DelegateConsequence<T> consequence, Func<SemanticModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }
    }
}