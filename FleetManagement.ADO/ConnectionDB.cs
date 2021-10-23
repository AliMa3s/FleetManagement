using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO
{
    public abstract class ConnectionDB
    {
        private readonly string _connectionString = 
            ConfigurationManager.ConnectionStrings["FleetManagerConnectionString"].ConnectionString;

        protected virtual string ConnectionString => _connectionString;
    }
}
