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
namespace OdoyuleRules.Graphing
{
    using System;
    using System.Collections.Generic;
    using Models.RuntimeModel;
    using Util.Caching;

    public class GraphRulesEngineVisitor :
        RuntimeModelVisitorImpl
    {
        readonly List<Edge> _edges = new List<Edge>();
        readonly Stack<Vertex> _stack;
        readonly Cache<int, Vertex> _vertices;

        Vertex _current;

        public GraphRulesEngineVisitor()
        {
            _stack = new Stack<Vertex>();
            _vertices = new DictionaryCache<int, Vertex>();
        }

        public RulesEngineGraph Graph
        {
            get { return new RulesEngineGraph(_vertices, _edges); }
        }

        public override bool Visit(RulesEngine rulesEngine, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(0, id => new Vertex(typeof (RulesEngine), typeof (object), "Rules Engine"));

            return Next(() => base.Visit(rulesEngine, next));
        }

        public override bool Visit<T>(AlphaNode<T> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id, id => new Vertex(typeof (AlphaNode<>), typeof (T), Tokens<T>()));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T, TProperty>(PropertyNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id,
                                     id =>
                                     new Vertex(typeof (PropertyNode<,>), typeof (TProperty), node.PropertyInfo.Name));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T, TProperty>(EqualNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id, id => new Vertex(typeof (EqualNode<,>), typeof (TProperty), "=="));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T, TProperty>(ValueNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id,
                                     id => new Vertex(typeof (ValueNode<,>), typeof (TProperty), node.Value.ToString()));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T>(ConditionNode<T> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id, id => new Vertex(typeof (ConditionNode<>), typeof (T), "~"));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T>(ConstantNode<T> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id, id => new Vertex(typeof (ConstantNode<>), typeof (T), "C"));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_current, _stack.Peek(), _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T>(JoinNode<T> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id, id => new Vertex(typeof (LeftJoinNode<,>), typeof (T), Tokens<T>()));

            _vertices.WithValue(node.RightActivation.Id,
                                right => _edges.Add(new Edge(right, _current, _current.TargetType.Name)));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T, TDiscard>(LeftJoinNode<T, TDiscard> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id, id => new Vertex(typeof (LeftJoinNode<,>), typeof (T), Tokens<T>()));

            _vertices.WithValue(node.RightActivation.Id,
                                right => _edges.Add(new Edge(right, _current, _current.TargetType.Name)));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<TInput, TOutput>(ConvertNode<TInput, TOutput> node,
                                                    Func<RuntimeModelVisitor, bool> next)
        {
            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T>(DelegateProductionNode<T> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id,
                                     id => new Vertex(typeof (DelegateProductionNode<>), typeof (T), Tokens<T>()));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        bool Next(Func<bool> callback)
        {
            if (_current != null)
            {
                _stack.Push(_current);
                bool result = callback();
                _stack.Pop();
                return result;
            }

            return callback();
        }

        string Tokens<T>()
        {
            return Tokens(typeof (T));
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