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
namespace OdoyuleRules.Models.RuntimeModel
{
    using Conditionals;

    public class CompareNode<T, TProperty> :
        NodeImpl<Token<T, TProperty>>,
        Node<Token<T, TProperty>>
        where T : class
    {
        readonly TokenValueFactory<T, TProperty> _tokenValue;
        readonly Comparator<TProperty, TProperty> _comparator;
        readonly Value<TProperty> _value;

        public CompareNode(int id,
                           TokenValueFactory<T, TProperty> tokenValue,
                           Comparator<TProperty, TProperty> comparator,
                           Value<TProperty> value)
            : base(id)
        {
            _tokenValue = tokenValue;
            _comparator = comparator;
            _value = value;
        }

        public Comparator<TProperty, TProperty> Comparator
        {
            get { return _comparator; }
        }

        public Value<TProperty> Value
        {
            get { return _value; }
        }

        public override void Activate(ActivationContext<Token<T, TProperty>> context)
        {
            var leftValue = _tokenValue.GetValue(context.Fact);

            _comparator.Match(leftValue, _value, (x, y) => base.Activate(context));
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, Successors);
        }
    }
}