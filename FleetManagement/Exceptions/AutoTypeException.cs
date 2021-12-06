using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    public class AutoTypeException : Exception
    {
        public AutoTypeException(string message) : base(message)
        {
        }

        public AutoTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
