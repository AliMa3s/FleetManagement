using FleetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces
{
    //Implementatie Voertuig om de businessroles af te dwingen
    //Verander naam indien gewenst en vul aan met methods die de businessroles beschrijven
   public interface IVoertuig
    {
        public void GetVoertuigID(Voertuig voertuigID);
        public void GetChassisNummer(string chassisnummer);
        public void UpdateNummerplaat(string nummerplaat);
        public void SetAutoKleur(Kleur kleur);
        public void SetBrandStof(BrandstofType brandstof);
        public void Getinboekdatum(DateTime inboekdatum);
        public void SetAantalDeuren(int deurenaantal);
        public void GetAantalDeuren(int deurenaantal);
        public void GetBestuurder(Bestuurder bestuurder);
    }
}
