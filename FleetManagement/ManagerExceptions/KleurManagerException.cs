using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ManagerExceptions
{
    [Serializable]
    public class KleurManagerException : Exception //internal => public
    {
        public KleurManagerException() { }

        public KleurManagerException(string message) : base(message) { }

        public KleurManagerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
