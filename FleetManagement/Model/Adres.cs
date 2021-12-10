using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model {
    public class Adres {
        public int AdresId { get; private set; }
        public string Straat { get; set; }
        public string Nr { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }

        public Adres(string straat, string nr, string postcode, string gemeente) {

#warning exception is nog maar pas gegooid en is nog NIET getest. Opgelet! Leeg is toegestaan! Het is dit ofwel SQL veranderen
            if(straat == null) throw new AdresException("Straat mag niet null zijn");
            if (nr == null) throw new AdresException("Nummer mag niet null zijn");
            if (postcode == null) throw new AdresException("Postcode mag niet null zijn");
            if (gemeente == null) throw new AdresException("Gemeente mag niet null zijn");

            Straat = straat;
            Nr = nr;
            Postcode = postcode;
            Gemeente = gemeente;
        }

       public void VoegIdToe(int adresid)
        {
            if(adresid < 1) throw new AdresException($"{nameof(AdresId)} moet meer zijn dan 0");

            AdresId = adresid;
        }

        public override string ToString() {
            StringBuilder build = new($"{Straat} {Nr}");
            build.Append($"{Environment.NewLine} {Postcode} {Gemeente}");
            return build.ToString();
        }
    }
}
