using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Main.Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseKestrel(options =>
                   {
                       options.AddServerHeader = true;
                       options.AllowSynchronousIO = true;
                       // Set properties and call methods on options
                   });
    }
}
