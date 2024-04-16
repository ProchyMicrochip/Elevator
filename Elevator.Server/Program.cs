using System.Globalization;
using Microsoft.AspNetCore;
using NLog.Web;

namespace Elevator.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Main enter");
                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("cs-CZ");
                CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture; //CultureInfo.InvariantCulture;
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentUICulture;
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;
                BuildWebHost(args).Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .UseStartup<Startup>()
                //.ConfigureKestrel(options => { options.ListenAnyIP(80); })
                .Build();
    }
}