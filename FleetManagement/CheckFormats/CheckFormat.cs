using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleetManagement.CheckFormats
{
    public static class CheckFormat
    {
        public static bool IsPincodeGeldig(string pincode)
        {
            //We mogen geen exception gooien voor iets wat toegestaan is.
            if (pincode == string.Empty)
                return true;

            return Regex.IsMatch(pincode, @"^[0-9]{4,5}$")
                ? true : throw new PincodeException($"{nameof(pincode)} moet een string zijn met 4 of 5 cijfers");
        }

        public static bool IsRijbewijsNummerGeldig(string rijBewijsNummer)
        {

            return Regex.IsMatch(rijBewijsNummer, @"^[0-9]{10}$")
                ? true : throw new RijBewijsNummerException($"{nameof(rijBewijsNummer)} moet een string zijn van 10 cijfers");
        }

        public static bool IsChassisNummerGeldig(string chassisNummer)
        {

            return Regex.IsMatch(chassisNummer.ToUpper(), @"^[0-9A-HJ-NPR-Z]{17}$")
                ? true : throw new ChassisNummerException($"{nameof(chassisNummer)} moet string zijn van 17 cijfers/letters" +
                $" maar letter I/i, O/o en Q/q mag niet voorkomen");
        }

        public static bool IsNummerplaatGeldig(string nummerPlaat)
        {
            return Regex.IsMatch(nummerPlaat.ToUpper(), @"^[1-9]{1}[A-Z]{3}[0-9]{3}$")
                ? true : throw new NummerPlaatException($"{nameof(nummerPlaat)} moet format [1-9][a-z][0-9] zijn");
        }

        public static bool IsRijksRegisterGeldig(string rijksRegisterNummer, string geboorteDatum)
        {
            //Controleer eerst het format
            if (Regex.IsMatch(rijksRegisterNummer.ToUpper(), @"^[0-9]{11}$"))
            {
                string laatsteTweeCijfersGeboorteDatum = geboorteDatum.Substring(2, 2);
                string jaar = rijksRegisterNummer.Substring(0, 2);
                string maand = rijksRegisterNummer.Substring(2, 2);
                string dag = rijksRegisterNummer.Substring(4, 2);
                string getallenreeks = rijksRegisterNummer.Substring(6, 3);

                //Controleer of Bestuurder is geboren in of na 2000
                if (Int32.Parse(laatsteTweeCijfersGeboorteDatum) >= 2000)
                {
                    jaar = "2" + jaar;
                }

                //Controleer de inhoud van het format
                if (jaar == laatsteTweeCijfersGeboorteDatum
                    && IsRangeGeldig(dag, 0, 31)
                    && (IsRangeGeldig(maand, 0, 12) || IsRangeGeldig(maand, 20, 32) | IsRangeGeldig(maand, 40, 52))
                    && IsRangeGeldig(getallenreeks, 0, 998)
                    && CheckSum(jaar + maand + dag + getallenreeks, rijksRegisterNummer.Substring(9, 2)))
                {
                    return true;
                }

                throw new RijksRegisterNummerException($" {nameof(rijksRegisterNummer)} De inhoud van het format bevat fouten");
            }
            else
            {
                throw new RijksRegisterNummerException($" {nameof(rijksRegisterNummer)} is niet het juiste format");
            }
        }

        public static bool IsTankKaartNummerGeldig(string tankKaartNummer)  //Heeft TankKaart modulo 97?
        {
            return Regex.IsMatch(tankKaartNummer.ToUpper(), @"^[0-9]{19}$")
                ? true : throw new TankKaartException($" {nameof(tankKaartNummer)} is niet het juiste format");
        }

        private static bool IsRangeGeldig(string nummer, int min, int max)
        {
            int getal = Int32.Parse(nummer);
            return getal >= min && getal <= max;
        }

        private static bool CheckSum(string nummer, string controleGetal)
        {
            return (97 - (Int32.Parse(nummer) % 97)).ToString("D2") == controleGetal;
        }
    }
}