using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using LogLevel = NLog.LogLevel;

namespace GR.Logger.Extensions
{
    public static class LoggerExtension
    {
        public static IWebHostBuilder RegisterGearLoggingProviders(this IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //var sp = services.BuildServiceProvider();  -->  for inject some services
                var config = new NLog.Config.LoggingConfiguration();
                var logConsole = new NLog.Targets.ConsoleTarget();
                var customTarget = new GearLoggerTarget();
                config.AddRule(LogLevel.Trace, LogLevel.Fatal, logConsole);
                config.AddRule(LogLevel.Trace, LogLevel.Fatal, customTarget);

                LogManager.Configuration = config;
            });

            builder/*.ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    //logging.ClearProviders();
                    //logging.AddFilter("Microsoft", Microsoft.Extensions.Logging.LogLevel.Trace);
                    //logging.AddFilter("System", Microsoft.Extensions.Logging.LogLevel.Trace);
                })*/
                .UseNLog();

            return builder;
        }
    }
}
