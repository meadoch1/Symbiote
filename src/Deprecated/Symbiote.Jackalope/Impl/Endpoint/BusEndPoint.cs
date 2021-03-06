/* 
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
using Symbiote.Jackalope.Config;

namespace Symbiote.Jackalope.Impl.Endpoint
{
    public class BusEndPoint : IEndPoint, IDisposable
    {
        protected IAmqpEndpointConfiguration _configuration;

        public IAmqpEndpointConfiguration EndpointConfiguration
        {
            get { return _configuration; }
        }

        #region Fluently

        public IEndPoint Broker(string broker)
        {
            _configuration.Broker = broker;
            return this;
        }

        public IEndPoint QueueName(string queueName)
        {
            _configuration.QueueName = queueName;
            return this;
        }

        public IEndPoint RoutingKeys(params string[] routingKeys)
        {
            _configuration.RoutingKeys = new List<string>(routingKeys);
            return this;
        }

        public IEndPoint Exchange(string exchangeName, ExchangeType exchange)
        {
            _configuration.ExchangeName = exchangeName;
            _configuration.ExchangeType = exchange;
            return this;
        }

        public IEndPoint Durable()
        {
            _configuration.Durable = true;
            return this;
        }

        public IEndPoint Exclusive()
        {
            _configuration.Exclusive = true;
            return this;
        }

        public IEndPoint Passive()
        {
            _configuration.Passive = true;
            return this;
        }

        public IEndPoint AutoDelete()
        {
            _configuration.AutoDelete = true;
            return this;
        }

        public IEndPoint Immediate()
        {
            _configuration.ImmediateDelivery = true;
            return this;
        }

        public IEndPoint Internal()
        {
            _configuration.Internal = true;
            return this;
        }

        public IEndPoint LoadBalanced()
        {
            _configuration.LoadBalance = true;
            return this;
        }

        public IEndPoint Mandatory()
        {
            _configuration.MandatoryDelivery = true;
            return this;
        }

        public IEndPoint NoWait()
        {
            _configuration.NoWait = true;
            return this;
        }

        public IEndPoint NoAck()
        {
            _configuration.NoAck = true;
            return this;
        }

        public IEndPoint PersistentDelivery()
        {
            _configuration.PersistentDelivery = true;
            return this;
        }

        #endregion

        public bool Initialized { get; set; }

        public BusEndPoint()
        {
            _configuration = new AmqpEndpointConfiguration();
        }

        public static IEndPoint CreateFromAmqpEndpoint(IAmqpEndpointConfiguration endpoint)
        {
            return new BusEndPoint() {_configuration = endpoint};
        }

        public void Dispose()
        {

        }
    }
}