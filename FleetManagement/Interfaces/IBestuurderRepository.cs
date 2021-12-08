using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces {
    public interface IBestuurderRepository {

        bool BestaatBestuurder(int bestuurderid);
        bool BestaatRijksRegisterNummer(string rijksRegisterNr);
        void UpdateBestuurder(Bestuurder bestuurder);
        void UpdateBestuurder(Bestuurder bestuurder, string anderRijksregisterNummer);
        void VerwijderBestuurder(Bestuurder bestuurder);
        Bestuurder VoegBestuurderToe(Bestuurder bestuurder);
        IReadOnlyList<Bestuurder> SelecteerBestuurdersZonderVoertuig(string achterNaamEnVoornaam);
        IReadOnlyList<Bestuurder> SelecteerBestuurdersZondertankkaart(string achterNaamEnVoornaam);
        IReadOnlyList<Bestuurder> FilterOpBestuurdersNaam(string achterNaamEnVoornaam);
        Bestuurder ZoekBestuurder(string rijksRegisterNummer);
    }
}
