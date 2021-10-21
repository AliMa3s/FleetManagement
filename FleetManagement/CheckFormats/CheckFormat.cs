using FleetManagement.Exceptions;
using FleetManagement.Helpers;
using System;
using System.Text.RegularExpressions;

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
                ? true : throw new ChassisNummerException($"chassisnummer moet string zijn van 17 cijfers/letters maar letter I/i, O/o en Q/q mag niet voorkomen");
        }

        public static bool IsNummerplaatGeldig(string nummerPlaat)
        {
            return Regex.IsMatch(nummerPlaat.ToUpper(), @"^[1-9]{1}[A-Z]{3}[0-9]{3}$")
                ? true : throw new NummerPlaatException($"{nameof(nummerPlaat)} moet format [1-9][a-z][0-9] zijn");
        }

        public static bool IsRijksRegisterGeldig(string rijksRegisterNummer, string ingegevenGeboorteDatum)
        {
            //Controleer eerst het format
            if (Regex.IsMatch(rijksRegisterNummer.ToUpper(), @"^[0-9]{11}$"))
            {
                SplitRijksRegister rijksRegister = new(rijksRegisterNummer);
                SplitGeboorteDatum geboortedatum = new(ingegevenGeboorteDatum);

                //Controleer of Bestuurder is geboren in of na 2000
                if (geboortedatum.Jaartal >= 2000)
                {
                    rijksRegister.CheckGetal += 2000000000;
                }

                //Controleer de inhoud van het format
                if (rijksRegister.ControleDatum == geboortedatum.ControleDatum
                    && IsBinnenBereik(rijksRegister.Dag, 0, 31)
                    && (IsBinnenBereik(rijksRegister.Maand, 0, 12) 
                        || IsBinnenBereik(rijksRegister.Maand, 20, 32) || IsBinnenBereik(rijksRegister.Maand, 40, 52))
                    && IsBinnenBereik(rijksRegister.Geslacht, 1, 998)
                    && CheckSum(rijksRegister.CheckGetal, rijksRegister.ControleSom))
                {
                    return true;
                }

                throw new RijksRegisterNummerException($"De inhoud van {nameof(rijksRegisterNummer)} bevat fouten");
            }
            else
            {
                throw new RijksRegisterNummerException($" {nameof(rijksRegisterNummer)} is niet het juiste format");
            }
        }

        public static bool IsTankKaartNummerGeldig(string tankKaartNummer)
        {
            return Regex.IsMatch(tankKaartNummer.ToUpper(), @"^[0-9]{19}$")
                ? true : throw new TankKaartException($" {nameof(tankKaartNummer)} is niet het juiste format");
        }

        private static bool IsBinnenBereik(string nummer, int min, int max)
        {
            int getal = Int32.Parse(nummer);
            return getal >= min && getal <= max;
        }

        private static bool CheckSum(long nummer, string controleGetal)
        {
            return (97 - (nummer % 97)).ToString("D2") == controleGetal;
        }
    }
}