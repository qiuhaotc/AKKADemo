using System;
using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Event;

namespace Common
{
    /*
 * Create an actor and a message type that gets shared between Deployer and DeployTarget
 * in a common DLL
 */
    /// <summary>
    /// Actor that just replies the message that it received earlier
    /// </summary>
    public class EchoActor : ReceiveActor
    {
        public EchoActor()
        {
            Receive<Hello>(hello =>
            {
                Context.GetLogger().Info("[{0}]: {1}", Sender, hello.Message);
                //Sender.Tell(hello);
            });
        }
    }

    public class Hello
    {
        public Hello(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }

    public class SimpleClusterListener : UntypedActor
    {
        protected ILoggingAdapter Log = Context.GetLogger();
        protected Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);

        /// <summary>
        /// Need to subscribe to cluster changes
        /// </summary>
        protected override void PreStart()
        {
            Cluster.Subscribe(Self, ClusterEvent.InitialStateAsEvents, new[] { typeof(ClusterEvent.IMemberEvent), typeof(ClusterEvent.UnreachableMember) });
        }

        /// <summary>
        /// Re-subscribe on restart
        /// </summary>
        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
        }

        protected override void OnReceive(object message)
        {
            var up = message as ClusterEvent.MemberUp;
            if (up != null)
            {
                var mem = up;
                Log.Info("Member is Up: {0}", mem.Member);
            }
            else if (message is ClusterEvent.UnreachableMember)
            {
                var unreachable = (ClusterEvent.UnreachableMember)message;
                Log.Info("Member detected as unreachable: {0}", unreachable.Member);
            }
            else if (message is ClusterEvent.MemberRemoved)
            {
                var removed = (ClusterEvent.MemberRemoved)message;
                Log.Info("Member is Removed: {0}", removed.Member);
            }
            else if (message is ClusterEvent.IMemberEvent)
            {
                //IGNORE                
            }
            else if (message is ClusterEvent.CurrentClusterState)
            {

            }
            else
            {
                Unhandled(message);
            }
        }
    }

    public class Sender : ReceiveActor
    {
        int sendTimes = 0;

        public Sender()
        {
            // activate the extension
            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            Receive<string>(str =>
            {
                var upperCase = str.ToUpper();
                mediator.Tell(new Send(path: "/user/destination", message: upperCase, localAffinity: true));
            });
        }
    }

    public class Destination : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();

        public Destination()
        {
            // activate the extension
            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            // register to the path
            mediator.Tell(new Put(Self));

            Receive<string>(s =>
            {
                log.Info($"Got {s}");
            });
        }
    }
}
