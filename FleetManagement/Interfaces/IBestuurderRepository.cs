using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces {
    public interface IBestuurderRepository {
        IReadOnlyList<Bestuurder> GeefAlleBestuurder();
        IReadOnlyList<Bestuurder> ZoekBestuurders(int? id, string voornaam, string achternaam, string geboortedatum, Adres adres);
        Bestuurder ZoekBestuurder(int? id, string voornaam, string achternaam, string geboorteDatum, Adres adres);
        bool BestaatBestuurder(int id);
        Bestuurder GetBestuurderId(int id);
        void VoegBestuurder(Bestuurder bestuurder);
        void UpdateBestuurder(Bestuurder bestuurder);
        bool BestaatRijksRegisterNummer(string rijksRegisterNr);

    }
}
