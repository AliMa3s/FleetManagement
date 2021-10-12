using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Models
{
    public class Adres
    {
        public int AdresId { get; }
        public string Straat { get; set; }
        public string Nr { get; set; }
        public string Postcode { get; set; }
        public string Stad { get; set; }

        public Adres() { }

        public Adres(int adresId) 
        {
            AdresId = adresId;
        }
    }
}
