using System;
using System.Runtime.Serialization;

namespace FleetManagement.Model {
    [Serializable]
    public class BrandstofTypeException : Exception {
        public BrandstofTypeException() {
        }

        public BrandstofTypeException(string message) : base(message) {
        }

        public BrandstofTypeException(string message, Exception innerException) : base(message, innerException) {
        }

        protected BrandstofTypeException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}