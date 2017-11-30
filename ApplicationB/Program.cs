using System;
using Microsoft.Owin.Hosting;

namespace ApplicationB
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUri = "http://localhost:18002";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);

            Console.ReadLine();
        }
    }
}
