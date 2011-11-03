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
namespace OdoyuleRules.Designer
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using Visualization;

    public class RightHandSideExpressionVisitor :
        ExpressionVisitor
    {
        readonly Type _compareType;

        TypeConverter _toConverter;

        Func<object> _value;

        public RightHandSideExpressionVisitor(Type compareType)
        {
            _compareType = compareType;
            _toConverter = TypeDescriptor.GetConverter(_compareType);

            _value = () => { throw new InvalidOperationException("No match was found"); };
        }

        public object Value
        {
            get { return _value(); }
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type == _compareType)
            {
                _value = () => node.Value;
                return node;
            }

            _value = () => ConvertToCompareType(node.Value);
            return node;
        }

        object ConvertToCompareType(object value)
        {
            if (value == null)
                return null;

            Type sourceType = value.GetType();

            TypeConverter fromConverter = TypeDescriptor.GetConverter(sourceType);
            if (fromConverter != null && fromConverter.CanConvertTo(_compareType))
            {
                return fromConverter.ConvertTo(value, _compareType);
            }

            if (_toConverter != null && _toConverter.CanConvertFrom(sourceType))
            {
                return _toConverter.ConvertFrom(value);
            }

            throw new ArgumentException("Could not convert value to compare type: " + sourceType.GetShortName());
        }
    }
}