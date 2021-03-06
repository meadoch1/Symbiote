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
using Symbiote.Core;
using Symbiote.Lucene.Config;
using Symbiote.Lucene.Impl;

namespace Symbiote.Lucene
{
    public static class LuceneAssimilation
    {
        public static IAssimilate Lucene(this IAssimilate assimilate)
        {
            var configurator = new LuceneConfigurator();
            var configuration = configurator.GetConfiguration();

            return Configure(assimilate, configuration);
        }

        public static IAssimilate Lucene(this IAssimilate assimilate, Action<LuceneConfigurator> config)
        {
            var configurator = new LuceneConfigurator();
            config(configurator);
            var configuration = configurator.GetConfiguration();

            return Configure(assimilate, configuration);
        }

        private static IAssimilate Configure(IAssimilate assimilate, ILuceneConfiguration configuration)
        {
            assimilate
                .Dependencies(x =>
                                  {
                                      x.For<ILuceneConfiguration>().Use(configuration).AsSingleton();
                                      x.For<ILuceneServiceFactory>().Use<LuceneServiceFactory>().AsSingleton();
                                      x.For<IDocumentQueue>().Use<DocumentQueue>().AsSingleton();
                                      x.For<BaseIndexingObserver>().Use<LuceneIndexingObserver>();
                                      x.For<BaseSearchProvider>().Use<LuceneSearchProvider>();
                                  });

            return assimilate;
        }
    }
}
