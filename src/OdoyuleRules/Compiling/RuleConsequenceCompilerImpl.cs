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
namespace OdoyuleRules.Compiling
{
    using System;
    using Configuration.RulesEngineConfigurators;
    using Models.RuntimeModel;
    using Models.SemanticModel;

    public class RuleConsequenceCompilerImpl :
        SemanticModelVisitorImpl,
        RuleConsequenceCompiler
    {
        readonly RuleConditionCompiler _conditionCompiler;
        readonly RuntimeConfigurator _configurator;

        public RuleConsequenceCompilerImpl(RuntimeConfigurator configurator, RuleConditionCompiler conditionCompiler)
        {
            _configurator = configurator;
            _conditionCompiler = conditionCompiler;
        }

        public override bool Visit<T>(DelegateConsequence<T> consequence, Func<SemanticModelVisitor, bool> next)
        {
            _conditionCompiler.MatchJoinNode<T>(joinNode =>
                {
                    DelegateProductionNode<T> node = _configurator.Delegate(consequence.Callback);
                    joinNode.AddActivation(node);
                });

            return base.Visit(consequence, next);
        }

        public override bool Visit<T, TFact>(AddFactConsequence<T, TFact> consequence,
                                             Func<SemanticModelVisitor, bool> next)
        {
            _conditionCompiler.MatchJoinNode<T>(joinNode =>
                {
                    AddFactProductionNode<T, TFact> node = _configurator.AddFact(consequence.FactFactory);
                    joinNode.AddActivation(node);
                });

            return base.Visit(consequence, next);
        }
    }
}