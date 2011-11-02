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
namespace OdoyuleRules
{
    using System;
    using System.Collections.Generic;

    public interface Session :
        IDisposable
    {
        /// <summary>
        /// Adds a fact to the session
        /// </summary>
        /// <typeparam name="T">The type of the fact</typeparam>
        /// <param name="obj">The fact</param>
        /// <returns>A fact handle, which can be used to remove the fact from the session</returns>
        FactHandle<T> Add<T>(T obj)
            where T : class;

        /// <summary>
        /// Adds a fact to the session
        /// </summary>
        /// <param name="fact">The fact</param>
        /// <returns>A fact handle, which can be used to remove the fact from the session</returns>
        FactHandle Add(object fact);

        /// <summary>
        /// Runs the rules until completion
        /// </summary>
        void Run();

        /// <summary>
        /// Returns an enumeration of the facts matching the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<FactHandle<T>> Facts<T>()
            where T : class;
    }
}