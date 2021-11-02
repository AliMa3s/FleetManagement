using FleetManagement.Manager.Helpers;
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
                throw new BestuurderManagerException("Bestuurder - BestaatBestuurder - Foutief", ex);
            }
        }

        public bool BestaatRijksRegisterNummer(string rijksRegisterNr) {
            try {
                if (string.IsNullOrWhiteSpace(rijksRegisterNr)) throw new BestuurderManagerException("RijksRegiserNr mag niet leeg of On-nodige spaties hebben!");
                if (!repo.BestaatRijksRegisterNummer(rijksRegisterNr)) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                throw new BestuurderManagerException("RijksRegisterNr - BestaatRijksRegisterNummer - Foutief", ex);
            }
        }
 
        public IReadOnlyList<Bestuurder> GeefAlleBestuurder() {
            return repo.GeefAlleBestuurder();
        }

        public Bestuurder GetBestuurderId(int bestuuderId) {
            if (bestuuderId < 1) throw new BestuurderManagerException("Bestuurder id mag niet 0 of kleiner zijn.");
            return repo.GetBestuurderId(bestuuderId);
        }

        public void UpdateBestuurder(Bestuurder bestuurder) {
            try {
                if (bestuurder == null) throw new BestuurderManagerException("Bestuurder - Bestuurder mag niet null zijn");
                if (repo.BestaatBestuurder(bestuurder.BestuurderId)) {
                    repo.UpdateBestuurder(bestuurder);
                } else {
                    throw new BestuurderManagerException("Bestuurder - bestaat niet!");
                }
            } catch (Exception ex) {

                throw new BestuurderManagerException(ex.Message);
            }
        }

        public void VerwijderBestuurder(Bestuurder bestuurder) {
            try {
                if (bestuurder == null) throw new BestuurderManagerException("Bestuurder - Bestuurder mag niet null zijn");
                if (repo.BestaatBestuurder(bestuurder.BestuurderId)) {
                    repo.VerwijderBestuurder(bestuurder);
                } else {
                    throw new BestuurderManagerException("Bestuurder - Bestuurder bestaat niet!");
                }
            } catch (Exception ex) {

                throw new BestuurderManagerException(ex.Message);
            }
        }

        public void VoegBestuurderToe(Bestuurder bestuurder) {
            try {
                if (bestuurder == null) throw new BestuurderManagerException("Bestuurder - Bestuurder mag niet null zijn");
                if (!repo.BestaatBestuurder(bestuurder.BestuurderId)) {
                    repo.VoegBestuurderToe(bestuurder);
                } else {
                    throw new BestuurderManagerException("Bestuurder Bestaat al");
                }
            } catch (Exception ex) {

                throw new BestuurderManagerException(ex.Message);
            }
        }

        public Bestuurder ZoekBestuurder(int bestuurderid) {
            try {
                if (bestuurderid < 1) throw new BestuurderManagerException("Bestuurder id kan niet kleiner dan 0 zijn");
                if (repo.BestaatBestuurder(bestuurderid)) {
                    return repo.ZoekBestuurder(bestuurderid);
                } else {
                    throw new BestuurderManagerException("Bestuurder - Bestaat niet!");
                }
            } catch (Exception ex) {

                throw new BestuurderManagerException(ex.Message);
            }

        }

        public IReadOnlyList<Bestuurder> ZoekBestuurders(int? id, string voornaam, string achternaam, string geboortedatum, Adres adres) {
            throw new NotImplementedException();
        }


        //bezig idee uitwerking Filip
        public PaginaLijst<Bestuurder> AlleBestuurdersZonderVoertuig()
        {
            throw new NotImplementedException();
        }

        public PaginaLijst<Bestuurder> FilterOpBestuurdersNaam(string voornaam, string achternaam)
        {
            throw new NotImplementedException();
        }

        public Bestuurder ZoekBestuurder(string RijksRegisterNummer)
        {
            throw new NotImplementedException();
        }

        public PaginaLijst<Bestuurder> AlleBestuurders(SorteerOptie sorteer)
        {
            throw new NotImplementedException();
        }
    }
}
