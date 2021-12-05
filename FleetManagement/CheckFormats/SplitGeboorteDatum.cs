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
            if(geboorteDatum == null) throw new GeboorteDatumException("GeboorteDatum mag niet null zijn");

            string datumInDigits = geboorteDatum.Replace("-", "").Replace("/", "");

            if (Regex.IsMatch(datumInDigits, @"^[0-9]{4}$"))
            {
                Jaartal = Int32.Parse(datumInDigits);
                ControleDatum = datumInDigits.Substring(2, 2) + "0000";
            }
            else if(Regex.IsMatch(datumInDigits, @"^[0-9]{6}$"))
            {
                Jaartal = Int32.Parse(datumInDigits.Substring(0, 4));
                ControleDatum = datumInDigits.Substring(2, 4) + "00";
            }
            else if (Regex.IsMatch(datumInDigits, @"^[0-9]{8}$"))
            {
                Jaartal = Int32.Parse(datumInDigits.Substring(0, 4));
                ControleDatum = datumInDigits.Substring(2, 6);
            }
            else
            {
                throw new GeboorteDatumException("GeboorteDatum is niet het juiste formaat");
            }
        }
    }
}
