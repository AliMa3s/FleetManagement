using FleetManagement.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace FleetManagement.CheckFormats
{
    internal class SplitGeboorteDatum
    {
        public int Jaartal { get; }
        public string ControleDatum { get; }

        public SplitGeboorteDatum(string geboorteDatum)
        {
            if(geboorteDatum == null) throw new GeboorteDatumException("Geboortedatum mag niet null zijn");

            if(geboorteDatum.Length == 10)
            {
                string datumInDigits = geboorteDatum.Replace("-", "").Replace("/", "");

                if (Regex.IsMatch(datumInDigits, @"^[0-9]{8}$"))
                {
                    Jaartal = Int32.Parse(datumInDigits.Substring(0, 4));
                    ControleDatum = datumInDigits.Substring(2, 6);
                }
                else
                {
                    throw new GeboorteDatumException("Geboortedatum is niet het juiste format");
                }
            }
            else
            {
                throw new GeboorteDatumException("Geboortedatum is niet het juiste format");
            }
        }
    }
}
