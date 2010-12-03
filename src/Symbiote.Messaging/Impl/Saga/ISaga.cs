﻿/* 
Copyright 2008-2010 Alex Robson

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;

namespace Symbiote.Messaging.Impl.Saga
{
    public interface ISaga
    {
        Type ActorType { get; }
        List<Type> Handles { get; }
    }

    public interface ISaga<TActor> :
        ISaga
        where TActor : class
    {
        Action<StateMachine<TActor>> Setup();
        bool Process<TMessage>( TActor actor, IEnvelope<TMessage> envelope );
    }
}