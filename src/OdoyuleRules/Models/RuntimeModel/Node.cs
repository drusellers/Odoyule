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
    using System.Collections.Generic;

    public interface Node
    {
        int Id { get; }
    }

    public abstract class Node<T> :
        Node
        where T : class
    {
        readonly int _id;
        readonly ActivationList<T> _successors;

        protected Node(int id)
        {
            _id = id;

            _successors = new ActivationList<T>();
        }

        public IEnumerable<Activation<T>> Successors
        {
            get { return _successors; }
        }

        public int Id
        {
            get { return _id; }
        }

        public virtual void Activate(ActivationContext<T> context)
        {
            context.Access<T>(_id, x => x.Activate(context));

            context.Schedule(() => _successors.All(activation => activation.Activate(context)));
        }

        public void RightActivate(ActivationContext context, Func<ActivationContext<T>, bool> callback)
        {
            context.Access<T>(_id, x => x.All(callback));
        }

        public void RightActivate(ActivationContext<T> context, Action<ActivationContext<T>> callback)
        {
            context.Access<T>(_id, x => x.Any(context, callback));
        }

        public void AddActivation(Activation<T> activation)
        {
            _successors.Add(activation);
        }

        public void RemoveActivation(Activation<T> activation)
        {
            _successors.Remove(activation);
        }
    }
}