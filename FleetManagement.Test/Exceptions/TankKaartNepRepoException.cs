using System;

namespace FleetManagement.Test.Exceptions
{
    [Serializable]
    internal class TankKaartNepRepoException : Exception
    {
        public TankKaartNepRepoException() { }

        public TankKaartNepRepoException(string message) : base(message) { }

        public TankKaartNepRepoException(string message, Exception innerException) : base(message, innerException) { }
    }
}