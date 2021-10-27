using System;

namespace FleetManagement.Test.Exceptions
{

    [Serializable]
    internal class BestuurderNepRepoException : Exception
    {
        public BestuurderNepRepoException() { }

        public BestuurderNepRepoException(string message) : base(message) { }

        public BestuurderNepRepoException(string message, Exception innerException) : base(message, innerException) { }
    }
}