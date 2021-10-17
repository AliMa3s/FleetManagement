using System;

namespace FleetManagement.Test.Exceptions
{
    [Serializable]
    internal class VoertuigNepManagerException : Exception
    {
        public VoertuigNepManagerException() { }

        public VoertuigNepManagerException(string message) : base(message) { }

        public VoertuigNepManagerException(string message, Exception innerException) : base(message, innerException) { }
    }
}