using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.RepositoryExceptions {
    public class VoertuigRepositoryADOException : Exception {
        public VoertuigRepositoryADOException() {
        }

        public VoertuigRepositoryADOException(string message) : base(message) {
        }

        public VoertuigRepositoryADOException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
