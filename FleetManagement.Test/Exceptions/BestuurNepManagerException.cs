using System;

namespace FleetManagement.Test.Exceptions
{

    [Serializable]
    internal class BestuurderNepManagerException : Exception
    {
        public BestuurderNepManagerException() { }

        public BestuurderNepManagerException(string message) : base(message) { }

        public BestuurderNepManagerException(string message, Exception innerException) : base(message, innerException) { }
    }
}