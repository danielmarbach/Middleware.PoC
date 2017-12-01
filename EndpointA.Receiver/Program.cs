using System;
using System.Threading.Tasks;
using NServiceBus;

namespace EndpointA.Receiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("EndpointA.Receiver");
            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.UsePersistence<LearningPersistence>();

            endpointConfiguration.SendFailedMessagesTo("error");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));

            var endpoint = await Endpoint.Start(endpointConfiguration);

            Console.ReadLine();
        }
    }
}
