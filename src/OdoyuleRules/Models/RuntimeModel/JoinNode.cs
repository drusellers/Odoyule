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
    using System.Linq;

    public class JoinNode<T> :
        MemoryNodeImpl<T>,
        MemoryNode<T>
        where T : class
    {
        readonly RightActivation<T> _rightActivation;

        public JoinNode(int id, RightActivation<T> rightActivation)
            : base(id)
        {
            _rightActivation = rightActivation;
        }

        public RightActivation<T> RightActivation
        {
            get { return _rightActivation; }
        }

        public override void Activate(ActivationContext<T> context)
        {
            _rightActivation.RightActivate(context, match => base.Activate(context));
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, next => _rightActivation.Accept(next)
                                               && Successors.All(activation => activation.Accept(next)));
        }
    }
}