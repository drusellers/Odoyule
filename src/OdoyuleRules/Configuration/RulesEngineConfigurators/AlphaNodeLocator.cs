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
    using Models.RuntimeModel;

    public class AlphaNodeLocator<T> :
        RuntimeModelVisitorImpl
        where T : class
    {
        readonly RuntimeConfigurator _configurator;
        readonly Node<T> _start;
        AlphaNode<T> _node;

        public AlphaNodeLocator(RuntimeConfigurator configurator, Node<T> start)
        {
            _configurator = configurator;
            _start = start;
        }

        public void Find(Action<AlphaNode<T>> callback)
        {
            if (_node == null)
            {
                _start.Accept(this);

                if (_node == null)
                {
                    _node = _configurator.CreateNode(id => new AlphaNode<T>(id));

                    _start.AddActivation(_node);
                }
            }

            if (_node != null)
                callback(_node);
        }

        public override bool Visit<TT>(AlphaNode<TT> node, Func<RuntimeModelVisitor, bool> next)
        {
            var locator = this as AlphaNodeLocator<TT>;
            if (locator != null)
            {
                locator._node = node;
                return false;
            }

            return base.Visit(node, next);
        }
    }
}