﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Benaming geeft wat verwarring voor mij. suggestie: CheckFormatException() 
//Net zoals class CheckFormat()
namespace FleetManagement.Exceptions {
    public class StaticCheckersException : Exception {
        public StaticCheckersException(string message) : base(message) {
        }

        public StaticCheckersException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}