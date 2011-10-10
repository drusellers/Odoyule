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
namespace OdoyuleRules.Conditionals
{
    using System;

    /// <summary>
    /// Wraps a value in a callback that is only invoked if the value is actually present
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface Value<out T>
    {
        /// <summary>
        /// Matches the value, if it is present -- sort of like a Maybe
        /// </summary>
        /// <param name="valueCallback">Called if a value is present</param>
        void Match(Action<T> valueCallback);
    }
}