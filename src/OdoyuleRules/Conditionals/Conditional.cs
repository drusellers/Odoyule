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
namespace OdoyuleRules
{
    using System;
    using Conditionals;
    using Models.RuntimeModel;

    public static class Conditional
    {
        public static Value<T> Constant<T>(T value)
        {
            Type underlyingType = Nullable.GetUnderlyingType(typeof (T));
            if (underlyingType != null)
            {
                Type nullableType = typeof (NullableConstantValue<>).MakeGenericType(underlyingType);
                return (Value<T>) Activator.CreateInstance(nullableType, value);
            }

            if (typeof (T).IsValueType)
            {
                Type constantType = typeof(ConstantValue<>).MakeGenericType(typeof(T));
                return (Value<T>)Activator.CreateInstance(constantType, value);
            }

            Type referenceType = typeof(ReferenceConstantValue<>).MakeGenericType(typeof(T));
            return (Value<T>)Activator.CreateInstance(referenceType, value);
        }

        public static TokenValueFactory<T,TProperty> Property<T, TProperty>() 
            where T : class
        {
            Type underlyingType = Nullable.GetUnderlyingType(typeof(TProperty));
            if (underlyingType != null)
            {
                Type constantType = typeof(NullableTokenValueFactory<,>).MakeGenericType(typeof(T), typeof(TProperty));
                return (TokenValueFactory<T, TProperty>)Activator.CreateInstance(constantType);
            }

            if (typeof(TProperty).IsValueType)
            {
                Type constantType = typeof(ValueTypeTokenValueFactory<,>).MakeGenericType(typeof(T), typeof(TProperty));
                return (TokenValueFactory<T, TProperty>)Activator.CreateInstance(constantType);
            }

            Type referenceType = typeof(ReferenceTypeTokenValueFactory<,>).MakeGenericType(typeof(T), typeof(TProperty));
            return (TokenValueFactory<T, TProperty>)Activator.CreateInstance(referenceType);
        }
    }
}