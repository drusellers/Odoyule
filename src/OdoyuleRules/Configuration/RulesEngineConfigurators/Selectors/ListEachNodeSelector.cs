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
namespace OdoyuleRules.Configuration.RulesEngineConfigurators.Selectors
{
    using System.Collections.Generic;
    using Models.RuntimeModel;

    public class ListEachNodeSelector<T, TProperty, TElement> :
        EachNodeSelector<T, TProperty, TElement>
        where T : class
        where TProperty : class, IList<TElement>
    {
        public ListEachNodeSelector(NodeSelector next, RuntimeConfigurator configurator)
            : base(next, configurator)
        {
        }

        protected override EachNode<T, TProperty, TElement> CreateNode()
        {
            return Configurator.Each<T, TProperty, TElement>((list, callback) =>
                {
                    for (int index = 0; index < list.Count; index++)
                    {
                        callback(list[index], index);
                    }
                });
        }
    }
}