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
using Microsoft.Practices.ServiceLocation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Symbiote.Core.Extensions;
using Symbiote.Jackalope.Impl.Channel;

namespace Symbiote.Jackalope.Impl
{
    public class MessageDelivery : IMessageDelivery, IDisposable
    {
        private IChannelProxy _proxy;
        private IChannelProxyFactory _factory;
        private string _exchange = "";
        private ulong _deliveryTag;
        private string _consumerTag = "";
        private bool _isReplyToPresent;
        private IBasicProperties _properties;
        private bool _redelivered;
        private ushort _replyCode;
        private string _replyText;
        private string _key;
        private bool _responded = false;

        public string Exchange {  get { return _exchange; } }

        public string Queue { get { return _proxy.QueueName; } }

        public bool Redelivered { get { return _redelivered; } }

        public bool RespondedTo { get { return _responded; } }

        public bool IsReply { get { return _isReplyToPresent; } }

        public string RoutingKey { get { return _key; } }

        public IBasicProperties Details { get { return _properties; } }

        public void Acknowledge()
        {
            Respond(x => x.Acknowledge(_deliveryTag, false), _exchange);
        }

        public void Reject()
        {
            Respond(x => x.Reject(_deliveryTag, true), _exchange);
        }

        public void Reply<TReply>(TReply reply)
            where TReply : class
        {
            if(!_properties.IsReplyToPresent())
                return;

            Respond(x => x.Reply(_properties.ReplyToAddress, _properties, reply), _exchange);
        }

        private void Respond(Action<IChannelProxy> response, string exchange)
        {
            if (_responded)
                return;

            _responded = true;
            try
            {
                if(!_proxy.Channel.IsOpen)
                {
                    using(var proxy = _factory.GetProxyForQueue(exchange))
                    {
                        response(proxy);
                    }
                }
                else
                {
                    response(_proxy);   
                }
            }
            catch (Exception e)
            {
                "An exception occurred trying to respond to a received message: \r\n\t {0}"
                    .ToError<IBus>(e);
            }
        }

        public MessageDelivery(IChannelProxy proxy, BasicDeliverEventArgs args)
        {
            _factory = ServiceLocator.Current.GetInstance<IChannelProxyFactory>();
            _proxy = proxy;
            _exchange = args.Exchange;
            _deliveryTag = args.DeliveryTag;
            _properties = args.BasicProperties;
            _redelivered = args.Redelivered;
            _key = args.RoutingKey;
        }

        public MessageDelivery(IChannelProxy proxy, BasicGetResult result)
        {
            _factory = ServiceLocator.Current.GetInstance<IChannelProxyFactory>();
            _proxy = proxy;
            _exchange = result.Exchange;
            _deliveryTag = result.DeliveryTag;
            _properties = result.BasicProperties;
            _redelivered = result.Redelivered;
            _key = result.RoutingKey;
        }

        public MessageDelivery(IChannelProxy proxy, string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties)
        {
            _proxy = proxy;
            _exchange = exchange;
            _deliveryTag = deliveryTag;
            _redelivered = redelivered;
            _consumerTag = consumerTag;
            _key = routingKey;
            _isReplyToPresent = properties.IsReplyToPresent();
            _properties = properties;
        }

        public MessageDelivery(IChannelProxy proxy, BasicReturnEventArgs args)
        {
            _factory = ServiceLocator.Current.GetInstance<IChannelProxyFactory>();
            _proxy = proxy;
            _exchange = args.Exchange;
            _properties = args.BasicProperties;
            _replyCode = args.ReplyCode;
            _replyText = args.ReplyText;
            _key = args.RoutingKey;
        }

        public void Dispose()
        {
            _proxy.Dispose();
        }
    }
}