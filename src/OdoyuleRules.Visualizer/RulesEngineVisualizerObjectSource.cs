﻿// Copyright 2011 Chris Patterson
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
namespace OdoyuleRules.Visualizer
{
    using System;
    using System.IO;
    using Graphing;
    using Microsoft.VisualStudio.DebuggerVisualizers;

    public class RulesEngineVisualizerObjectSource :
        VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            if (target == null)
                return;

            Type machineType = target.GetType();
            Type instanceType = GetInstanceType(machineType);
            if (instanceType == null)
                return;

            object graph = GetType()
                .GetMethod("CreateStateMachineGraph")
                .MakeGenericMethod(machineType, instanceType)
                .Invoke(this, new[] {target});

            base.GetData(graph, outgoingData);
        }

        Type GetInstanceType(Type machineType)
        {
            while (machineType != null && machineType != typeof (object))
            {
                if (machineType.IsGenericType && machineType.GetGenericTypeDefinition() == typeof (OdoyuleRulesEngine))
                {
                    Type instanceType = machineType.GetGenericArguments()[0];
                    return instanceType;
                }

                machineType = machineType.BaseType;
            }

            return null;
        }

        RulesEngineGraph CreateRulesEngineGraph(OdoyuleRulesEngine machine)
        {
            var visitor = new GraphRulesEngineVisitor();
            machine.Accept(visitor);

            return visitor.Graph;
        }
    }
}