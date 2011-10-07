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

    public class LeftJoinNode<T, TDiscard> :
        MemoryNodeImpl<T>,
        Activation<Token<T, TDiscard>>,
        MemoryNode<T>,
        RightActivation<Token<T, TDiscard>>
        where T : class
    {
        readonly RightActivation<T> _rightActivation;

        public LeftJoinNode(int id, RightActivation<T> rightActivation)
            : base(id)
        {
            _rightActivation = rightActivation;
        }

        public RightActivation<T> RightActivation
        {
            get { return _rightActivation; }
        }

        public void Activate(ActivationContext<Token<T, TDiscard>> context)
        {
            _rightActivation.RightActivate(context.Fact.Item1, match => base.Activate(context.Fact.Item1));
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, next => _rightActivation.Accept(next)
                                               && Successors.All(activation => activation.Accept(next)));
        }

        public void RightActivate(ActivationContext context,
                                  Func<ActivationContext<Token<T, TDiscard>>, bool> callback)
        {
            context.Access<Token<T, TDiscard>>(Id, x => x.All(callback));
        }

        public void RightActivate(ActivationContext<Token<T, TDiscard>> context,
                                  Action<ActivationContext<Token<T, TDiscard>>> callback)
        {
            context.Access<Token<T, TDiscard>>(Id, x => x.Any(context, callback));
        }
    }
}