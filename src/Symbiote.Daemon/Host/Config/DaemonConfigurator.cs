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
using System.Linq;
using Symbiote.Core.DI;
using Symbiote.Core.Extensions;

namespace Symbiote.Daemon
{
    public class DaemonConfigurator
    {
        public DaemonConfiguration Configuration { get; set; }

        public DaemonConfigurator Name(string name)
        {
            Configuration.Name = name;
            Configuration.DisplayName = Configuration.DisplayName ?? name;
            Configuration.Description = Configuration.Description ?? name;
            return this;
        }

        public DaemonConfigurator DisplayName(string displayName)
        {
            Configuration.DisplayName = displayName;
            return this;
        }

        public DaemonConfigurator Description(string description)
        {
            Configuration.Description = description;
            return this;
        }

        public DaemonConfigurator Arguments(string[] arguments)
        {
            Configuration.Arguments = arguments;
            return this;
        }

        public DaemonConfigurator AsAccount(string user, string password)
        {
            Configuration.Principal = user;
            Configuration.Password = password;
            Configuration.AsLocal = false;
            return this;
        }

        public DaemonConfigurator TimeoutIn(int seconds)
        {
            Configuration.StartupTimeout = TimeSpan.FromSeconds(seconds);
            return this;
        }

        public DaemonConfigurator()
        {
            Configuration = new DaemonConfiguration();
        }
    }
}