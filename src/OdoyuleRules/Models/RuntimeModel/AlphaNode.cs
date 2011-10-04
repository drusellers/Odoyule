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
    using System.Linq;


    public class AlphaNode<T> :
        Node<T>,
        Activation,
        Activation<T>,
        RightActivation<T>
        where T : class
    {
        public AlphaNode(int id)
            : base(id)
        {
        }

        public void Activate<TActivation>(ActivationContext<TActivation> context)
            where TActivation : class
        {
            var self = this as Activation<TActivation>;
            if (self == null)
                throw new ArgumentException("The activation type of " + typeof (TActivation).Name
                                            + " did not match the alpha node type of " + typeof (T).Name);

            self.Activate(context);
        }

        public bool Accept(RuntimeModelVisitor visitor)
        {
            return visitor.Visit(this, next => Successors.All(activation => activation.Accept(next)));
        }
    }
}