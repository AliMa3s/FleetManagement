using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Model;

namespace FleetManagement.ADO.Repositories.Connections
{
    abstract class ConfigurationDB
    {
        protected readonly IConfiguration _config;

        public ConfigurationDB()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
    }
}
