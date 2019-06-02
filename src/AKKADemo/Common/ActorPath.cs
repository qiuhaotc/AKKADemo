using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace Common
{
    public class ActorPathConstans
    {
        public static readonly ActorMetaData SetResceenDataActor = new ActorMetaData("SetResceenDataActor", "akka://ClusterSystem/user/GetResceenData");
    }

    /// <summary>
    /// Meta-data class
    /// </summary>
    public class ActorMetaData
    {
        public ActorMetaData(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; private set; }

        public string Path { get; private set; }
    }
}
