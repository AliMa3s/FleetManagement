using System;
using System.Runtime.Serialization;

namespace FleetManagement.Models {
    [Serializable]
    internal class TankKaartException : Exception {
        public TankKaartException() {
        }

        public TankKaartException(string message) : base(message) {
        }

        public TankKaartException(string message, Exception innerException) : base(message, innerException) {
        }


    }
}