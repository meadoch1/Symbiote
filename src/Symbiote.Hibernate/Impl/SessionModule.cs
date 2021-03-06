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
using NHibernate;

namespace Symbiote.Hibernate.Impl
{
    public class SessionModule : ISessionModule
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISessionManager _sessionManager;

        public SessionModule(ISessionFactory factory, ISessionManager manager)
        {
            _sessionFactory = factory;
            _sessionManager = manager;
        }

        public virtual void BeginSession()
        {
            var session = _sessionFactory.OpenSession();
            session.BeginTransaction();
            _sessionManager.CurrentSession = session;
        }

        public virtual void EndSession()
        {
            var session = _sessionManager.CurrentSession;
            if (session != null && session.IsOpen)
            {
                if (session.Transaction.IsActive)
                {
                    session.Transaction.Commit();
                }
                session.Close();
            }
            if (session != null)
            {
                session.Dispose();
            }
        }

        public virtual void Dispose()
        {
            _sessionFactory.Dispose();
        }
    }
}
