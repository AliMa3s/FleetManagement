using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    [Serializable]
    public class VoertuigBouwerException : Exception
    {
        public VoertuigBouwerException() { }

        public VoertuigBouwerException(string message) : base(message) { }

        public VoertuigBouwerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
