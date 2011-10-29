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
    using Conditionals;
    using Models.RuntimeModel;

    public class CompareNodeSelectorFactory<TValue> :
        NodeSelectorFactory
    {
        readonly Comparator<TValue, TValue> _comparator;
        readonly NodeSelectorFactory _nextFactory;
        readonly Value<TValue> _value;
        RuntimeConfigurator _configurator;

        public CompareNodeSelectorFactory(NodeSelectorFactory nextFactory,
                                          RuntimeConfigurator configurator,
                                          Comparator<TValue, TValue> comparator,
                                          Value<TValue> value)
        {
            _nextFactory = nextFactory;
            _value = value;
            _configurator = configurator;
            _comparator = comparator;
        }

        public NodeSelector Create<T>()
            where T : class
        {
            NodeSelector next = null;
            if (_nextFactory != null)
                next = _nextFactory.Create<T>();

            if (typeof (T).IsGenericType && typeof (T).GetGenericTypeDefinition() == typeof (Token<,>))
            {
                Type[] arguments = typeof (T).GetGenericArguments();
                if (arguments[1] != typeof (TValue))
                    throw new ArgumentException("Value type does not match token type");

                Type nodeType = typeof (CompareNodeSelector<,>).MakeGenericType(arguments);

                var selector = (NodeSelector) Activator.CreateInstance(nodeType, next, _configurator, _comparator, _value);

                return selector;
            }

            throw new ArgumentException("Type was not a token type: " + typeof (T).FullName);
        }
    }
}