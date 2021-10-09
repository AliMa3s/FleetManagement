using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces
{
    //Implementatie Bestuurder om de businessroles af te dwingen
    //Verander naam indien gewenst en vul aan met methods die de businessroles beschrijven
    interface IBestuurder
    {
        public void VoertuigToevoegen();  //moet worden: (Voertuig voertuig)
        public void VoertuigVerwijderen();  //moet worden: (Voertuig voertuig)
        public bool IsRijksRegisterGeldig(string rijksregister, DateTime geboorteDatum);
        public bool IsRijbewijsGeldig(string rijbewijsType);
        public Array StatusBestuurders { get; }
    }
}
