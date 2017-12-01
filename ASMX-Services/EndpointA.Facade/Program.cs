﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndpointA.Facade
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUri = "http://localhost:18001";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);

            Console.ReadLine();
        }
    }
}
