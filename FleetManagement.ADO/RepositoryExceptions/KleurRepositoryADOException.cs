using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.RepositoryExceptions
{
    public class KleurRepositoryADOException : Exception
    {
        public KleurRepositoryADOException()
        {
        }

        public KleurRepositoryADOException(string message) : base(message)
        {
        }

        public KleurRepositoryADOException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
