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
               //требуется для core > 2.0
               /*
               см  https://blogs.msdn.microsoft.com/dotnet/2017/05/12/announcing-ef-core-2-0-preview-1/
                */

               .UseDefaultServiceProvider(options =>options.ValidateScopes = false)
                .Build();

            return host;

        }
           
    }
}
