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

using Symbiote.Core;

namespace Symbiote.Messaging.Impl.Monitor
{
    public class AdapterMonitor :
        IAdapterMonitor
    {
        private IBus _bus;
        public IBus Bus
        {
            get
            {
                _bus = _bus ?? Assimilate.GetInstanceOf<IBus>();
                return _bus;
            }
        }

        public MonitorConfiguration Configuration { get; set; }

        public void MessageReceived<TMessage>( IEnvelope<TMessage> envelope )
        {
            Bus.Publish( Configuration.EventChannel, new MessageEvent() );
        }

        public AdapterMonitor( MonitorConfiguration configuration )
        {
            Configuration = configuration;
        }
    }
}