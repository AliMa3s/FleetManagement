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
        Bestuurder ZoekBestuurder(int bestuurderid);
        bool BestaatBestuurder(int id);
        Bestuurder GetBestuurderId(int id);
        void VoegBestuurderToe(Bestuurder bestuurder);
        void UpdateBestuurder(Bestuurder bestuurder);
        void VerwijderBestuurder(Bestuurder bestuurder);
        bool BestaatRijksRegisterNummer(string rijksRegisterNr);

        //bezig idee uitwerking Filip
        Bestuurder ZoekBestuurder(string RijksRegisterNummer);

        IReadOnlyList<Bestuurder> FilterOpBestuurdersNaam(string achterNaamEnVoornaam, bool bestuurdersZonderVoertuig);
    }
}
