using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces {
    public interface IBestuurderRepository {

        public bool BestaatBestuurder(int bestuurderid);
        public bool BestaatRijksRegisterNummer(string rijksRegisterNr);
        public void UpdateBestuurder(Bestuurder bestuurder);
        public void VerwijderBestuurder(Bestuurder bestuurder);
        public Bestuurder VoegBestuurderToe(Bestuurder bestuurder);
        public IReadOnlyList<Bestuurder> SelecteerBestuurdersZonderVoertuig(string achterNaamEnVoornaam);
        public IReadOnlyList<Bestuurder> SelecteerBestuurdersZondertankkaart(string achterNaamEnVoornaam);
        public IReadOnlyList<Bestuurder> FilterOpBestuurdersNaam(string achterNaamEnVoornaam);
        public Bestuurder ZoekBestuurder(string rijksRegisterNummer);
    }
}
