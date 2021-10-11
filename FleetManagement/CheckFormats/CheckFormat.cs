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
        public static bool IsPincodeGeldig(string pincode)
        {
            //Pincode mag in principe LEEG zijn in TankKaart, maar hier niet! 
            //We mogen geen exception gooien voor iets wat is toegestaan.

            int lengtePincode = pincode.Length;

            if(lengtePincode == 4 || lengtePincode == 5)
            {
                if (Int32.TryParse(pincode, out int nr))
                {
                    return true;
                }
            }

            throw new CheckFormatException("Pincode voeldoet niet aan het juiste formaat");
        }


        public static bool IsRijbewijsGeldig(string rijbewijsType) {
            throw new CheckFormatException("Controle nummer plaat nog te implementeren");
        }

        public static bool IsChassisNummerGeldig(string chassisNummer) {
            throw new CheckFormatException("Controle chassisnummer nog te implementeren");
        }

        public static bool IsNummerplaatGeldig()
        {
            throw new CheckFormatException("Controle nummer plaat nog te implementeren");
        }

        internal static bool IsRijksRegisterNumberGeldig(string rijksregisternummer, DateTime date)
        {
            throw new NotImplementedException();
        }

        public static bool IsRijksRegisterGeldig(string rijksRegisterNummer)
        {
            int checksum = CheckSum(1672339, 97);  //59
            throw new CheckFormatException("Controle rijksregister nog te implementeren");
        }

        public static bool IsTankKaartGeldig(string tankKaartNummer, int pincode)
        {
            throw new CheckFormatException("Controle TankKaart nog te implementeren");
        }

        private static int CheckSum(int number, int modulo)
        {
            return number % modulo;
        }
    }
}
