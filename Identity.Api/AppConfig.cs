using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Api.Services
{
    internal class AppConfig
    {
        public static IConfiguration Configuration { get; set; }

        static AppConfig()
        {
            if (Configuration == null)
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.Development.json")
                    .Build();
            }
        }
        public static string? GetConnectionString() => Configuration.GetConnectionString("Default");
    }
}
