using System;
using System.Threading.Tasks;
using EndpointA.Receiver.Messages.Commands;
using EndpointB.Receiver.Messages.Commands;
using NServiceBus;

namespace EndpointA.Receiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("EndpointB.Receiver");
            var routing = endpointConfiguration.UseTransport<LearningTransport>().Routing();
            endpointConfiguration.UsePersistence<LearningPersistence>();

            endpointConfiguration.SendFailedMessagesTo("error");

            routing.RouteToEndpoint(typeof(ReportOnY).Assembly, "EndpointB.Receiver");
            routing.RouteToEndpoint(typeof(ExecuteYRequest).Assembly, "EndpointA.Receiver");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));
            conventions.DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"));

            var endpoint = await Endpoint.Start(endpointConfiguration);

            Console.ReadLine();
        }
    }
}
