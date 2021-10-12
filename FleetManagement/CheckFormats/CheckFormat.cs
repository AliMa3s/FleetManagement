using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.CheckFormats
{
    public static class CheckFormat
    {
        public static bool IsPincodeGeldig(string pincode = "")
        {
            //We mogen geen exception gooien voor iets wat toegestaan is.
            if (pincode == string.Empty)
                return true;

            int lengtePincode = pincode.Length;

            if(lengtePincode == 4 || lengtePincode == 5)
            {
                if (Int32.TryParse(pincode.TrimStart('0'), out int nr) 
                    && nr >= 0 
                    && nr < 100000)      
                {
                    return true;
                }
            }

            throw new PincodeException($"{nameof(pincode)} voeldoet niet aan het juiste formaat"); 
        }

        public static bool IsRijbewijsNummerGeldig(string rijBewijsNummer) {
            throw new RijBewijsNummerException($"Controle {nameof(rijBewijsNummer)} nummer nog te implementeren");
        }

        public static bool IsChassisNummerGeldig(string chassisNummer) {
            throw new CheckFormatException($"Controle {nameof(chassisNummer)} nog te implementeren");
        }

        public static bool IsNummerplaatGeldig(string nummerPlaat)
        {
            throw new CheckFormatException($"Controle {nameof(nummerPlaat)} nog te implementeren");
        }

        internal static bool IsRijksRegisterGeldig(string rijksRegisterNummer, DateTime date)
        {
            throw new RijksRegisterNummerException($" {nameof(rijksRegisterNummer)} Not implemented");
        }

        public static bool IsTankKaartGeldig(string tankKaartNummer, string pincode = "")  
        {
            throw new CheckFormatException($"Controle {nameof(tankKaartNummer)} nog te implementeren");
        }

        private static int CheckSum(int number, int modulo)
        {
            return number % modulo;
        }
    }
}
