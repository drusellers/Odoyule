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
    using System.Text;
    using Models.RuntimeModel;

    public class TextRuntimeModelVisitor :
        RuntimeModelVisitor
    {
        const int PaddingWidth = 2;
        int _depth;
        string _padding;
        StringBuilder _sb;

        public TextRuntimeModelVisitor()
        {
            _sb = new StringBuilder();

            _depth = 0;
            _padding = "";
        }

        public virtual bool Visit(RulesEngine rulesEngine, Func<RuntimeModelVisitor, bool> next)
        {
            Append("Rules Engine");

            return Indent(next);
        }

        public virtual bool Visit<T>(MemoryNode<T> node, Func<RuntimeModelVisitor, bool> next) where T : class
        {
            Append("MemoryNode<{0}>", typeof (T).Name);

            return Indent(next);
        }

        public virtual bool Visit<T>(AlphaNode<T> node, Func<RuntimeModelVisitor, bool> next) where T : class
        {
            Append("AlphaNode<{0}>", typeof (T).Name);

            return Indent(next);
        }

        public virtual bool Visit<TInput, TOutput>(ConvertNode<TInput, TOutput> node,
                                                   Func<RuntimeModelVisitor, bool> next) where TInput : class, TOutput
            where TOutput : class
        {
            Append("ConvertNode<{0}> => {1}", typeof (TInput).Name, typeof (TOutput).Name);

            return Indent(next);
        }

        public virtual bool Visit<T>(DelegateProductionNode<T> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
        {
            Append("DelegateProductionNode<{0}>", typeof (T).Name);

            return Indent(next);
        }

        public virtual bool Visit<T, TProperty>(PropertyNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
            where T : class
        {
            Append("PropertyNode<{0}>.{1}", typeof (T).Name, typeof (TProperty).Name);

            return Indent(next);
        }

        public virtual bool Visit<T>(ConstantNode<T> node, Func<RuntimeModelVisitor, bool> next) where T : class
        {
            Append("v<{0}>", typeof (T).Name);

            return Indent(next);
        }

        public virtual bool Visit<T>(ConditionNode<T> node, Func<RuntimeModelVisitor, bool> next) where T : class
        {
            Append("ConditionNode<{0}>", typeof (T).Name);

            return Indent(next);
        }

        public virtual bool Visit<T, TProperty>(EqualNode<T, TProperty> rulesEngine,
                                                Func<RuntimeModelVisitor, bool> next) where T : class
        {
            Append("EqualNode<{0}>.{1}", typeof (T).Name, typeof (TProperty).Name);

            return Indent(next);
        }

        void Append(string format, params object[] args)
        {
            Append(string.Format(format, args));
        }

        void Append(string text)
        {
            _sb.Append(_padding).AppendLine(text);
        }

        bool Indent(Func<RuntimeModelVisitor, bool> next)
        {
            _depth++;
            string previous = _padding;
            _padding = new string(' ', _depth*PaddingWidth);

            bool result = next(this);

            _depth--;
            _padding = previous;

            return result;
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}