using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Model;

namespace FleetManagement.ADO.Repositories 
{
    class ConnectionDB
    {
        private readonly string _connectionString;

        public virtual string ConnectionString => _connectionString;

        public ConnectionDB()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = config.GetConnectionString("FleetManagerConnectionString");
        }
    }
}
