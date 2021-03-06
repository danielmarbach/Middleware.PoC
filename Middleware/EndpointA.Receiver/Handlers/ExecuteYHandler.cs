﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using EndpointA.Receiver.Messages.Commands;
using EndpointB.Receiver.Messages.Messages;
using Microsoft.Owin.Infrastructure;
using NServiceBus;
using NServiceBus.Logging;

namespace EndpointA.Receiver.Handlers
{
    public class ExecuteYHandler : IHandleMessages<ExecuteYRequest>
    {
        private static HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:18001")
        };

        static ILog log = LogManager.GetLogger<ExecuteYHandler>();

        public async Task Handle(ExecuteYRequest message, IMessageHandlerContext context)
        {
            log.Info($"Received ReportyOnY message with order id {message.OrderId}");
            log.Info("Now calling ApplicationB Facade layer to return status");

            var requestUri = WebUtilities.AddQueryString("/api/home", "orderId", message.OrderId.ToString());

            await Task.Delay(5000); // simulate that ApplicationA is really slow.

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var newMessage = new ExecuteYResponse {OrderId = message.OrderId};
            await context.Reply(newMessage);
        }
    }
}
