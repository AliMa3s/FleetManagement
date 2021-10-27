﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.Repositories.Connections
{
    class BestuurderConnection : ConfigurationDB
    {
        private readonly string _connectionString;

        public virtual string ConnectionString => _connectionString;

        public BestuurderConnection() : base() 
        {
            _connectionString = _config.GetConnectionString("FleetManagerConnectionString");
        }
    }
}
