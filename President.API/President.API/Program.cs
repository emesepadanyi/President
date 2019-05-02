using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace President.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:5000", "https://localhost:5001", "http://152.66.183.126:5000")
                .UseStartup<Startup>();
    }
}
