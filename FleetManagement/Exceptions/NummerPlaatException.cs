using System;

namespace FleetManagement.Exceptions
{
    public class NummerPlaatException : Exception
    {
        public NummerPlaatException()
        {
        }

        public NummerPlaatException(string message) : base(message)
        {
        }

        public NummerPlaatException(string message, Exception innerException) : base(message, innerException)
        {
        }


    }
}