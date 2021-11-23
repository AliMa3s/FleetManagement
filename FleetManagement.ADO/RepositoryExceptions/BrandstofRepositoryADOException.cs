using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.RepositoryExceptions
{
    public class BrandstofRepositoryADOException : Exception
    {
        public BrandstofRepositoryADOException()
        {
        } 

        public BrandstofRepositoryADOException(string message) : base(message)
        {
        }

        public BrandstofRepositoryADOException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
