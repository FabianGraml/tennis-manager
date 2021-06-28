using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TennisDbLib;

namespace Tennis
{
    public class Program
    {
        private static TennisContext db = new TennisContext();
        public static void Main(string[] args)
        {
            db.Database.Migrate();
            int nr = db.Persons.Count();
            Console.WriteLine($"{nr} Persons");
            db.Dispose();
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
