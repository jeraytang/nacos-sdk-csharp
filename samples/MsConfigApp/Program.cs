using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace MsConfigApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Debug()
                .WriteTo.Console(
                    outputTemplate: outputTemplate)
                /*.WriteTo.File(
                    path: "logs/ApiTpl.log",
                    outputTemplate: outputTemplate,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 5,
                    encoding: System.Text.Encoding.UTF8)*/
                .CreateLogger();

            try
            {
                Log.ForContext<Program>().Information("Application starting...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                Log.ForContext<Program>().Fatal(ex, "Application start-up failed!!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((context, builder) =>
                 {
                     var c = builder.Build();

                     var dataId = c.GetValue<string>("NacosConfig:DataId");
                     var group = c.GetValue<string>("NacosConfig:Group");
                     var tenant = c.GetValue<string>("NacosConfig:Tenant");
                     var optional = c.GetValue<bool>("NacosConfig:Optional");
                     var serverAddresses = c.GetSection("NacosConfig:ServerAddresses").Get<List<string>>();

                     // read configuration from config files
                     builder.AddNacosConfiguration(c.GetSection("NacosConfig"));

                     //// hard code here
                     //builder.AddNacosConfiguration(x =>
                     //{
                     //    x.DataId = dataId;
                     //    x.Group = group;
                     //    x.Tenant = tenant;
                     //    x.Optional = optional;
                     //    x.ServerAddresses = serverAddresses;
                     //});
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .UseSerilog();
    }
}