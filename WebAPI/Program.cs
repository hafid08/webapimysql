using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel(o =>
            {
                o.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(60);
                o.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(60);
            }).UseContentRoot(Directory.GetCurrentDirectory())
            .UseUrls("http://localhost:4242")
            .UseStartup<Startup>();
    }
}
