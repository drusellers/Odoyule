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

    public interface RightActivation<T>
        where T : class
    {
        /// <summary>
        /// Performs a join operation between the two messages, performing the callback for
        /// every activation on the right node until the callback returns false
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        void RightActivate(ActivationContext context, Func<ActivationContext<T>, bool> callback);

        /// <summary>
        /// Performs a match operation between two activations, 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        void RightActivate(ActivationContext<T> context, Action<ActivationContext<T>> callback);
    }
}