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

    public interface ContextMemory
    {
        void Access<T>(Action<ContextMemory<T>> callback)
            where T : class;
    }

    public class ContextMemory<T> :
        ContextMemory
        where T : class
    {
        readonly IList<ActivationContext<T>> _contexts;
        readonly IList<Func<ActivationContext<T>, bool>> _joins;

        public ContextMemory()
        {
            _contexts = new List<ActivationContext<T>>();
            _joins = new List<Func<ActivationContext<T>, bool>>();
        }

        public int Count
        {
            get { return _contexts.Count; }
        }

        public void Add(ActivationContext<T> context)
        {
            _contexts.Add(context);

            CallbackPendingJoins(context);
        }

        void CallbackPendingJoins(ActivationContext<T> context)
        {
            for (int i = _joins.Count - 1; i >= 0; i--)
            {
                if (false == _joins[i](context))
                    _joins.RemoveAt(i);
            }
        }

        public void All(Func<ActivationContext<T>, bool> callback)
        {
            Join(callback);
        }

        public void Any(ActivationContext<T> match, Action<ActivationContext<T>> callback)
        {
            Join(context =>
                {
                    if (match.Equals(context))
                    {
                        callback(context);
                        return false;
                    }

                    return true;
                });
        }

        void Join(Func<ActivationContext<T>, bool> callback)
        {
            for (int i = 0; i < _contexts.Count; i++)
            {
                bool result = callback(_contexts[i]);
                if (result == false)
                    return;
            }

            _joins.Add(callback);
        }

        public void Access<TAccess>(Action<ContextMemory<TAccess>> callback) 
            where TAccess : class
        {
            var self = this as ContextMemory<TAccess>;
            if (self == null)
                throw new ArgumentException("Requested type " + typeof (TAccess).Name + " does not match "
                                            + typeof (T).Name);

            callback(self);
        }
    }
}