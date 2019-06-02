using System.Threading;
using Akka.Actor;
using Akka.Event;

namespace Common
{
   public class SetResceenDataActor : ReceiveActor
    {
       public IActorRef Router { get; set; }

        public SetResceenDataActor()
        {
            Receive<string>(u =>
            {
                Context.GetLogger().Warning("Get some data from main seed node: " + u);
                Thread.Sleep(2000); // simulate time costing operator;
                Sender.Tell("AAA");
            });
        }
    }
}
