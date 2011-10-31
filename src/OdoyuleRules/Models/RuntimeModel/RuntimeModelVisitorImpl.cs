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
    using System.Collections;

    public abstract class RuntimeModelVisitorImpl :
        RuntimeModelVisitor
    {
        public virtual bool Visit(RulesEngine rulesEngine, Func<RuntimeModelVisitor, bool> next)
        {
            return next(this);
        }

        public virtual bool Visit<T>(JoinNode<T> node, Func<RuntimeModelVisitor, bool> next) where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T, TDiscard>(LeftJoinNode<T, TDiscard> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T>(AlphaNode<T> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<TInput, TOutput>(ConvertNode<TInput, TOutput> node,
                                                   Func<RuntimeModelVisitor, bool> next)
            where TInput : class, TOutput
            where TOutput : class
        {
            return next(this);
        }

        public virtual bool Visit<T>(DelegateProductionNode<T> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(PropertyNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T>(ConstantNode<T> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T>(ConditionNode<T> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(EqualNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(ValueNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(CompareNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(NotNullNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
            where TProperty : class
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty>(ExistsNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
            where TProperty : class, IEnumerable
        {
            return next(this);
        }

        public virtual bool Visit<T, TProperty, TElement>(EachNode<T, TProperty, TElement> node,
                                                          Func<RuntimeModelVisitor, bool> next)
            where T : class
            where TProperty : class, IEnumerable
        {
            return next(this);
        }
    }
}