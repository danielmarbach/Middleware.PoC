using System;
using System.Threading.Tasks;
using EndpointB.Receiver.Messages.Messages;
using NServiceBus;

namespace EndpointA.Receiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("EndpointA.Receiver");
            var routing = endpointConfiguration.UseTransport<LearningTransport>().Routing();
            endpointConfiguration.UsePersistence<LearningPersistence>();

            endpointConfiguration.SendFailedMessagesTo("error");

            routing.RouteToEndpoint(typeof(ExecuteYResponse).Assembly, "EndpointB.Receiver");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));
            conventions.DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"));

            var endpoint = await Endpoint.Start(endpointConfiguration);

            Console.ReadLine();
        }
    }
}
