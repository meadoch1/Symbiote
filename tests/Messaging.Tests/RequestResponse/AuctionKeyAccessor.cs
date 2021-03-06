﻿using Symbiote.Actor;
using Symbiote.Messaging;

namespace Messaging.Tests.RequestResponse
{
    public class AuctionKeyAccessor
        : IKeyAccessor<Auction>
    {
        public string GetId( Auction actor )
        {
            return actor.Item;
        }

        public void SetId<TKey>( Auction actor, TKey id )
        {
            actor.Item = id.ToString();
        }
    }
}