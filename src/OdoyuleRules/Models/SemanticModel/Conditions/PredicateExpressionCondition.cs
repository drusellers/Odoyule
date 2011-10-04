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
namespace OdoyuleRules.Models.SemanticModel.Conditions
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// A predicate condition is a very primitive version of a predicate that given an object
    /// returns true. There is no safety checking at this point. It is preferable to build
    /// out a property match node and then have a predicate on that match node instead of 
    /// referencing an object property directly since it might be null.
    /// </summary>
    /// <typeparam name="T">The fact type for the expression</typeparam>
    public class PredicateExpressionCondition<T> :
        PredicateCondition<T>
    {
        readonly Expression<Func<T, bool>> _predicateExpression;

        public PredicateExpressionCondition(Expression<Func<T, bool>> predicateExpression)
            : base(predicateExpression.Compile())
        {
            _predicateExpression = predicateExpression;
        }

        public Expression<Func<T, bool>> PredicateExpression
        {
            get { return _predicateExpression; }
        }
    }
}