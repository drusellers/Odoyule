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
namespace OdoyuleRules.Tests
{
    using System;
    using System.Reflection;

    public class PropertyNodeSelector<T, TProperty> :
        NodeSelector
    {
        readonly NodeSelector _next;

        public PropertyNodeSelector(NodeSelector next, PropertyInfo property)
        {
            _next = next;

            NodeType = typeof (T);
            Property = property;
        }

        public PropertyInfo Property { get; private set; }
        public Type NodeType { get; private set; }

        public NodeSelector Next
        {
            get { return _next; }
        }

        public override string ToString()
        {
            return string.Format("Property: [{0}], {1} => {2}", typeof (T).Tokens(), Property.Name,
                                 typeof (TProperty).Name);
        }
    }
}