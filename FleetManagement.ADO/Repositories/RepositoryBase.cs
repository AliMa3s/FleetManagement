using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.Repositories
{
    public abstract class RepositoryBase : IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection _conn;

        private bool Disposing { get; set; }

        public RepositoryBase(string connectionString)
        {
            _connectionString = connectionString;
            Disposing = false;
        }

        protected SqlConnection Connection
        {
            get {
                if(_conn == null)
                {
                    _conn = new SqlConnection(_connectionString);
                    Disposing = true;
                }

                return _conn;
            }
        }

        public void Dispose()
        {
            Dispose(Disposing);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _conn.Dispose();
                _conn = null;
                Disposing = false;
            }
        }
    }
}
