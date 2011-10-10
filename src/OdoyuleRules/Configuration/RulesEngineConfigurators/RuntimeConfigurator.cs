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
namespace OdoyuleRules.Configuration.RulesEngineConfigurators
{
    using System;
    using System.Reflection;
    using Models.RuntimeModel;

    public interface RuntimeConfigurator
    {
        RulesEngine RulesEngine { get; }

        T CreateNode<T>(Func<int, T> nodeFactory);
        T CreateNode<T>(Func<T> nodeFactory);

        AlphaNode<T> GetAlphaNode<T>()
            where T : class;

        void MatchPropertyNode<T, TProperty>(PropertyInfo propertyInfo, Action<PropertyNode<T, TProperty>> callback)
            where T : class;

        void MatchEqualNode<T, TProperty>(PropertyInfo propertyInfo, Action<EqualNode<T, TProperty>> callback)
            where T : class;

        void MatchCompareNode<T, TProperty>(PropertyInfo propertyInfo,
                                            CompareNode<T, TProperty> compareNode,
                                            Action<CompareNode<T, TProperty>> callback)
            where T : class;

        void MatchAlphaNode<T>(Node<T> start, Action<AlphaNode<T>> callback)
            where T : class;

        void MatchJoinNode<T>(MemoryNode<T> left, Action<JoinNode<T>> callback)
            where T : class;

        void MatchJoinNode<T>(MemoryNode<T> left, MemoryNode<T> right, Action<JoinNode<T>> action)
            where T : class;

        void MatchLeftJoinNode<T, TDiscard>(MemoryNode<Token<T, TDiscard>> start,
                                            Action<LeftJoinNode<T, TDiscard>> callback)
            where T : class;
    }
}