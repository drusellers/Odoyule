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
    using System;
    using System.Collections.Generic;
    using Models.RuntimeModel;
    using Visualization;

    public class EachNodeSelectorFactory<TValue, TElement> :
        NodeSelectorFactory
    {
        readonly RuntimeConfigurator _configurator;
        readonly NodeSelectorFactory _nextFactory;

        public EachNodeSelectorFactory(NodeSelectorFactory nextFactory,
                                       RuntimeConfigurator configurator)
        {
            _nextFactory = nextFactory;
            _configurator = configurator;
        }

        public NodeSelector Create<T>()
            where T : class
        {
            NodeSelector next = null;
            if (_nextFactory != null)
                next = _nextFactory.Create<Token<T, Tuple<TElement, int>>>();

            if (typeof (T).IsGenericType && typeof (T).GetGenericTypeDefinition() == typeof (Token<,>))
            {
                Type[] arguments = typeof (T).GetGenericArguments();
                if (arguments[1] != typeof (TValue))
                    throw new ArgumentException("Value type does not match token type");

                if (typeof (IList<TElement>).IsAssignableFrom(typeof (TValue)))
                {
                    Type elementType = typeof (TValue).GetGenericArguments()[0];
                    if(elementType != typeof(TElement))
                        throw new ArgumentException("Element type does not match list element type");

                    Type nodeType = typeof (ListEachNodeSelector<,,>).MakeGenericType(arguments[0], arguments[1],
                                                                                      elementType);

                    var selector = (NodeSelector) Activator.CreateInstance(nodeType, next, _configurator);

                    return selector;
                }

                throw new ArgumentException("Unknown enumerable type: " + typeof (T).GetShortName());
            }

            throw new ArgumentException("Type was not a token type: " + typeof (T).FullName);
        }
    }
}