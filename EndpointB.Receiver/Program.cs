using System;
using System.Threading.Tasks;
using NServiceBus;

namespace EndpointA.Receiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("EndpointB.Receiver");
            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.UsePersistence<LearningPersistence>();

            endpointConfiguration.SendFailedMessagesTo("error");

            var endpoint = await Endpoint.Start(endpointConfiguration);

            Console.ReadLine();
        }
    }
}
