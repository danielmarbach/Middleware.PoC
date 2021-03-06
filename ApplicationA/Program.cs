﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Infrastructure;

namespace ApplicationA
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:18003")
        };

        static async Task Main(string[] args)
        {
            string baseUri = "http://localhost:18001";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);

            Console.WriteLine("\n\nPress a key to make a choice...");
            Console.WriteLine("[1] Execute DoX");
            Console.WriteLine("[2] Execute DoZSyncOverAsync");
            Console.WriteLine("[x] Exit application");

            while (true)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.X:
                        Environment.Exit(0);
                        continue;
                    case ConsoleKey.D1:
                        var customerId = Guid.NewGuid();
                        Console.WriteLine($"\tSending customerId {customerId}");

                        var requestUri = WebUtilities.AddQueryString("/api/home", "customerId", customerId.ToString());

                        var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

                        var response = await httpClient.SendAsync(request);

                        Console.WriteLine($"\tReturned HTTP StatusCode: {response.StatusCode}");
                        response.EnsureSuccessStatusCode();
                        continue;
                    case ConsoleKey.D2:
                        customerId = Guid.NewGuid();
                        Console.WriteLine($"\tSending customerId {customerId}");

                        requestUri = WebUtilities.AddQueryString("/api/home", "customerId", customerId.ToString());

                        request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                        response = await httpClient.SendAsync(request);
                        var content = await response.Content.ReadAsStringAsync();

                        Console.WriteLine($"\tReturned HTTP StatusCode: {response.StatusCode}\r\n\tContent: {content}");
                        response.EnsureSuccessStatusCode();
                        continue;
                }
            }
        }
    }
}