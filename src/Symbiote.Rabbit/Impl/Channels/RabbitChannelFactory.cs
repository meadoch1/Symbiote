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
using Symbiote.Messaging.Impl.Channels;
using Symbiote.Messaging.Impl.Dispatch;
using Symbiote.Messaging.Impl.Serialization;

namespace Symbiote.Rabbit.Impl.Channels
{
    public class RabbitChannelFactory
        : IChannelFactory
    {
        public IChannelProxyFactory ProxyFactory { get; set; }
        public IMessageSerializer Serializer { get; set; }
        public IDispatcher MessageDispatcher { get; set; }

        public IChannel CreateChannel(IChannelDefinition definition)
        {
            var rabbitDef = definition as ChannelDefinition;
            var proxy = ProxyFactory.GetProxyForExchange(rabbitDef);
            var serializer = Activator.CreateInstance( definition.SerializerType ) as IMessageSerializer;
            var channel = new RabbitChannel(proxy, serializer, rabbitDef, MessageDispatcher) as IChannel;
            return channel;
        }

        public RabbitChannelFactory(IChannelProxyFactory proxyFactory, IDispatcher dispatcher)
        {
            ProxyFactory = proxyFactory;
            MessageDispatcher = dispatcher;
        }
    }
}
