using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    [Serializable]
    public class AutoModelException : Exception
    {
        public AutoModelException() { }

        public AutoModelException(string message) : base(message) { }

        public AutoModelException(string message, Exception innerException) : base(message, innerException) { }
    }
}
