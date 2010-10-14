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

using System.Collections.Generic;
using RabbitMQ.Client;
using Symbiote.Rabbit.Config;

namespace Symbiote.Rabbit.Impl.Server
{
    public class ConnectionManager : IConnectionManager
    {
        protected RabbitConfiguration Configuration { get; set; }

        protected IRabbitBroker DefaultBroker { get { return Configuration.Brokers["default"]; } }

        public IConnection GetConnection()
        {
            return DefaultBroker.GetConnection();
        }

        public IConnection GetConnection(string brokerName)
        {
            return Configuration.Brokers[brokerName].GetConnection();
        }

        public ConnectionManager(RabbitConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}