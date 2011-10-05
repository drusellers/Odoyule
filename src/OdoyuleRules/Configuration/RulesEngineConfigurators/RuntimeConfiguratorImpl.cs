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
    using System.Threading;
    using Models.RuntimeModel;

    public class RuntimeConfiguratorImpl :
        RuntimeConfigurator
    {
        int _nextNodeId;
        OdoyuleRulesEngine _rulesEngine;

        public RuntimeConfiguratorImpl()
        {
            _rulesEngine = new OdoyuleRulesEngine(this);
        }

        public RulesEngine RulesEngine
        {
            get { return _rulesEngine; }
        }

        public T CreateNode<T>(Func<int, T> nodeFactory)
        {
            int nodeId = Interlocked.Increment(ref _nextNodeId);

            return nodeFactory(nodeId);
        }

        public T CreateNode<T>(Func<T> nodeFactory)
        {
            return nodeFactory();
        }

        public AlphaNode<T> GetAlphaNode<T>()
            where T : class
        {
            return _rulesEngine.GetAlphaNode<T>();
        }

        public void MatchPropertyNode<T, TProperty>(PropertyInfo propertyInfo,
                                                    Action<PropertyNode<T, TProperty>> callback)
            where T : class
        {
            var locator = new PropertyNodeLocator<T, TProperty>(this, propertyInfo);
            locator.Find(callback);
        }

        public void MatchEqualNode<T, TProperty>(PropertyInfo propertyInfo,
                                                 Action<EqualNode<T, TProperty>> callback)
            where T : class
        {
            var locator = new EqualNodeLocator<T, TProperty>(this, propertyInfo);
            locator.Find(callback);
        }
    }
}