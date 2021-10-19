using FleetManagement.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace FleetManagement.Helpers
{
    internal class SplitGeboorteDatum
    {
        public int Jaartal { get; }
        public string ControleDatum { get; }

        public SplitGeboorteDatum(string geboorteDatum)
        {
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
                throw new RijksRegisterNummerException(" GeboorteDatum kan alleen bestaan uit cijfers: 'jaartal', 'jaartal-maand-dag' " +
                    "of 'jaartal/maand/dag'");
            }
        }
    }
}
