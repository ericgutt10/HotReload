using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;


namespace BlazorReloaded04
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       // Work around https://github.com/dotnet/coreclr/issues/22812
                       // Disable hosting startup for now
                       webBuilder.UseSetting(WebHostDefaults.PreventHostingStartupKey, "true");

                       webBuilder.UseStartup<Startup>();
                   })
                   .UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }
    }
}
