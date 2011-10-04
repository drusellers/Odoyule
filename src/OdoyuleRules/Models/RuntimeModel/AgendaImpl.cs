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
    using System.Collections.Generic;

    class AgendaImpl :
        Agenda
    {
        const int InitialCapacity = 10;
        IList<Tuple<int, Action>> _operations;
        bool _stopped;

        public AgendaImpl()
        {
            _operations = new List<Tuple<int, Action>>(InitialCapacity);
        }

        public void Schedule(Action operation, int priority)
        {
            if (_stopped)
                return;

            Tuple<int, Action> item = Tuple.Create(priority, operation);

            int count = _operations.Count;
            if(count > 0 && _operations[count-1].Item1 == priority)
            {
                _operations.Add(item);
                return;
            }

            for (int i = 0; i < count; i++)
            {
                if (priority > _operations[i].Item1)
                {
                    _operations.Insert(i, item);
                    return;
                }
            }

            _operations.Add(item);
        }

        public void Stop()
        {
            _stopped = true;
        }

        public bool Run()
        {
            if (_stopped)
                return false;

            int count = _operations.Count;
            if (count == 0)
                return false;

            IList<Tuple<int, Action>> operations = _operations;
            _operations = new List<Tuple<int, Action>>(InitialCapacity);

            for (int i = 0; i < count; i++)
            {
                if (_stopped)
                    return false;

                operations[i].Item2();
            }

            return _operations.Count > 0;
        }
    }
}