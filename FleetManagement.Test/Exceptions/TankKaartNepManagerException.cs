using System;

namespace FleetManagement.Test.Exceptions
{
    [Serializable]
    internal class TankKaartNepManagerException : Exception
    {
        public TankKaartNepManagerException() { }

        public TankKaartNepManagerException(string message) : base(message) { }

        public TankKaartNepManagerException(string message, Exception innerException) : base(message, innerException) { }
    }
}