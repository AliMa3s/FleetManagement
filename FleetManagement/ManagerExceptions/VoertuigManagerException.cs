using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ManagerExceptions {
    public class VoertuigManagerException : Exception {
        public VoertuigManagerException() {
        }

        public VoertuigManagerException(string message) : base(message) {
        }

        public VoertuigManagerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
