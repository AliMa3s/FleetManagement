using System;

namespace FleetManagement.Test.Exceptions
{
    [Serializable]
    internal class VoertuigNepRepoException : Exception
    {
        public VoertuigNepRepoException() { }

        public VoertuigNepRepoException(string message) : base(message) { }

        public VoertuigNepRepoException(string message, Exception innerException) : base(message, innerException) { }
    }
}