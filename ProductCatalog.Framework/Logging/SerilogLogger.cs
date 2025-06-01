using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ProductCatalog.Framework.Logging
{
    public class SerilogLogger
    {
        public static ILogger Initialize()
        {
            var configuration = new ConfigurationBuilder()
                                   .SetBasePath(Directory.GetCurrentDirectory())
                                   .AddJsonFile("appsettings.LogConfigs.json")
                                   .Build();

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();

            return Log.Logger;
        }
    }
}