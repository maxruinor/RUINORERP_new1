using System;
using System.Threading;
using RUINORERP.SimpleHttp;
using System.IO;
using System.Linq;

namespace RUINORERP.WebServerConsole
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Running HTTP server on: ");
            test();
        }

        private static void test()
        {
            WebServer server = new WebServer();
            server.RunWebServer();
            Console.ReadLine();
        }

    }
}
