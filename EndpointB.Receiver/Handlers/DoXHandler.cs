using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EndpointB.Receiver.Messages.Commands;
using Microsoft.Owin.Infrastructure;
using NServiceBus;
using NServiceBus.Logging;

namespace EndpointB.Receiver.Handlers
{
    class DoXHandler : IHandleMessages<DoX>
    {
        static ILog log = LogManager.GetLogger<DoXHandler>();

        public async Task Handle(DoX message, IMessageHandlerContext context)
        {
            log.Info($"Received message with customer id {message.CustomerId}");
            log.Info("Now calling ApplicationB Facade layer");

            var requestUri = WebUtilities.AddQueryString("/api/home", "customerId", message.CustomerId.ToString());

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:18002");
                var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

                var response = httpClient.SendAsync(request)
                    .GetAwaiter()
                    .GetResult();

                Console.WriteLine($"\tReturned HTTP StatusCode: {response.StatusCode}");
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
