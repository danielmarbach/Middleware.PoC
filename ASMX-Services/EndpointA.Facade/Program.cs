using System;
using Microsoft.Owin.Hosting;

namespace EndpointA.Facade
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUri = "http://localhost:18003";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);
            
            Console.ReadLine();
        }
    }
}
