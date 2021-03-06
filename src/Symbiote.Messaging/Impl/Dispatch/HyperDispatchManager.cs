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
using System.Collections.Concurrent;
using Symbiote.Core;
using Symbiote.Core.Extensions;
using Symbiote.Core.Utility;

namespace Symbiote.Messaging.Impl.Dispatch
{
    public class HyperDispatchManager
        : IDispatcher
    {
        public int Count { get; set; }
        public ConcurrentDictionary<Type, IDispatchMessage> Dispatchers { get; set; }
        public ConcurrentDictionary<string, IDispatchMessage> ResponseDispatchers { get; set; }
        public VolatileRingBufferArray RingBuffer { get; set; }

        public void Send<TMessage>(IEnvelope<TMessage> envelope)
        {
            Count++;
            //SendToHandler( envelope );
            RingBuffer.Write( envelope.CorrelationId, envelope );
        }

        public void Send(IEnvelope envelope)
        {
            Count++;
            RingBuffer.Write(envelope.CorrelationId, envelope);
            //SendToHandler( envelope );
        }

        public void ExpectResponse<TResponse>(string correlationId, Action<TResponse> onResponse)
        {
            ResponseDispatchers.TryAdd(correlationId, new ResponseDispatcher<TResponse>(onResponse));
        }

        public void SendToHandler(IEnvelope envelope)
        {
            IDispatchMessage dispatcher;
            if (Dispatchers.TryGetValue(envelope.MessageType, out dispatcher))
            {
                dispatcher.Dispatch(envelope);
            }
            else // message is a response to a request message
            {
                if(ResponseDispatchers.TryRemove(envelope.CorrelationId, out dispatcher))
                {
                    dispatcher.Dispatch(envelope);
                }
            }
        }

        public void WireupDispatchers()
        {
            if (Dispatchers.Count == 0)
            {
                var dispatchers = Assimilate.GetAllInstancesOf<IDispatchMessage>();
                dispatchers
                    .ForEach(x => x.Handles.ForEach(y => Dispatchers.AddOrUpdate((Type) y, (IDispatchMessage) x, (t, m) => x)));
            }
        }

        public HyperDispatchManager()
        {
            RingBuffer = new VolatileRingBufferArray( 10000, 10000 );
            RingBuffer.AddTransform( o =>
            {
                SendToHandler( o as IEnvelope );
                return null;
            } );
            RingBuffer.Start();
            Dispatchers = new ConcurrentDictionary<Type, IDispatchMessage>();
            ResponseDispatchers = new ConcurrentDictionary<string, IDispatchMessage>();
            WireupDispatchers();
        }
    }
}