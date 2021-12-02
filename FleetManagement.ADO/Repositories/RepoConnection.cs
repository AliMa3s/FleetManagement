using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.Repositories
{
    public abstract class RepoConnection
    {
        private readonly string _connectionString;
        private SqlConnection _conn;

        public RepoConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected SqlConnection Connection
        {
            get
            {
                if (_conn == null)
                {
                    _conn = new SqlConnection(_connectionString);
                }

                return _conn;
            }
            set => _conn = value;
        }
    }
}
