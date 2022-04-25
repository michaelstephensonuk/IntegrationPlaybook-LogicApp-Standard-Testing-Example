using IPB.LogicApp.Standard.Testing;
using IPB.LogicApp.Standard.Testing.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogicApp.Testing.AcceptanceTests.Helpers
{
    public class ConfigHelper
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            return config;
        }



    }
}