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

    public abstract class Node<T>
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


        public virtual void Activate(ActivationContext<T> context)
        {
            Add(context);
        }

        public void RightActivate(ActivationContext session, Func<ActivationContext<T>, bool> callback)
        {
            All(session, callback);
        }

        public void RightActivate(ActivationContext<T> context, Action<ActivationContext<T>> callback)
        {
            Any(context, callback);
        }

        protected void Add(ActivationContext<T> context)
        {
            context.Access<T>(_id, x=> x.Add(context));
        }

        protected void All(ActivationContext context, Func<ActivationContext<T>, bool> callback)
        {
            context.Access<T>(_id, x => x.All(callback));
        }

        protected void Any(ActivationContext<T> match, Action<ActivationContext<T>> callback)
        {
            match.Access<T>(_id, x => x.Any(match, callback));
        }

        public void AddActivation(Activation<T> activation)
        {
            _successors.Add(activation);

            // TODO: any existing working memories will not be activated based on this change...

//            _contexts.All(context =>
//                {
//                    activation.Activate(context);
//
//                    return true;
//                });
        }

        public void RemoveActivation(Activation<T> activation)
        {
            _successors.Remove(activation);
        }
    }
}