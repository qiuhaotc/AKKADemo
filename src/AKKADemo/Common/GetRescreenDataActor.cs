using System;
using System.Threading;
using Akka.Actor;
using Akka.Event;

namespace Common
{
    public class GetRescreenDataActor : ReceiveActor
    {
        ILoggingAdapter loggingAdapter = Context.GetLogger();
        private IActorRef sender;

        public GetRescreenDataActor(IActorRef sender)
        {
            this.sender = sender;

            ReceiveAsync<string>(async u =>
            {
                loggingAdapter.Warning("Stant Get Result:" + u);
                loggingAdapter.Info("GetResults: " + await this.sender.Ask<string>("OOO", new TimeSpan(0, 0, 10)));
                loggingAdapter.Info("Screening start");
                Thread.Sleep(1000);
                loggingAdapter.Info("Screening finished");
                Self.Tell("Fetch result");
            });
        }
    }
}
