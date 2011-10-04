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

    public interface ActivationContext
    {
        ActivationContext<TContext> CreateContext<TContext>(TContext fact)
            where TContext : class;

        /// <summary>
        /// Provides access to memory storage for nodes
        /// </summary>
        /// <typeparam name="TMemory">The type of memory to access</typeparam>
        /// <param name="id">The identifier for the node</param>
        /// <param name="callback">The callback to access the memory</param>
        void Access<TMemory>(int id, Action<ContextMemory<TMemory>> callback)
            where TMemory : class;

        /// <summary>
        /// Schedule an operation on the agenda for this session
        /// </summary>
        /// <param name="operation">The operation to invoke</param>
        /// <param name="priority">The priority of the operation, should be zero</param>
        void Schedule(Action operation, int priority = 0);
    }


    public interface ActivationContext<T> :
        ActivationContext
        where T : class
    {
        T Fact { get; }

        void Convert<TOutput>(Action<ActivationContext<TOutput>> callback)
            where TOutput : class;
    }
}