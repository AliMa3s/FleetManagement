using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ManagerExceptions {
    public class AdresManagerException : Exception {
        public AdresManagerException() {
        }

        public AdresManagerException(string message) : base(message) {
        }

        public AdresManagerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
