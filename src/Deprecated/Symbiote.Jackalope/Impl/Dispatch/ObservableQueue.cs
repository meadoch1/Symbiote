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
using System.Collections.Concurrent;
using System.Threading;
using Symbiote.Core.Extensions;
using Symbiote.Jackalope.Impl.Channel;
using Symbiote.Jackalope.Impl.Serialization;
using System.Linq;

namespace Symbiote.Jackalope.Impl.Dispatch
{
    public class ObservableQueue : IQueueObserver
    {
        protected ConcurrentBag<IObserver<Envelope>> observers { get; set; }
        protected IMessageSerializer messageSerializer { get; set; }
        protected IChannelProxyFactory proxyFactory { get; set; }
        protected ConcurrentQueue<IAsyncResult> dispatchers { get; set; }

        public string QueueName { get; protected set; }

        public bool Running { get; protected set; }

        public event ObserverShutdown OnShutdown;

        public virtual void Dispatch(Envelope message)
        {
            observers.ForEach(x =>
                                  {
                                      x.OnNext(message);
                                  });
        }

        public virtual void SendCompletion()
        {
            observers.ForEach(x => x.OnCompleted());
        }

        public void Start(string queueName)
        {
            QueueName = queueName;
            Running = true;
            Action dequeue = this.Dequeue;
            dequeue.BeginInvoke(null, null);
        }

        protected void Dequeue()
        {
            Action<Envelope> dispatch = this.Dispatch;
            while (Running)
            {
                var proxy = proxyFactory.GetProxyForQueue(QueueName);
                bool valid;
                do
                {
                    valid = false;
                    var envelope = proxy.Dequeue();
                    if (envelope != null)
                    {
                        dispatchers.Enqueue(dispatch.BeginInvoke(envelope, DisposeProxy, proxy));
                        valid = true;
                    }
                    else
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(50));
                    }
                } while (!valid && Running);
            }
        }

        protected virtual void DisposeProxy(IAsyncResult ar)
        {
            var proxy = ar.AsyncState as IChannelProxy;
            proxy.Dispose();
        }

        public void Stop()
        {
            Running = false;
            SendCompletion();
        }

        public virtual IDisposable Subscribe(IObserver<Envelope> observer)
        {
            var disposable = this as IDisposable;
            observers.Add(observer);
            return disposable;
        }

        public ObservableQueue(IMessageSerializer messageSerializer, IChannelProxyFactory proxyFactory)
        {
            this.messageSerializer = messageSerializer;
            this.proxyFactory = proxyFactory;
            observers = new ConcurrentBag<IObserver<Envelope>>();
            dispatchers = new ConcurrentQueue<IAsyncResult>();
            var observable = dispatchers.ToObservable();

            observable.DoWhile(() => dispatchers.Count > 3).Subscribe(x => x.AsyncWaitHandle.WaitOne());
        }

        public void Dispose()
        {
            while (observers.Count > 0)
            {
                IObserver<Envelope> o;
                observers.TryTake(out o);
            }
        }
    }
}