using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Loony.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseUrls("http://192.168.1.23:5000");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
