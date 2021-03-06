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
using System.Text;

namespace Symbiote.Core.Log.Impl
{
    public class ProxyLogger<T> : ILogger<T>
    {
        public void Log(LogLevel level, object message)
        {
            LogManager.Log<T>(level, message);
        }

        public void Log(LogLevel level, object message, Exception exception)
        {
            LogManager.Log<T>(level, message, exception);
        }

        public void Log(LogLevel level, string format, params object[] args)
        {
            LogManager.Log<T>(level, format, args);
        }

        public void Log(LogLevel level, IFormatProvider provider, string format, params object[] args)
        {
            LogManager.Log<T>(level, provider, format, args);
        }
    }
}
