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
namespace OdoyuleRules.Compiling
{
    using System;
    using Models.RuntimeModel;

    public interface ConditionAlphaNode
    {
        void Select<T>(Action<AlphaNode<T>> callback)
            where T : class;
    }

    public class ConditionAlphaNode<T> :
        ConditionAlphaNode
        where T : class
    {
        AlphaNode<T> _node;


        public ConditionAlphaNode(AlphaNode<T> node)
        {
            _node = node;
        }

        public void Select<TSelect>(Action<AlphaNode<TSelect>> callback)
            where TSelect : class
        {
            var self = this as ConditionAlphaNode<TSelect>;
            if (self != null)
            {
                callback(self._node);
                return;
            }


            if (typeof (T).IsGenericType && typeof (T).GetGenericTypeDefinition() == typeof (Token<,>))
            {
                Type[] arguments = typeof (T).GetGenericArguments();
            }
        }

        string Tokens(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Token<,>))
            {
                Type[] arguments = type.GetGenericArguments();

                return string.Join(",", Tokens(arguments[0]), arguments[1].Name);
            }

            return type.Name;
        }
    }
}