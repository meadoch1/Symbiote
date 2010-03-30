﻿using Machine.Specifications;
using Symbiote.Relax.Impl;

namespace Relax.Tests.Repository
{
    public abstract class with_create_database_command : with_document_repository
    {
        private Establish context = () =>
                                        {
                                            uri = new CouchUri("http", "localhost", 5984, "testdocument");
                                            commandMock.Setup(x => x.Put(couchUri));
                                            WireUpCommandMock(commandMock.Object);
                                        };
    }
}