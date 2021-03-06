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
using System.Collections;
using RabbitMQ.Client;
using Symbiote.Core.Extensions;
using Symbiote.Jackalope.Config;
using Symbiote.Jackalope.Impl.Server;

namespace Symbiote.Jackalope.Impl.Endpoint
{
    public class EndpointManager : IEndpointManager
    {
        protected IConnectionManager _connectionManager;
        protected IEndpointIndex _endpointIndex;

        public void AddEndpoint(IEndPoint endpoint)
        {
            ConfigureEndpoint(endpoint);
            _endpointIndex.AddEndpoint(endpoint);
        }

        public void ConfigureEndpoint(IEndPoint endpoint)
        {
            if (!endpoint.Initialized)
            {
                var configuration = endpoint.EndpointConfiguration;

                var connections = _connectionManager.GetBrokerConnections(endpoint.EndpointConfiguration.Broker);
                foreach(var connection in connections)
                {
                    using(var channel = connection.CreateModel())
                    {
                    
                        if (!string.IsNullOrEmpty(configuration.ExchangeName))
                            BuildExchange(channel, configuration);

                        if (!string.IsNullOrEmpty(configuration.QueueName))
                            BuildQueue(channel, configuration);

                        if (!string.IsNullOrEmpty(configuration.ExchangeName) && !string.IsNullOrEmpty(configuration.QueueName))
                            BindQueue(channel, configuration.ExchangeName, configuration.QueueName, configuration.RoutingKeys.ToArray());

                        endpoint.Initialized = true;
                    }
                }
            }
        }

        public void BindQueue(string exchangeName, string queueName, params string[] routingKeys)
        {
            var endpoint = _endpointIndex.GetEndpointByExchange(exchangeName);
            var configuration = endpoint.EndpointConfiguration;

            var connections = _connectionManager.GetBrokerConnections(endpoint.EndpointConfiguration.Broker);
            foreach (var connection in connections)
            {
                using (var channel = connection.CreateModel())
                {
                    BindQueue(channel, exchangeName, queueName, routingKeys);
                }
            }
        }

        public void BindQueue(IModel channel, string exchangeName, string queueName, params string[] routingKeys)
        {
            if(routingKeys.Length == 0)
                routingKeys = new [] {""};
            try
            {
                routingKeys
                    .ForEach(x => Bind(channel, exchangeName, queueName, x, false, null));
            }
            catch (Exception x)
            {
                
                throw;
            }
        }

        protected void Bind(IModel channel, string exchangeName, string queueName, string routingKey, bool noWait, IDictionary args)
        {
            channel.QueueBind(queueName, exchangeName, routingKey, noWait, args);
        }

        public void BuildExchange(IModel channel, IAmqpEndpointConfiguration endpointConfiguration)
        {
            channel.ExchangeDeclare(
                endpointConfiguration.ExchangeName,
                endpointConfiguration.ExchangeTypeName,
                endpointConfiguration.Passive,
                endpointConfiguration.Durable,
                endpointConfiguration.AutoDelete,
                endpointConfiguration.Internal,
                endpointConfiguration.NoWait,
                endpointConfiguration.Arguments);
        }

        public void BuildQueue(IModel channel, IAmqpEndpointConfiguration endpointConfiguration)
        {
            channel.QueueDeclare(
                endpointConfiguration.QueueName,
                endpointConfiguration.Passive,
                endpointConfiguration.Durable,
                endpointConfiguration.Exclusive,
                endpointConfiguration.AutoDelete,
                endpointConfiguration.NoWait,
                endpointConfiguration.Arguments);
        }

        public IEndPoint GetEndpointByExchange(string exchangeName)
        {
            var endpoint = _endpointIndex.GetEndpointByExchange(exchangeName);
            if (endpoint == null)
                throw new JackalopeConfigurationException(
                    "There was no endpoint configured for exchange {0}. Please provide configuration using the AddEndPoint method on the IBus interface.".AsFormat(exchangeName));
            ConfigureEndpoint(endpoint);
            return endpoint;
        }

        public IEndPoint GetEndpointByQueue(string queueName)
        {
            var endpoint = _endpointIndex.GetEndpointByQueue(queueName);
            if (endpoint == null)
                throw new JackalopeConfigurationException(
                    "There was no endpoint configured for exchange {0}. Please provide configuration using the AddEndPoint method on the IBus interface.".AsFormat(queueName));
            ConfigureEndpoint(endpoint);
            return endpoint;
        }

        public EndpointManager(IConnectionManager connectionManager, IEndpointIndex endpointIndex)
        {
            _connectionManager = connectionManager;
            _endpointIndex = endpointIndex;
        }
    }
}