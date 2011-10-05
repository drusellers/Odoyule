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
namespace OdoyuleRules.Configuration.RulesEngineConfigurators
{
    using System;
    using System.Reflection;
    using Models.RuntimeModel;

    public class PropertyNodeLocator<T, TProperty> :
        NodeLocator
        where T : class
    {
        readonly RuntimeConfigurator _configurator;
        readonly PropertyInfo _propertyInfo;
        PropertyNode<T, TProperty> _node;

        public PropertyNodeLocator(RuntimeConfigurator configurator, PropertyInfo propertyInfo)
        {
            _configurator = configurator;
            _propertyInfo = propertyInfo;
        }

        public void Find(Action<PropertyNode<T, TProperty>> callback)
        {
            if (_node == null)
            {
                if (typeof (T).IsGenericType && typeof (T).GetGenericTypeDefinition() == typeof (Token<,>))
                {
                }
                else
                {
                    AlphaNode<T> alphaNode = _configurator.GetAlphaNode<T>();
                    alphaNode.Accept(this);

                    if (_node == null)
                    {
                        _node = _configurator.Property<T, TProperty>(_propertyInfo);
                        alphaNode.AddActivation(_node);
                    }
                }
            }

            if (_node != null)
                callback(_node);
        }

        public override bool Visit<TT, TTProperty>(PropertyNode<TT, TTProperty> node,
                                                   Func<RuntimeModelVisitor, bool> next)
        {
            var locator = this as PropertyNodeLocator<TT, TTProperty>;
            if (locator != null)
            {
                if (locator._propertyInfo.Equals(node.PropertyInfo))
                {
                    locator._node = node;
                    return false;
                }
            }

            return base.Visit(node, next);
        }
    }
}