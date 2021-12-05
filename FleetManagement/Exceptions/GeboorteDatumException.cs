using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    [Serializable]
    public class GeboorteDatumException : Exception
    {
        public GeboorteDatumException() { }

        public GeboorteDatumException(string message) : base(message) { }

        public GeboorteDatumException(string message, Exception innerException) : base(message, innerException) { }
    }
}
