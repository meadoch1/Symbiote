﻿using Machine.Specifications;
using Symbiote.Actor.Impl.Memento;

namespace Actor.Tests.Agent
{
    public class with_cached_actor 
        : with_agent_setup
    {
        private Establish context = () =>
        {
            MockActorCache.Setup(x => x.Get<string>(Moq.It.IsAny<string>())).Returns(new PassthroughMemento<DummyActor>() { Actor = new DummyActor() });
            MockActorStore.Setup(x => x.Get<string>(Moq.It.IsAny<string>())).Returns(new PassthroughMemento<DummyActor>());
        };
    }
}