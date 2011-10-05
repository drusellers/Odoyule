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

    public class EqualNodeLocator<T, TProperty> :
        NodeLocator
        where T : class
    {
        readonly RuntimeConfigurator _configurator;
        readonly PropertyInfo _propertyInfo;
        EqualNode<T, TProperty> _node;

        public EqualNodeLocator(RuntimeConfigurator configurator, PropertyInfo propertyInfo)
        {
            _configurator = configurator;
            _propertyInfo = propertyInfo;
        }

        public void Find(Action<EqualNode<T, TProperty>> callback)
        {
            if (_node == null)
            {
                if (typeof (T).IsGenericType && typeof (T).GetGenericTypeDefinition() == typeof (Token<,>))
                {
                }
                else
                {
                    var locator = new PropertyNodeLocator<T, TProperty>(_configurator, _propertyInfo);
                    locator.Find(propertyNode =>
                        {
                            propertyNode.Accept(this);
                            if (_node == null)
                            {
                                _node = _configurator.Equal<T, TProperty>();
                                propertyNode.AddActivation(_node);
                            }
                        });
                }
            }

            if (_node != null)
                callback(_node);
        }

        public override bool Visit<TT, TTProperty>(EqualNode<TT, TTProperty> node, Func<RuntimeModelVisitor, bool> next)
        {
            var locator = this as EqualNodeLocator<TT, TTProperty>;
            if(locator != null)
            {
                locator._node = node;
                return false;
            }

            return base.Visit(node, next);
        }
    }
}