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
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;

namespace Symbiote.Log4Net
{
    public class ConsoleConfigurator : AppenderConfigurator<ConsoleAppender, ConsoleConfigurator>
    {
        protected override void Initialize()
        {
            _appender.ActivateOptions();
            _appender.Name = "consoleAppender";
            _appender.Target = "Console.Out";
            _appender.Threshold = Level.All;
            var layout = 
                new PatternLayout(
                    Pattern
                        .New()
                        .Message()
                        .Newline()
                        .ToString()
                    );
            layout.ActivateOptions();
            _appender.Layout = layout;
        }
    }
}