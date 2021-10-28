using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.RepositoryExceptions {
    public class BestuurderRepositoryADOException : Exception {
        public BestuurderRepositoryADOException() {
        }

        public BestuurderRepositoryADOException(string message) : base(message) {
        }

        public BestuurderRepositoryADOException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
