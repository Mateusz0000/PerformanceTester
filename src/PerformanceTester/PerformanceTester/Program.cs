using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using System.Net;
using System.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;

namespace PerformanceTester
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget()
            {
                Name = "File",
                FileName = "logs/log.txt",
                ArchiveFileName = "log.{#}.txt",
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveDateFormat = "yyyy-MM-dd",              
            };
            config.AddRule(LogLevel.Info, LogLevel.Fatal, fileTarget, "*");
            LogManager.Configuration = config;

            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
#if KESTREL
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Limits.MaxConcurrentConnections = 1200;
                        serverOptions.Limits.MaxConcurrentUpgradedConnections = 1200;
                        serverOptions.Listen(IPAddress.Any, 5004, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                            listenOptions.UseHttps(new HttpsConnectionAdapterOptions()
                            {
                                SslProtocols = SslProtocols.Tls12,
                                ServerCertificate = new X509Certificate2(File.ReadAllBytes("https/localhost.pfx"), 
                                    ConvertToSecureString())
                            });
                        });
                        serverOptions.Listen(IPAddress.Any, 5005);
                    });
                    //webBuilder.UseUrls("http://localhost:5005", "https://localhost:5004");
                    webBuilder.UseKestrel();
#endif

#if (IIS || RELEASE || DEBUG)
                    webBuilder.UseIIS();
#endif
                    webBuilder.UseStartup<Startup>();
                })
                .UseNLog();

        private static SecureString ConvertToSecureString()
        {
            SecureString pass = new SecureString();
            Array.ForEach("P@ssw0rd".ToArray(), pass.AppendChar);

            return pass;
        }
    }
}
