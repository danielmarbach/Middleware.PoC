using System;
using System.Net.Http;
using System.Threading.Tasks;
using EndpointB.Receiver.Messages.Commands;
using EndpointB.Receiver.Messages.Messages;
using Microsoft.Owin.Infrastructure;
using NServiceBus;
using NServiceBus.Logging;

namespace EndpointB.Receiver.Handlers
{
    class DoZSyncOverAsyncHandler : IHandleMessages<DoZSyncOverAsync>
    {
        static HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:18002")
        };

        static ILog log = LogManager.GetLogger<DoZSyncOverAsyncHandler>();

        public async Task Handle(DoZSyncOverAsync message, IMessageHandlerContext context)
        {
            log.Info($"Received message with customer id {message.CustomerId}");
            log.Info("Now calling ApplicationB Facade layer");

            var requestUri = WebUtilities.AddQueryString("/api/home", "customerId", message.CustomerId.ToString());

            var request = new HttpRequestMessage(HttpMethod.Put, requestUri);

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"\tReturned HTTP StatusCode: {response.StatusCode}\r\n\tContent: {content}");
            response.EnsureSuccessStatusCode();
            await context.Reply(new ZResponse {Content = content});
        }
    }
}