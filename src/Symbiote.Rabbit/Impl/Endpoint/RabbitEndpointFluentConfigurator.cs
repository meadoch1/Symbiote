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
using Symbiote.Messaging.Impl.Channels;
using Symbiote.Messaging.Impl.Serialization;
using Symbiote.Rabbit.Impl.Channels;

namespace Symbiote.Rabbit.Impl.Endpoint
{
    public class RabbitEndpointFluentConfigurator<TMessage>
    {
        public RabbitEndpoint Endpoint { get; set; }
        public RabbitChannelDefinition<TMessage> ChannelDefinition { get; protected set;}
        public bool Subscribe { get; set; }

        public RabbitEndpointFluentConfigurator<TMessage> Broker(string broker)
        {
            Endpoint.Broker = broker;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> CreateResponseChannel()
        {
            Endpoint.NeedsResponseChannel = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> QueueName(string queueName)
        {
            Endpoint.QueueName = queueName;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> RoutingKeys(params string[] routingKeys)
        {
            Endpoint.RoutingKeys = new List<string>(routingKeys);
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> Direct(string exchangeName)
        {
            Endpoint.ExchangeName = exchangeName;
            ChannelDefinition.Name = exchangeName;
            Endpoint.ExchangeType = ExchangeType.direct;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> Fanout(string exchangeName)
        {
            Endpoint.ExchangeName = exchangeName;
            ChannelDefinition.Name = exchangeName;
            Endpoint.ExchangeType = ExchangeType.fanout;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> Topic(string exchangeName)
        {
            Endpoint.ExchangeName = exchangeName;
            ChannelDefinition.Name = exchangeName;
            Endpoint.ExchangeType = ExchangeType.topic;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> Durable()
        {
            Endpoint.Durable = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> Exclusive()
        {
            Endpoint.Exclusive = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> Passive()
        {
            Endpoint.Passive = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> AutoDelete()
        {
            Endpoint.AutoDelete = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> Immediate()
        {
            Endpoint.ImmediateDelivery = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> Internal()
        {
            Endpoint.Internal = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> LoadBalanced()
        {
            Endpoint.LoadBalance = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> Mandatory()
        {
            Endpoint.MandatoryDelivery = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> NoWait()
        {
            Endpoint.NoWait = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> NoAck()
        {
            Endpoint.NoAck = true;
            return this;
        }
        
        public RabbitEndpointFluentConfigurator<TMessage> PersistentDelivery()
        {
            Endpoint.PersistentDelivery = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> StartSubscription()
        {
            Subscribe = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> UseTransactions()
        {
            Endpoint.UseTransactions = true;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> Named(string name)
        {
            ChannelDefinition.Name = name;
            Endpoint.ExchangeName = name;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> CorrelateBy(string correlationId)
        {
            ChannelDefinition.CorrelationMethod = x => correlationId;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> CorrelateBy(Func<TMessage, string> messageProperty)
        {
            ChannelDefinition.CorrelationMethod = messageProperty;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> RouteBy(string routingKey)
        {
            ChannelDefinition.RoutingMethod = x => routingKey;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> RouteBy(Func<TMessage, string> messageProperty)
        {
            ChannelDefinition.RoutingMethod = messageProperty;
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> SerializeWithProtobuf()
        {
            ChannelDefinition.MessageSerializerType = typeof(ProtobufMessageSerializer);
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> SerializeWithBinary()
        {
            ChannelDefinition.MessageSerializerType = typeof(NetBinarySerializer);
            return this;
        }

        public RabbitEndpointFluentConfigurator<TMessage> SerializeWithJson()
        {
            ChannelDefinition.MessageSerializerType = typeof(JsonMessageSerializer);
            return this;
        }

        public RabbitEndpointFluentConfigurator()
        {
            Endpoint = new RabbitEndpoint();
            ChannelDefinition = new RabbitChannelDefinition<TMessage>();
        }
    }
}