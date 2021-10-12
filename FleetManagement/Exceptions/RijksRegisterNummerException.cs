using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    [Serializable]
    internal class RijksRegisterNummerException : Exception
    {
        public RijksRegisterNummerException() { }

        public RijksRegisterNummerException(string message) : base(message) { }

        public RijksRegisterNummerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
