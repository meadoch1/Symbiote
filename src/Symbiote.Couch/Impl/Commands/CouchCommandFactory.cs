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
using Symbiote.Couch.Config;

namespace Symbiote.Couch.Impl.Commands
{
    public class CouchCommandFactory 
    {
        protected ICouchConfiguration configuration { get; set; }

        protected TCommand CreateCommand<TCommand>()
        {
            return Assimilate.GetInstanceOf<TCommand>();
        }

        public DeleteAttachmentCommand CreateDeleteAttachmentCommand()
        {
            return CreateCommand<DeleteAttachmentCommand>();
        }

        public GetAttachmentCommand CreateGetAttachmentCommand()
        {
            return CreateCommand<GetAttachmentCommand>();
        }

        public SaveAttachmentCommand CreateSaveAttachmentCommand()
        {
            return CreateCommand<SaveAttachmentCommand>();
        }

        public DeleteDocumentCommand CreateDeleteCommand()
        {
            return CreateCommand<DeleteDocumentCommand>();
        }

        public GetDocumentCommand CreateGetDocumentCommand()
        {
            return CreateCommand<GetDocumentCommand>();
        }

        public GetAllDocumentsCommand CreateGetAllDocumentsCommand()
        {
            return CreateCommand<GetAllDocumentsCommand>();
        }

        public GetDocumentsPagedCommand CreateGetDocumentsPagedCommand()
        {
            return CreateCommand<GetDocumentsPagedCommand>();
        }

        public GetDocumentsByIdsCommand CreateGetDocumentsByIdsCommand()
        {
            return CreateCommand<GetDocumentsByIdsCommand>();
        }

        public GetDocumentsInRangeCommand CreateGetDocumentsInRangeCommand()
        {
            return CreateCommand<GetDocumentsInRangeCommand>();
        }

        public GetFromViewCommand CreateGetFromViewCommand()
        {
            return CreateCommand<GetFromViewCommand>();
        }

        public CouchQueryCommand CreateQueryCommand()
        {
            return CreateCommand<CouchQueryCommand>();
        }

        public ISaveDocument CreateSaveDocumentCommand()
        {
            return configuration.BreakDownDocumentGraphs ? (ISaveDocument)
                        CreateSaveDocumentGraphCommand()
                       : CreateSaveDocumentOnlyCommand();
        }

        public ISaveDocuments CreateSaveDocumentsCommand()
        {
            return configuration.BreakDownDocumentGraphs
                       ? (ISaveDocuments)
                         CreateSaveDocumentOnlyListCommand()
                       : CreateSaveDocumentGraphListCommand();
        }

        public SaveDocumentCommand CreateSaveDocumentOnlyCommand()
        {
            return CreateCommand<SaveDocumentCommand>();
        }

        public SaveDocumentGraphCommand CreateSaveDocumentGraphCommand()
        {
            return CreateCommand<SaveDocumentGraphCommand>();
        }

        public SaveDocumentListCommand CreateSaveDocumentOnlyListCommand()
        {
            return CreateCommand<SaveDocumentListCommand>();
        }

        public SaveDocumentGraphListCommand CreateSaveDocumentGraphListCommand()
        {
            return CreateCommand<SaveDocumentGraphListCommand>();
        }

        public ServerCommand CreateServerCommand()
        {
            return CreateCommand<ServerCommand>();
        }

        public ChangeStreamCommand CreateStreamCommand()
        {
            return CreateCommand<ChangeStreamCommand>();
        }

        public CouchCommandFactory(ICouchConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}