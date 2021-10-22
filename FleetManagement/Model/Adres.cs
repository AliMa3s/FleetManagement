using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model
{
    public class Adres
    {
        public int AdresId { get; }
        public string Straat { get; set; }
        public string Nr { get; set; }
        public string Postcode { get; set; }
        public string Stad { get; set; }

        public Adres(string straat, string nr, string postcode, string stad)
        {
            Straat = straat;
            Nr = nr;
            Postcode = postcode;
            Stad = stad;
        }

        public Adres(int adresId, string straat, string nr, string postcode, string stad)
            : this(straat, nr, postcode, stad)
        {
            if (adresId > 0)
            {
                AdresId = adresId;
            }
            else
            {
                throw new AdresException($"{nameof(AdresId)} moet meer zijn dan 0");
            }
        }

        public override string ToString()
        { 
            StringBuilder build = new($"{Straat} {Nr}");
            build.Append("{Postcode} {Stad}");
            return build.ToString();
        }
    }
}
