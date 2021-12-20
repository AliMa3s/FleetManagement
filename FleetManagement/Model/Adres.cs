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

            Straat = straat ?? throw new AdresException("Straat mag niet null zijn");
            Nr = nr ?? throw new AdresException("Nummer mag niet null zijn");
            Postcode = postcode ?? throw new AdresException("Postcode mag niet null zijn");
            Gemeente = gemeente ?? throw new AdresException("Gemeente mag niet null zijn");
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
