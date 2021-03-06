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
using System.Net.Sockets;
using System.Text;
using System.Timers;
using Symbiote.Core;
using Symbiote.Core.Extensions;

namespace Symbiote.WebSocket.Impl
{
    public class WebSocket : IWebSocket
    {
        protected IList<IObserver<Tuple<string, string>>> _observers = new List<IObserver<Tuple<string, string>>>();
        protected Socket _socket;
        protected int _bufferSize;
        protected DelimitedBuilder _builder = new DelimitedBuilder("");
        protected bool _receiving = false;
        protected bool _listening = false;

        public string ClientId { get; set; }

        public Socket NetworkSocket
        {
            get { return _socket; }
            protected set { _socket = value; }
        }

        public Action<string> OnDisconnect { get; set; }

        public virtual IDisposable Subscribe(IObserver<Tuple<string, string>> observer)
        {
            _observers.Add(observer);
            return observer as IDisposable;
        }

        public virtual void Close()
        {
            if(_socket!=null && _socket.Connected)
                _socket.Close();
            
            if(_observers != null && _observers.Count > 0)
            {
                _observers.ForEach(x => x.OnCompleted());
                _observers.Clear();
                _observers = null;
            }
            if(OnDisconnect != null)
                OnDisconnect(ClientId);
        }

        public virtual void Dispose()
        {
            Close();   
        }

        public virtual void Send(string message)
        {
            var content = new List<byte>(Encoding.UTF8.GetBytes(message));
            content.Insert(0, (byte)Signal.Start);
            content.Add((byte)Signal.End);
            _socket.Send(content.ToArray());
        }

        protected void Receive(IAsyncResult result)
        {
            _listening = false;
            var received = _socket.EndReceive(result);

            if(received > 0)
            {
                var buffer = result.AsyncState as byte[];

                if(buffer[0] == (byte)Signal.Start)
                {
                    _receiving = true;
                    _builder.Clear();
                }

                var ends = buffer.Contains((byte) Signal.End);
                _builder.Append(UTF8Encoding.UTF8.GetString(
                        buffer
                            .SkipWhile(x => x == (byte)Signal.Start)
                            .TakeWhile(x => x != (byte)Signal.End).ToArray()));
                
                if(ends)
                {
                    _receiving = false;
                    var message = Tuple.Create(ClientId, _builder.ToString());
                    _observers.AsParallel().ForEach(x => x.OnNext(message));
                }
            }

            Listen();
        }

        protected virtual void Listen()
        {
            _listening = true;
            var buffer = new byte[_bufferSize];
            try
            {
                _socket.BeginReceive(buffer, 0, _bufferSize, SocketFlags.None, Receive, buffer);
            }
            catch (Exception e)
            {
                "An exception occurred while listening to the socket \r\n\t {0}"
                    .ToError<ISocketServer>(e);

                Close();
            }
        }

        void OnTimer(object sender, ElapsedEventArgs e)
        {
            _socket.Poll(50, SelectMode.SelectError);
        }

        public WebSocket(string clientId, Socket socket, int bufferSize)
        {
            ClientId = clientId;
            _socket = socket;
            _bufferSize = bufferSize;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            Listen();
        }
    }
}