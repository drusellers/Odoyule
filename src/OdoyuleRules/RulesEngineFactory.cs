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
namespace OdoyuleRules
{
    using System;
    using Configuration;
    using Configuration.Configurators;
    using Configuration.RulesEngineConfigurators;

    public static class RulesEngineFactory
    {
        public static RulesEngine New(Action<RulesEngineConfigurator> configureCallback)
        {
            if (configureCallback == null)
                throw new ArgumentNullException("configureCallback");

            var configurator = new RulesEngineConfiguratorImpl();

            configureCallback(configurator);


            ConfigurationResult result = ConfigurationResultImpl.CompileResults(configurator.ValidateConfiguration());

            try
            {
                RulesEngine engine = configurator.Create();

                return engine;
            }
            catch (Exception ex)
            {
                throw new ConfigurationException(result, "An exception was thrown during rules engine creation", ex);
            }
        }
    }
}