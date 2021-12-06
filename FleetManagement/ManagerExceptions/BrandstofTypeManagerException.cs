using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ManagerExceptions
{
    [Serializable]
    public class BrandstofTypeManagerException : Exception //internal => public
    {
        public BrandstofTypeManagerException() { }

        public BrandstofTypeManagerException(string message) : base(message) { }

        public BrandstofTypeManagerException(string message, Exception innerException) : base(message, innerException) { }
    }

}
