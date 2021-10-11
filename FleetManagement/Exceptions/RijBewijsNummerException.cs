using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    [Serializable]
    internal class RijBewijsNummerException : Exception
    {
        public RijBewijsNummerException() { }

        public RijBewijsNummerException(string message) : base(message) { }

        public RijBewijsNummerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
