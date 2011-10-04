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
    using System.Collections.Generic;
    using System.Text;

    public interface Token
    {
        int Size { get; }
    }

    [Serializable]
    public class Token<T1, T2> :
        IStructuralEquatable,
        IStructuralComparable,
        IComparable,
        Token
        where T1 : class
    {
        readonly ActivationContext<T1> _item1;
        readonly T2 _item2;

        public Token(ActivationContext<T1> item1, T2 item2)
        {
            _item1 = item1;
            _item2 = item2;
        }

        public ActivationContext<T1> Item1
        {
            get { return _item1; }
        }

        public T2 Item2
        {
            get { return _item2; }
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj, Comparer<object>.Default);
        }

        public int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
                return 1;

            var token = other as Token<T1, T2>;
            if (token == null)
                throw new ArgumentException("Incorrect argument type: " + GetType().Name, "other");

            int num = comparer.Compare(_item1, token._item1);
            if (num != 0)
                return num;

            return comparer.Compare(_item2, token._item2);
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (other == null)
                return false;

            var token = other as Token<T1, T2>;
            if (token == null || !comparer.Equals(_item1, token._item1))
                return false;

            return comparer.Equals(_item2, token._item2);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return CombineHashCodes(comparer.GetHashCode(_item1), comparer.GetHashCode(_item2));
        }

        public int Size
        {
            get { return 2; }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj, EqualityComparer<object>.Default);
        }

        public override int GetHashCode()
        {
            return GetHashCode(EqualityComparer<object>.Default);
        }

        static int CombineHashCodes(int h1, int h2)
        {
            return (((h1 << 5) + h1) ^ h2);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("(");
            return ToString(sb);
        }

        string ToString(StringBuilder sb)
        {
            sb.Append(_item1);
            sb.Append(", ");
            sb.Append(_item2);
            sb.Append(")");
            return sb.ToString();
        }
    }
}