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
    using Models.RuntimeModel;
    using Visualization;

    public class TypeNodeSelector<T> :
        NodeSelector
        where T : class
    {
        readonly NodeSelector _next;
        readonly RuntimeConfigurator _configurator;

        public TypeNodeSelector(NodeSelector next, RuntimeConfigurator configurator)
        {
            _next = next;
            _configurator = configurator;
        }

        public NodeSelector Next
        {
            get { return _next; }
        }

        public void Select()
        {
            AlphaNode<T> alphaNode = _configurator.GetAlphaNode<T>();
            _next.Select(alphaNode);
        }

        public void Select<TNode>(Node<TNode> node) where TNode : class
        {
            throw new NotImplementedException("A multi-type node must be in the beta network, and is not supported yet.");
        }

        public void Select<TNode>(MemoryNode<TNode> node) where TNode : class
        {
            throw new NotImplementedException("A multi-type node must be in the beta network, and is not supported yet.");
        }

        public override string ToString()
        {
            return string.Format("Type: [{0}]", typeof (T).Tokens());
        }
    }
}