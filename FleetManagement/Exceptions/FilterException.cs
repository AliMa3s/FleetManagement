using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    public class FilterException : Exception
    {
        public FilterException(string message) : base(message)
        {
        }

        public FilterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
