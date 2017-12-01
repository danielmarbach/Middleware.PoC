using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Infrastructure;
using Newtonsoft.Json.Linq;

namespace ApplicationA
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUri = "http://localhost:18001";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);

            Console.WriteLine("\n\nPress a key to make a choice...");
            Console.WriteLine("[1] Execute DoX");
            Console.WriteLine("[x] Exit application");

            while (true)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.X:
                        System.Environment.Exit(0);
                        continue;
                    case ConsoleKey.D1:
                        using (var httpClient = new HttpClient())
                        {
                            var customerId = Guid.NewGuid();
                            Console.WriteLine($"\tSending customerId {customerId}");

                            var requestUri = WebUtilities.AddQueryString("/api/home", "customerId", customerId.ToString());

                            httpClient.BaseAddress = new Uri("http://localhost:18003");
                            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

                            var response = httpClient.SendAsync(request)
                                            .GetAwaiter()
                                            .GetResult();

                            Console.WriteLine($"\tReturned HTTP StatusCode: {response.StatusCode}");
                            response.EnsureSuccessStatusCode();
                        }
                        continue;
                }
            }
        }
    }
}