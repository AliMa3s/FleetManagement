using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions {
    public class CheckFormatException : Exception {
        public CheckFormatException(string message) : base(message) {
        }

        public CheckFormatException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
