using System;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace EndpointB.Facade
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUri = "http://localhost:18004";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);

            Console.ReadLine();
        }
    }
}
