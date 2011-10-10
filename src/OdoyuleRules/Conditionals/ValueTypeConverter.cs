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
    using System.ComponentModel;

    public class ValueTypeConverter<T, TInput> :
        Value<T>
    {
        readonly Value<TInput> _input;
        Func<TInput, T> _converter;

        public ValueTypeConverter(Value<TInput> input)
        {
            _input = input;
            _converter = CreateTypeConverter();
        }

        public void Match(Action<T> valueCallback)
        {
            _input.Match(value =>
                {
                    T matchedValue = _converter(value);
                    valueCallback(matchedValue);
                });
        }

        Func<TInput, T> CreateTypeConverter()
        {
            TypeConverter fromConverter = TypeDescriptor.GetConverter(typeof (TInput));
            if (fromConverter != null && fromConverter.CanConvertTo(typeof (T)))
            {
                return x => (T) fromConverter.ConvertTo(x, typeof (T));
            }

            TypeConverter toConverter = TypeDescriptor.GetConverter(typeof (T));
            if (toConverter != null && toConverter.CanConvertFrom(typeof (TInput)))
            {
                return x => (T) toConverter.ConvertFrom(x);
            }

            throw new RulesEngineException("Could not create a type converter for " + typeof (TInput).Name);
        }
    }
}