using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.RepositoryExceptions
{
    [Serializable]
    internal class AantalDeurenRepositoryException : Exception
    {
        public AantalDeurenRepositoryException()
        {
        }

        public AantalDeurenRepositoryException(string message) : base(message)
        {
        }

        public AantalDeurenRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AantalDeurenRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
