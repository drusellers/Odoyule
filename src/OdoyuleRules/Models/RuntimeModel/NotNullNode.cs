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

    public class NotNullNode<T, TProperty> :
        NodeImpl<Token<T, TProperty>>,
        Node<Token<T, TProperty>>
        where T : class
        where TProperty : class
    {
        readonly TokenValueFactory<T, TProperty> _tokenValue;

        public NotNullNode(int id, TokenValueFactory<T, TProperty> tokenValue)
            : base(id)
        {
            _tokenValue = tokenValue;
        }

        public override void Activate(ActivationContext<Token<T, TProperty>> context)
        {
            Value<TProperty> leftValue = _tokenValue.GetValue(context.Fact);

            leftValue.Match(value =>
                {
                    if (value != null)
                        base.Activate(context);
                });
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, Successors);
        }
    }
}