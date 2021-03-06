﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Symbiote.Core.Reflection;

namespace Symbiote.Jackalope.Impl.Dispatch
{
    public class Dispatcher<TMessage> : IDispatch<TMessage>
           where TMessage : class
       {
           protected List<Type> handlesMessagesOf { get; set; }
   
           public bool CanHandle(object payload)
           {
               return payload as TMessage != null;
           }
   
           public IEnumerable<Type> Handles
           {
               get
               {
                   handlesMessagesOf = handlesMessagesOf ?? GetMessageChain().ToList();
                   return handlesMessagesOf;
               }
           }
   
           private IEnumerable<Type> GetMessageChain()
           {
               yield return typeof (TMessage);
               var chain = Reflector.GetInheritenceChain(typeof (TMessage));
               if(chain != null)
               {
                   foreach (var type in chain)
                   {
                       yield return type;
                   }
               }
               yield break;
           }
   
           public void Dispatch(Envelope envelope)
           {
               try
               {
                   var handler = ServiceLocator.Current.GetInstance<IMessageHandler<TMessage>>();
                   handler.Process(envelope.Message as TMessage, envelope.MessageDelivery);
               }
               catch (Exception e)
               {
                   envelope.MessageDelivery.Reject();
                   //throw;
               }
           }
       }
}
