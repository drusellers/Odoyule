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
namespace OdoyuleRules.Conditionals
{
    using System;

    public class ValueEqualComparator<T> :
        Comparator<T, T>
        where T : struct
    {
        public void Match(Value<T> left, Value<T> right, Action<T, T> callback)
        {
            left.Match(x => right.Match(y =>
                {
                    if (x.Equals(y))
                        callback(x, y);
                }));
        }
    }
}