using System;
using System.Net.Http;
using System.Threading.Tasks;
using EndpointB.Receiver.Messages.Commands;
using Microsoft.Owin.Infrastructure;
using NServiceBus;
using NServiceBus.Logging;

namespace EndpointB.Receiver.Handlers
{
    class DoXHandler : IHandleMessages<DoX>
    {
        static HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:18002")
        };

        static ILog log = LogManager.GetLogger<DoXHandler>();

        public async Task Handle(DoX message, IMessageHandlerContext context)
        {
            log.Info($"Received message with customer id {message.CustomerId}");
            log.Info("Now calling ApplicationB Facade layer");

            var requestUri = WebUtilities.AddQueryString("/api/home", "customerId", message.CustomerId.ToString());

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var response = await httpClient.SendAsync(request);

            Console.WriteLine($"\tReturned HTTP StatusCode: {response.StatusCode}");
            response.EnsureSuccessStatusCode();
        }
    }
}
