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
    using Visualization;

    public class GraphRulesEngineVisitor :
        RuntimeModelVisitorImpl
    {
        readonly List<Edge> _edges = new List<Edge>();
        readonly Stack<Vertex> _stack;
        readonly Cache<int, Vertex> _vertices;

        Vertex _current;
        int _rightActivation;

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
            _current = _vertices.Get(node.Id, id => new Vertex(typeof (AlphaNode<>), typeof (T), typeof (T).Tokens()));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T, TProperty>(PropertyNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id, id => new Vertex(typeof (PropertyNode<,>), typeof (TProperty),
                                                               typeof (T).Tokens() + "." + node.PropertyInfo.Name));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T, TProperty>(EqualNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
        {
//            _current = _vertices.Get(node.Id, id => new Vertex(typeof (EqualNode<,>), typeof (TProperty), "=="));
//
//            if (_stack.Count > 0)
//                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T, TProperty>(ValueNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id,
                                     id =>
                                     new Vertex(typeof (ValueNode<,>), typeof (TProperty), "== " + node.Value.ToString()));

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

        public override bool Visit<T, TProperty>(CompareNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id, id => new Vertex(typeof (CompareNode<,>), typeof (Token<T, TProperty>),
                                                               string.Format("{0} {1}", node.Comparator, node.Value)));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T, TProperty>(NotNullNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id,
                                     id => new Vertex(typeof (NotNullNode<,>), typeof (Token<T, TProperty>), "not null"));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T, TProperty>(ExistsNode<T, TProperty> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id,
                                     id => new Vertex(typeof (ExistsNode<,>), typeof (Token<T, TProperty>), "exists"));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T, TProperty, TElement>(EachNode<T, TProperty, TElement> node,
                                                           Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id, id => new Vertex(typeof (EachNode<,,>), typeof (TElement),
                                                               typeof (T).Tokens() + "."
                                                               + typeof (TProperty).GetShortName() + "[n]"));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T>(ConstantNode<T> node, Func<RuntimeModelVisitor, bool> next)
        {
            if (!_vertices.Has(node.Id))
            {
                _current = _vertices.Get(node.Id, id => new Vertex(typeof (ConstantNode<>), typeof (T), "" /*"\x22A9"*/));

                if (_stack.Count > 0 && _rightActivation == node.Id)
                    _edges.Add(new Edge(_current, _stack.Peek(), _current.TargetType.Name));
            }

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T>(JoinNode<T> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id, id => new Vertex(typeof (JoinNode<>), typeof (T), typeof (T).Tokens()));

            if (_rightActivation == node.Id)
            {
                _edges.Add(new Edge(_current, _stack.Peek(), _current.TargetType.Name));
            }
            else if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(node.RightActivation.Id, () => base.Visit(node, next));
        }

        public override bool Visit<T, TDiscard>(LeftJoinNode<T, TDiscard> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id,
                                     id => new Vertex(typeof (LeftJoinNode<,>), typeof (T), typeof (T).Tokens()));

            if (_rightActivation == node.Id)
            {
                _edges.Add(new Edge(_current, _stack.Peek(), _current.TargetType.Name));
            }
            else if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(node.RightActivation.Id, () => base.Visit(node, next));
        }

        public override bool Visit<TInput, TOutput>(ConvertNode<TInput, TOutput> node,
                                                    Func<RuntimeModelVisitor, bool> next)
        {
            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T>(DelegateProductionNode<T> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id,
                                     id =>
                                     new Vertex(typeof (DelegateProductionNode<>), typeof (T), typeof (T).Tokens()));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return Next(() => base.Visit(node, next));
        }

        public override bool Visit<T, TFact>(AddFactProductionNode<T, TFact> node, Func<RuntimeModelVisitor, bool> next)
        {
            _current = _vertices.Get(node.Id,
                                     id =>
                                     new Vertex(typeof (AddFactProductionNode<,>), typeof (TFact),
                                                typeof (T).Tokens() + " \x279C " + typeof (TFact).GetShortName()));

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            return base.Visit(node, next);
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

        bool Next(int rightActivation, Func<bool> callback)
        {
            if (_current != null)
            {
                int previous = _rightActivation;
                _rightActivation = rightActivation;
                _stack.Push(_current);
                bool result = callback();
                _stack.Pop();
                _rightActivation = previous;
                return result;
            }

            return callback();
        }
    }
}