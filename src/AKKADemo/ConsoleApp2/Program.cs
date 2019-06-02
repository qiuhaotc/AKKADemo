using System;
using System.IO;
using System.Linq;
using System.Threading;
using Akka.Actor;
using Akka.Cluster.Routing;
using Akka.Configuration;
using Akka.Routing;
using Common;

namespace SeedNode
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("config.hocon"));

            using (var system = ActorSystem.Create("ClusterSystem", config))
            {
                var listerner = system.ActorOf(Props.Create<SimpleClusterListener>(), "Listerner");
                var des = system.ActorOf(Props.Create<Destination>(), "destination");
                var setActor = system.ActorOf(Props.Create(() => new SetResceenDataActor()), ActorPathConstans.SetResceenDataActor.Name);
                var router = system.ActorOf(Props.Create(() => new GetRescreenDataActor(setActor)).WithRouter(FromConfig.Instance), "myClusterPoolRouter");

                while (true)
                {
                    if(router.Ask<Routees>(new GetRoutees()).Result.Members.Count() > 0)
                    {
                        router.Tell("Start", setActor);
                        break;
                    }
                    else
                    {
                        Thread.Sleep(2000);
                    }
                }

                Console.ReadKey();
            }
        }
    }
}
