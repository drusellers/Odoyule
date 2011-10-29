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
    using Compiling;
    using Models.RuntimeModel;
    using Visualization;

    public class ConditionAlphaNodeSelector<T> :
        NodeSelector
        where T : class
    {
        readonly RuntimeConfigurator _configurator;
        readonly Action<ConditionAlphaNode> _nodeCallback;

        public ConditionAlphaNodeSelector(RuntimeConfigurator configurator, Action<ConditionAlphaNode> nodeCallback)
        {
            _configurator = configurator;
            _nodeCallback = nodeCallback;
        }

        public NodeSelector Next
        {
            get { return null; }
        }

        public void Select()
        {
            throw new NotImplementedException();
        }

        public void Select<TNode>(Node<TNode> node)
            where TNode : class
        {
            throw new NotImplementedException();
        }

        public void Select<TNode>(MemoryNode<TNode> node)
            where TNode : class
        {
            var alphaNode = node as AlphaNode<T>;
            if (alphaNode == null)
                throw new ArgumentException("Only alpha nodes can be condition alpha nodes");

            var conditionAlphaNode = new ConditionAlphaNode<T>(_configurator, alphaNode);

            _nodeCallback(conditionAlphaNode);
        }

        public override string ToString()
        {
            return string.Format("Condition Alpha Node: [{0}]", typeof (T).Tokens());
        }
    }
}