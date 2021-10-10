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
        int VoertuigId { get;}
        string ChassisNummer { get; private set; }
        string NummerPlaat { get; private set; }
        Kleur kleur { get; set; }
        BrandstofType Brandstof { get; set; }
        DateTime InBoekDatum { get; set; }
        int AantalDeuren { get; set; }

        public void UpdateNummerPlaat(string nummerPlaat);
        public void GetVoertuigID(Voertuig voertuigID);
        public void GetChassisNummer(string chassisnummer);
        public void GetBestuurder(Bestuurder bestuurder);

    }
}
