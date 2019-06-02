using System;
using System.IO;
using System.Linq;
using System.Threading;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using Common;

namespace Cluster
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("config.hocon"));

            using (var system = ActorSystem.Create("ClusterSystem", config))
            {
                //var router = system.ActorOf(Props.Create(() => new SetResceenDataActor()).WithRouter(FromConfig.Instance), "getDataRoute");
                //var getRescreenDataActor = system.ActorOf(Props.Create<GetRescreenDataActor>(), "GetRescreenDataActor");
                //getRescreenDataActor.Tell("Start", router);

                if (args != null && args.Any() && args[0] == "sender")
                {
                    int sendTimes = 0;
                    var sender = system.ActorOf(Props.Create<Sender>(), "sender");
                }
                else
                {
                    var destination = system.ActorOf(Props.Create<Destination>(), "destination");
                    Console.ReadKey();
                }

                Console.ReadKey();
            }
        }
    }
}
