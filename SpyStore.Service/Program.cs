using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SpyStore.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
           var host = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
               .UseUrls("http://*.40001/")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseApplicationInsights()
                .Build();

            return host;

        }
           
    }
}
