using System;
using System.Runtime.Serialization;

namespace FleetManagement.ADO.Repositories {
    [Serializable]
    internal class AutoModelRepositoryADOException : Exception {
        public AutoModelRepositoryADOException() {
        }

        public AutoModelRepositoryADOException(string message) : base(message) {
        }

        public AutoModelRepositoryADOException(string message, Exception innerException) : base(message, innerException) {
        }

        protected AutoModelRepositoryADOException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}