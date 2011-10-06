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

    public class DelegateProductionNode<T> :
        Activation<T>
        where T : class
    {
        readonly Action<T> _callback;
        readonly int _id;
        int _priority;

        public DelegateProductionNode(int id, Action<T> callback)
        {
            _id = id;
            _callback = callback;
            _priority = 0;
        }

        public int Id
        {
            get { return _id; }
        }

        public void Activate(ActivationContext<T> context)
        {
            context.Schedule(() => _callback(context.Fact), _priority);
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, next => true);
        }
    }
}