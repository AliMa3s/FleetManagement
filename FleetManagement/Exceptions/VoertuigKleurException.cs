using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    class VoertuigKleurException : Exception
    {
        public VoertuigKleurException(string message) : base(message)
        {
        }

        public VoertuigKleurException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
