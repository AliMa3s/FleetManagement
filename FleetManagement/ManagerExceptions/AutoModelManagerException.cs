using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ManagerExceptions
{
    [Serializable]
    public class AutoModelManagerException : Exception //internal => public
    {
        public AutoModelManagerException() { }

        public AutoModelManagerException(string message) : base(message) { }

        public AutoModelManagerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
