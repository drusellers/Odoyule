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
    using System.Threading;
    using Internal.Caching;

    class FactCache
    {
        readonly Cache<int, FactHandle> _cache;
        int _key;

        public FactCache()
        {
            _cache = new ConcurrentCache<int, FactHandle>();
        }

        public FactHandle Add<T>(ActivationContext<T> fact)
            where T : class
        {
            if (fact == null)
                throw new ArgumentNullException("fact");

            int factId = Interlocked.Increment(ref _key);

            var factHandle = new FactHandleImpl<T>(factId, fact, id => _cache.Remove(id));

            _cache.Add(factId, factHandle);

            return factHandle;
        }

        class FactHandleImpl<T> :
            FactHandle
            where T : class
        {
            readonly ActivationContext<T> _fact;
            readonly int _factId;
            readonly Action<int> _removeCallback;

            public FactHandleImpl(int factId, ActivationContext<T> fact, Action<int> removeCallback)
            {
                _factId = factId;
                _fact = fact;
                _removeCallback = removeCallback;
            }

            public void Remove()
            {
                _removeCallback(_factId);
            }
        }
    }
}