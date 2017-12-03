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
    class ReportOnYHandler : IHandleMessages<ReportOnY>
    {
        static HttpClient httpClient = new HttpClient();

        static ILog log = LogManager.GetLogger<ReportOnYHandler>();

        public async Task Handle(ReportOnY message, IMessageHandlerContext context)
        {
            log.Info($"Received ReportyOnY message with order id {message.OrderId}");
            log.Info("Now calling ApplicationB Facade layer to return status");

            var requestUri = WebUtilities.AddQueryString("/api/verify", new Dictionary<string, string>()
            {
                {"orderId", message.OrderId.ToString()},
                {"status", message.Status }
            });

            httpClient.BaseAddress = new Uri("http://localhost:18002");
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
    }
}
