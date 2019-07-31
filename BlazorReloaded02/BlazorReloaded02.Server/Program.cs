using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BlazorReloaded02.Server
{
    public class Program
    {
        static readonly string ProjectName = "BlazorReload01";
        static readonly string TargetFrameWork = "netcoreapp3.0";
        static readonly string ProjectPath = Path.GetFullPath($@"..\{ProjectName}\");
        static readonly string DllPath = Path.Combine(ProjectPath, $@"bin\Debug\{TargetFrameWork}\{ProjectName}.dll");

        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();

            new HostBuilder()
                    .UseContentRoot(ProjectPath)
                    .ConfigureLogging(logging =>
                    {
                        logging.AddConsole()
                               .AddFilter("Watcher", LogLevel.Debug)
                               .SetMinimumLevel(LogLevel.Warning);
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddHostedService<WatcherService>();
                        services.AddSingleton<HostingServer>();

                        services.Configure<ProjectOptions>(o =>
                        {
                            o.ProjectName = ProjectName;
                            o.ProjectPath = ProjectPath;
                            o.DllPath = DllPath;
                            o.DotNetPath = DotNetMuxer.MuxerPathOrDefault();
                            o.Args = args;
                        });
                    })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.Configure(app =>
                        {
                            app.UseDeveloperExceptionPage();

                            var server = app.ApplicationServices.GetRequiredService<HostingServer>();

                            app.Run(async context =>
                            {
                                var application = await server.WaitForApplicationAsync(default);

                                await application(context);
                            });
                        });
                    })
                    .Build()
                    .Run();
        }
    }
}
