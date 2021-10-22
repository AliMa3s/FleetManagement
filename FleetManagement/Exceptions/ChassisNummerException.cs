using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    [Serializable]
    public class ChassisNummerException : Exception
    {
        public ChassisNummerException() { }

        public ChassisNummerException(string message) : base(message) { }

        public ChassisNummerException(string message, Exception innerException) : base(message, innerException) { }
    }
}