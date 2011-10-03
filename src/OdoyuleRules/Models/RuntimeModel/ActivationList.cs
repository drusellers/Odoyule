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
    using System.Collections;
    using System.Collections.Generic;

    public class ActivationList<T> :
        IEnumerable<Activation<T>>
        where T : class
    {
        readonly IList<Activation<T>> _activations;

        public ActivationList(params Activation<T>[] activations)
        {
            _activations = new List<Activation<T>>(activations);
        }

        public IEnumerator<Activation<T>> GetEnumerator()
        {
            return _activations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public void Add(Activation<T> activation)
        {
            _activations.Add(activation);
        }

        public void All(Action<Activation<T>> callback)
        {
            foreach (var activation in this)
                callback(activation);
        }

        public void Remove(Activation<T> activation)
        {
            _activations.Remove(activation);
        }
    }
}