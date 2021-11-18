using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.WPF.exceptions
{
    [Serializable]
    public class AantalDeurenException : Exception
    {
        public AantalDeurenException()
        {
        }

        public AantalDeurenException(string message) : base(message)
        {
        }

        public AantalDeurenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AantalDeurenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
