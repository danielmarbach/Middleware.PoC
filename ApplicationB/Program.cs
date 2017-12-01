using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Infrastructure;

namespace ApplicationB
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string baseUri = "http://localhost:18002";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);

            Console.WriteLine("\n\nPress a key to make a choice...");
            Console.WriteLine("[1] Execute DoY");
            Console.WriteLine("[2] Request status");
            Console.WriteLine("[x] Exit application");

            Guid orderId = Guid.Empty;
            while (true)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.X:
                        System.Environment.Exit(0);
                        continue;
                    case ConsoleKey.D1:
                        orderId = Guid.NewGuid();
                        await ContactFacade(HttpMethod.Post, orderId);
                        continue;
                    case ConsoleKey.D2:
                        await ContactFacade(HttpMethod.Get, orderId);
                        Console.WriteLine("Requested status, waiting for asynchronous reply!");
                        continue;
                }
            }

        }

        private static async Task ContactFacade(HttpMethod httpMethod, Guid orderId)
        {
            using (var httpClient = new HttpClient())
            {
                Console.WriteLine($"\tSending orderId {orderId}");

                var requestUri = WebUtilities.AddQueryString("/api/home", "orderId", orderId.ToString());

                httpClient.BaseAddress = new Uri("http://localhost:18004"); // EndpointB.Facade
                var request = new HttpRequestMessage(httpMethod, requestUri);

                var response = await httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
