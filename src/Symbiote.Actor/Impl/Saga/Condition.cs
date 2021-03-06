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
using System.Linq;

namespace Symbiote.Actor.Impl.Saga
{
    public class Condition<TActor>
        : ICondition<TActor>
    {
        protected Predicate<TActor> Guard { get; set; }
        public Dictionary<Type, IConditionalTransition<TActor>> Transitions { get; set; }
        public List<Type> Handles { get { return Transitions.Keys.ToList(); } }

        public ICondition<TActor> On<TMessage>( Action<TActor, TMessage> processMessage)
        {
            var onMessage = new ConditionalTransition<TActor, TMessage>()
            {
                Process = processMessage,
                Guard = this.Guard
            };
            Transitions.Add( typeof(TMessage), onMessage );
            return this;
        }

        public ICondition<TActor> On<TMessage>( Action<TActor> transition)
        {
            var onMessage = new ConditionalTransition<TActor, TMessage>()
            {
                Transition = transition,
                Guard = this.Guard
            };
            Transitions.Add(typeof(TMessage), onMessage);
            return this;
        }

        public ICondition<TActor> On<TMessage>( Action<TActor, TMessage> processMessage, Action<TActor> transition)
        {
            var onMessage = new ConditionalTransition<TActor, TMessage>()
            {
                Transition = transition,
                Process = processMessage,
                Guard = this.Guard
            };
            Transitions.Add(typeof(TMessage), onMessage);
            return this;
        }

        public Condition()
            : this(x => true)
        {
        }

        public Condition( Predicate<TActor> guard )
        {
            Guard = guard;
            Transitions = new Dictionary<Type, IConditionalTransition<TActor>>();
        }
    }
}