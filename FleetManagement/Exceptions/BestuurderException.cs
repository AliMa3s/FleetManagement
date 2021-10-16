﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    [Serializable]
    internal class BestuurderException : Exception
    {
        public BestuurderException() { }

        public BestuurderException(string message) : base(message) { }

        public BestuurderException(string message, Exception innerException) : base(message, innerException) { }
    }
}