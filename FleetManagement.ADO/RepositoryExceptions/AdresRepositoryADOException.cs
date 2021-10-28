using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.RepositoryExceptions {
    public class AdresRepositoryADOException : Exception {
        public AdresRepositoryADOException() {
        }

        public AdresRepositoryADOException(string message) : base(message) {
        }

        public AdresRepositoryADOException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
