using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RM.Models;

namespace RM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SocketManager smse = new SocketManager("127.0.0.1",8000,flagTypeServer.Send);

            // Create the thread object, passing in the
            // serverObject.InstanceMethod method using a
            // ThreadStart delegate.
            Thread threadforsend = new Thread(
                new ThreadStart(smse.Run));
            threadforsend.Start();

            SocketManager smac = new SocketManager("127.0.0.2", 8080,flagTypeServer.Accept);

            // Create the thread object, passing in the
            // serverObject.InstanceMethod method using a
            // ThreadStart delegate.
            Thread threadforac = new Thread(
                new ThreadStart(smac.Run));
            threadforac.Start();



            // Start the thread.
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
