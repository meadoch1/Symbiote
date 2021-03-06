﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using RabbitDemo.Messages;
using Symbiote.Core;
using Symbiote.Core.Extensions;
using Symbiote.Daemon;
using Symbiote.Daemon.Host;
using Symbiote.Log4Net;
using Symbiote.Messaging;
using Symbiote.Messaging.Impl.Actors;
using Symbiote.Rabbit;
using Symbiote.StructureMap;

namespace RabbitDemo.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Assimilate
                .Core<StructureMapAdapter>()
                .Messaging()
                .Rabbit(x => x.AddBroker(r => r.Defaults()))
                .AddConsoleLogger<Subscriber>(x => x.Info().MessageLayout(m => m.TimeStamp().Message().Newline()))
                .AddConsoleLogger<IHost>(x => x.Info().MessageLayout(m => m.TimeStamp().Message().Newline()))
                .Daemon(x => x.Name("subscriber").Arguments(args))
                .RunDaemon();
        }
    }

    public class Subscriber
        : IDaemon
    {
        public IBus Bus { get; set; }
        public int MessageCount { get; set; }
        public int ActorCount { get; set; }

        public void Start()
        {
            try
            {
                "Starting Subscriber".ToInfo<Subscriber>();
                "Configuring Rabbit...".ToInfo<Subscriber>();
                Bus.AddRabbitChannel(x => x.Fanout("test").AutoDelete().PersistentDelivery());
                Bus.AddRabbitQueue( x => x.ExchangeName("test").QueueName("test").AutoDelete().StartSubscription());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Stop()
        {

        }

        public Subscriber(IBus bus)
        {
            Bus = bus;
        }
    }

    public class ForActorMessage
        : RabbitActorHandler<Actor, Message>
    {
        public int Total { get; set; }
        public double Cumulative { get; set; }
        public double Average { get; set; }
        public override void Handle(Actor actor, RabbitEnvelope<Message> envelope)
        {
            envelope.Acknowledge();
            //actor.AddMessageId(envelope.Message.MessageId);
            if(++Total%1000==0)
            {
                Cumulative += DateTime.UtcNow.Subtract( envelope.TimeStamp ).TotalMilliseconds;
                "Avg Latency: {0}"
                    .ToInfo<Subscriber>(Cumulative / Total);
            }
            
        }
    }

    public class Actor
    {
        public string Name { get; set; }
        public List<int> Ids { get; set; }

        public void AddMessageId(int Id)
        {
            Ids.Add(Id);
            //if(Ids.Count % 100 == 0)
            //    "Actor {0} has {1} messages!"
            //    .ToInfo<Subscriber>(Name, Ids.Count);
        }

        public Actor(string name)
        {
            Ids = new List<int>();
            Name = name;

            "New Actor, {0}, created!"
                .ToInfo<Subscriber>(name);
        }
    }

    public class ActorKeyAccessor
        : IKeyAccessor<Actor>
    {
        public string GetId( Actor actor )
        {
            return actor.Name;
        }

        public void SetId<TKey>( Actor actor, TKey id )
        {
            actor.Name = id.ToString();
        }
    }

    public class ActorFactory
        : IActorFactory<Actor>
    {
        public Actor CreateInstance<TKey>(TKey id)
        {
            return new Actor(id.ToString());
        }
    }
}
