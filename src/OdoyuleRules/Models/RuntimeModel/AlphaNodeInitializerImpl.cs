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

    class AlphaNodeInitializerImpl<T> :
        AlphaNodeInitializer
    {
        public void AddActivation<TParent>(OdoyuleRulesEngine rulesEngine, AlphaNode<TParent> activation)
            where TParent : class
        {
            AlphaNode<TParent> alphaNode = rulesEngine.GetAlphaNode<TParent>();

            Type convertNodeType = typeof (ConvertNode<,>).MakeGenericType(typeof (TParent), typeof (T));
            var adapter = (Activation<TParent>) Activator.CreateInstance(convertNodeType, alphaNode);

            activation.AddActivation(adapter);
        }
    }
}