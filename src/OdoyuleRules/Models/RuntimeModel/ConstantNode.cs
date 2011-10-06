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

    public class ConstantNode<T> :
        RightActivation<T>
        where T : class
    {
        readonly int _id;

        public ConstantNode(int id)
        {
            _id = id;
        }

        public int Id
        {
            get { return _id; }
        }

        public void RightActivate(ActivationContext context, Func<ActivationContext<T>, bool> callback)
        {
            // constant nodes are never activated by an alpha node, so they would never have pending joins
        }

        public void RightActivate(ActivationContext<T> context, Action<ActivationContext<T>> callback)
        {
            callback(context);
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, x => true);
        }
    }
}