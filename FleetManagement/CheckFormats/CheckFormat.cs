using FleetManagement.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace FleetManagement.CheckFormats
{
    public static class CheckFormat
    {
        public static bool IsPincodeGeldig(string pincode)
        {
            if(pincode == null) throw new PincodeException("Pincode mag niet null zijn");

            return Regex.IsMatch(pincode, @"^[0-9]{4,5}$")
                ? true : throw new PincodeException("Pincode moet een string zijn van 4 of 5 cijfers");
        }

        public static bool IsChassisNummerGeldig(string chassisNummer)
        {
            if (chassisNummer == null) throw new ChassisNummerException("Chassisnummer mag niet null zijn");

            return Regex.IsMatch(chassisNummer.ToUpper(), @"^[0-9A-HJ-NPR-Z]{17}$")
                ? true : throw new ChassisNummerException("Chassisnummer moet string zijn van 17 cijfers/letters maar " +
                $"letter I/i, O/o en Q/q mag niet voorkomen");
        }

        public static bool IsNummerplaatGeldig(string nummerPlaat)
        {
            if (nummerPlaat == null) throw new NummerPlaatException("Nummerplaat mag niet null zijn");

            return Regex.IsMatch(nummerPlaat.ToUpper(), @"^[1-9A-Z]{1}[A-Z]{3}[0-9]{3}$")
                ? true : throw new NummerPlaatException("Nummerplaat moet beginnen met 1 cijfer/letter gevolgd door 3 letters en dan 3 cijfers");
        }

        public static bool IsRijksRegisterGeldig(string rijksRegisterNummer)
        {
            if (rijksRegisterNummer == null) throw new RijksRegisterNummerException("Rijksregisternummer bestaat uit 11 digits");

            return Regex.IsMatch(rijksRegisterNummer, @"^[0-9]{11}$");
        }

        public static bool IsRijksRegisterGeldig(string rijksRegisterNummer, string ingegevenGeboorteDatum)
        {
            //Controleer eerst het format
            if (IsRijksRegisterGeldig(rijksRegisterNummer))
            {
                SplitRijksRegister rijksRegister = new(rijksRegisterNummer);
                SplitGeboorteDatum geboortedatum = new(ingegevenGeboorteDatum);

                //Controleer of Bestuurder is geboren in of na 2000 (+ iendien geboortedatum niet is gekend (0000))
                if (geboortedatum.Jaartal == 0 || geboortedatum.Jaartal >= 2000)
                {
                    rijksRegister.CheckGetal += 2000000000;
                }

                //Controleer de inhoud van het format
                if (rijksRegister.ControleDatum == geboortedatum.ControleDatum
                    && IsNummerBinnenBereik(rijksRegister.Dag, 0, 31)
                    && (IsNummerBinnenBereik(rijksRegister.Maand, 0, 12) 
                        || IsNummerBinnenBereik(rijksRegister.Maand, 20, 32) || IsNummerBinnenBereik(rijksRegister.Maand, 40, 52))
                    && IsNummerBinnenBereik(rijksRegister.Geslacht, 1, 999)
                    && CheckSum(rijksRegister.CheckGetal, rijksRegister.ControleSom))
                {
                    return true;
                }

                throw new RijksRegisterNummerException("Rijksregisternummer is niet het juiste format");
            }
            else
            {
                throw new RijksRegisterNummerException("Rijksregisternummer is niet het juiste format");
            }
        }

        public static bool IsTankKaartNummerGeldig(string tankKaartNummer)
        {
            if (tankKaartNummer == null) throw new TankKaartException("Tankkaartnummer mag niet null zijn");

            return Regex.IsMatch(tankKaartNummer, @"^[0-9]{16,20}$")
                ? true : throw new TankKaartException($"Tankkaartnummer is niet het juiste format");
        }

        private static bool IsNummerBinnenBereik(string nummer, int min, int max)
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
