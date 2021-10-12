using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces
{
    
   public interface IVoertuig
    {
        int VoertuigId { get;}
        string ChassisNummer { get; private set; }
        string NummerPlaat { get; private set; }
        Kleur kleur { get; set; }
        BrandstofType Brandstof { get; set; }
        DateTime InBoekDatum { get; set; }
        int AantalDeuren { get; set; }

        public void GetVoertuigID(Voertuig voertuigID);
        public void GetChassisNummer(string chassisnummer);
        public void UpdateNummerplaat(string nummerplaat);
        public void SetAutoKleur(Kleur kleur);
        public void SetBrandStof(BrandStofType brandstof);
        public void Getinboekdatum(DateTime inboekdatum);
        public void SetAantalDeuren(int deurenaantal);
        public void GetAantalDeuren(int deurenaantal);
        public void GetBestuurder(Bestuurder bestuurder);
    }
}
