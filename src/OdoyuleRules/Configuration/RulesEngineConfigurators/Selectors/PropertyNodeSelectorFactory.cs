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
    using System.Reflection;
    using Models.RuntimeModel;

    public class PropertyNodeSelectorFactory<TProperty> :
        NodeSelectorFactory
    {
        readonly NodeSelectorFactory _nextFactory;
        readonly PropertyInfo _propertyInfo;
        RuntimeConfigurator _configurator;

        public PropertyNodeSelectorFactory(NodeSelectorFactory nextFactory, RuntimeConfigurator configurator, PropertyInfo propertyInfo)
        {
            _nextFactory = nextFactory;
            _configurator = configurator;
            _propertyInfo = propertyInfo;
        }

        public NodeSelector Create<T>()
            where T : class
        {
            NodeSelector next = null;
            if (_nextFactory != null)
                next = _nextFactory.Create<Token<T, TProperty>>();

            if(typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Token<,>))
            {
                var arguments = typeof (T).GetGenericArguments();

                Type tokenType = typeof(PropertyNodeSelector<,,>).MakeGenericType(arguments[0], arguments[1], typeof(TProperty));
                var tokenSelector = (NodeSelector)Activator.CreateInstance(tokenType, next, _configurator, _propertyInfo);

                return tokenSelector;
            }

            Type type = typeof (PropertyNodeSelector<,>).MakeGenericType(typeof (T), typeof (TProperty));
            var selector = (NodeSelector) Activator.CreateInstance(type, next, _configurator, _propertyInfo);

            return selector;
        }
    }
}