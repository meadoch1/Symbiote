using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Impl;
using Symbiote.Jackalope.Impl;

namespace Symbiote.Jackalope
{
    public interface IBus : IDisposable
    {
        void AddEndPoint(Action<IEndPoint> endpointConfiguration);
        IBus AddProcessor<TMessage>(Func<TMessage, IMessageDelivery, object> processor)
            where TMessage : class;
        void BindQueue(string queueName, string exchangeName, params string[] routingKeys);
        bool ClearQueue(string queueName);
        void DestroyQueue(string queueName);
        void DestroyExchange(string exchangeName);
        Envelope GetNextMessage(string queueName);
        Envelope GetNextMessage(string queueName, int miliseconds);
        object ProcessNextMessage(string queueName);
        object ProcessNextMessage(string queueName, int miliseconds);
        void Subscribe(string queueName, AsyncCallback onSubscriptionFailed);
        void Subscribe(string queueName, int brokers, AsyncCallback onSubscriptionFailed);
        void Send<T>(string exchangeName, T body)
            where T : class;
        void Send<T>(string exchangeName, T body, string routingKey)
            where T : class;
        void Unsubscribe(string queueName);
    }
}