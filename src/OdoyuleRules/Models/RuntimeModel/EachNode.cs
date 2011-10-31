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
    using System;
    using System.Collections;
    using Conditionals;

    public class EachNode<T, TProperty, TElement> :
        NodeImpl<Token<Token<T, TProperty>, Tuple<TElement, int>>>,
        Node<Token<Token<T, TProperty>, Tuple<TElement, int>>>,
        Activation<Token<T, TProperty>>
        where T : class
        where TProperty : class, IEnumerable
    {
        readonly Action<TProperty, Action<TElement, int>> _elementMatch;
        readonly TokenValueFactory<T, TProperty> _tokenValue;

        public EachNode(int id,
                        TokenValueFactory<T, TProperty> tokenValue,
                        Action<TProperty, Action<TElement, int>> elementMatch)
            : base(id)
        {
            _tokenValue = tokenValue;
            _elementMatch = elementMatch;
        }

        public void Activate(ActivationContext<Token<T, TProperty>> context)
        {
            Value<TProperty> leftValue = _tokenValue.GetValue(context.Fact);

            leftValue.Match(value =>
                {
                    if (value == null) 
                        return;

                    _elementMatch(context.Fact.Item2, (element, index) =>
                        {
                            Tuple<TElement, int> item2 = Tuple.Create(element, index);

                            var fact = new Token<Token<T, TProperty>, Tuple<TElement, int>>(context, item2);

                            ActivationContext<Token<Token<T, TProperty>, Tuple<TElement, int>>> propertyContext =
                                context.CreateContext(fact);

                            base.Activate(propertyContext);
                        });
                });
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, Successors);
        }
    }
}