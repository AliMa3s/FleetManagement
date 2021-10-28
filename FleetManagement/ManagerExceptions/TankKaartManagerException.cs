using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ManagerExceptions {
    public class TankKaartManagerException : Exception {
        public TankKaartManagerException() {
        }

        public TankKaartManagerException(string message) : base(message) {
        }

        public TankKaartManagerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
