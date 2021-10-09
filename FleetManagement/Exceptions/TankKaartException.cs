using System;
using System.Runtime.Serialization;

//Goede implementatieregel voor exceptions, moesten we niet gebruiken. Maar dat wordt nu ook een regel van mij. Zeer interessant
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