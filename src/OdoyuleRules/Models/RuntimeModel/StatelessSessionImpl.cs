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
    using Internal.Caching;

    class StatelessSessionImpl :
        StatelessSession,
        ActivationContext
    {
        readonly FactCache _facts;
        readonly Cache<int, ContextMemory> _memoryCache;
        readonly Cache<Type, ActivationTypeProxy> _objectCache;
        readonly RulesEngine _rulesEngine;

        bool _disposed;
        Agenda _agenda;

        public StatelessSessionImpl(RulesEngine rulesEngine, Cache<Type, ActivationTypeProxy> objectCache)
        {
            _rulesEngine = rulesEngine;
            _objectCache = objectCache;
            _memoryCache = new ConcurrentCache<int, ContextMemory>();
            _facts = new FactCache();
            _agenda = new AgendaImpl();
        }

        public void Access<TMemory>(int id, Action<ContextMemory<TMemory>> callback)
            where TMemory : class
        {
            ContextMemory contextMemory = _memoryCache.Get(id, key => new ContextMemory<TMemory>());

            contextMemory.Access(callback);
        }

        public void Schedule(Action operation, int priority = 0)
        {
            _agenda.Schedule(operation, priority);
        }


        public ActivationContext<T> CreateContext<T>(T fact)
            where T : class
        {
            var context = new StatelessActivationContext<T>(this, fact);

            return context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public FactHandle Add<T>(T fact)
            where T : class
        {
            ActivationContext<T> context = CreateContext(fact);

            _rulesEngine.Activate(context);

            return _facts.Add(context);
        }

        public FactHandle Add(object fact)
        {
            if (fact == null)
                throw new ArgumentNullException("fact");

            Type factType = fact.GetType();
            if (factType.IsValueType || factType.Equals(typeof (string)))
                throw new ArgumentException("Facts must be reference types", "fact");

            return _objectCache[factType].Activate(_rulesEngine, this, _facts, fact);
        }

        public void Run()
        {
            _agenda.Run();
        }

        ~StatelessSessionImpl()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _facts.Clear();
                _memoryCache.Clear();
            }

            _disposed = true;
        }
    }
}