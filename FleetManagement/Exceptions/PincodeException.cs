using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    [Serializable]
    internal class PincodeException : Exception
    {
        public PincodeException() { }

        public PincodeException(string message) : base(message) { }

        public PincodeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
