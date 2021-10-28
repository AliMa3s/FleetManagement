using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.RepositoryExceptions {
    public class TankkaarRepositoryADOException : Exception {
        public TankkaarRepositoryADOException() {
        }

        public TankkaarRepositoryADOException(string message) : base(message) {
        }

        public TankkaarRepositoryADOException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
