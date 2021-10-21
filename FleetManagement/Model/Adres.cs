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

        

        public Adres(int adresId) 
        {
            AdresId = adresId;
        }


        public Adres(string straat, string nr, string postcode, string stad)
        {
            Straat = straat;
            Nr = nr;
            Postcode = postcode;
            Stad = stad;
        }

        public override string ToString()
        {
            return $"[STRAAT]:{Straat}" +
                $"[NUMMER]:{Nr}" +
                $"[POSTCODE]:{Postcode}" +
                $"[STAD]:{Stad}";

        }
    }
}
