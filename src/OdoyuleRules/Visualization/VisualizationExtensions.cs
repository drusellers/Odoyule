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
namespace OdoyuleRules.Visualization
{
    using System;
    using System.Linq;
    using Models.RuntimeModel;

    public static class VisualizationExtensions
    {
        public static string Tokens(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Token<,>))
            {
                Type[] arguments = type.GetGenericArguments();

                return string.Join(",", Tokens(arguments[0]), arguments[1].GetShortName());
            }

            return type.Name;
        }

        public static string GetShortName(this Type type)
        {
            if (type.IsGenericType)
            {
                string name = type.GetGenericTypeDefinition().Name;
                name = name.Substring(0, name.IndexOf('`'));

                Type[] arguments = type.GetGenericArguments();
                string innerTypeName = string.Join(",", arguments.Select(x => x.GetShortName()).ToArray());

                return string.Format("{0}<{1}>", name, innerTypeName);
            }

            return type.Name;
        }
    }
}