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
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace Symbiote.Lucene.Impl
{
    public abstract class BaseIndexingObserver : ILuceneIndexer
    {
        protected Document document { get; set; }
        public IDocumentQueue DocumentQueue { get; set; }

        public abstract void OnNext(Tuple<string, string> value);

        public abstract void OnError(Exception error);

        public abstract void OnCompleted();
    }
}