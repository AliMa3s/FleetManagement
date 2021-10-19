using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleetManagement.Helpers
{
    internal class BouwGeboorteDatum
    {
        public int Jaartal { get; }
        public string ControleDatum { get; }

        public BouwGeboorteDatum(string geboorteDatum)
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
                throw new TankKaartException(" GeboorteDatum kan alleen bestaan uit cijfers: 'jaartal', 'jaartal-maand-dag' " +
                    "of 'jaartal/maand/dag'");
            }
        }
    }
}
