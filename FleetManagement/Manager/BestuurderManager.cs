using FleetManagement.Interfaces;
using FleetManagement.ManagerExceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager {
    public class BestuurderManager : IBestuurderRepository {
        private IBestuurderRepository repo;
        public BestuurderManager(IBestuurderRepository repo) {
            this.repo = repo;
        }
        public bool BestaatBestuurder(int bestuurderid) {
            try {
                if (bestuurderid < 1) throw new BestuurderManagerException("Bestuurder id kan niet kleiner dan 0 zijn");
                if (!repo.BestaatBestuurder(bestuurderid)) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                throw new AdresManagerException("Adres - BestaatAdres - Foutief", ex);
            }
        }

        public bool BestaatRijksRegisterNummer(string rijksRegisterNr) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Bestuurder> GeefAlleBestuurder() {
            throw new NotImplementedException();
        }

        public Bestuurder GetBestuurderId(int id) {
            throw new NotImplementedException();
        }

        public void UpdateBestuurder(Bestuurder bestuurder) {
            throw new NotImplementedException();
        }

        public void VoegBestuurder(Bestuurder bestuurder) {
            throw new NotImplementedException();
        }

        public Bestuurder ZoekBestuurder(int? id, string voornaam, string achternaam, string geboorteDatum, Adres adres) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Bestuurder> ZoekBestuurders(int? id, string voornaam, string achternaam, string geboortedatum, Adres adres) {
            throw new NotImplementedException();
        }
    }
}
