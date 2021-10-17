﻿using System;

namespace FleetManagement.Exceptions
{
    [Serializable]
    internal class NummerPlaatException : Exception
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