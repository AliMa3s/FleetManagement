using FleetManagement.CheckFormats;
using FleetManagement.Interfaces;
using FleetManagement.ManagerExceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleetManagement.Manager {
    public class BestuurderManager : IBestuurderRepository
    {
        private readonly IBestuurderRepository _repo;
        public BestuurderManager(IBestuurderRepository repo) {
            this._repo = repo;
        }

        public bool BestaatBestuurder(int bestuurderid) {
            try {
                if (bestuurderid < 1) throw new BestuurderManagerException("Bestuurder id kan niet kleiner dan 0 zijn");
                if (!_repo.BestaatBestuurder(bestuurderid)) {
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

                //Controleer rijksregisternummer op aantal digits
                if (!Regex.IsMatch(rijksRegisterNr.ToUpper(), @"^[0-9]{11}$"))
                {
                    if (!_repo.BestaatRijksRegisterNummer(rijksRegisterNr)) {
                        return false;
                    } else {
                        return true;
                    }    
                }
                else
                {
                    throw new BestuurderManagerException("Rijksregisternummer is niet juist");
                }

            } catch (Exception ex) {
                throw new BestuurderManagerException("RijksRegisterNr - BestaatRijksRegisterNummer - Foutief", ex);
            }
        }
 
        //public IReadOnlyList<Bestuurder> GeefAlleBestuurder() {
        //    return _repo.GeefAlleBestuurder();
        //}

        //public Bestuurder GetBestuurderId(int bestuuderId) {
        //    if (bestuuderId < 1) throw new BestuurderManagerException("Bestuurder id mag niet 0 of kleiner zijn.");
        //    return _repo.GetBestuurderId(bestuuderId);
        //}

        public void UpdateBestuurder(Bestuurder bestuurder) {
            try {
                if (bestuurder == null) throw new BestuurderManagerException("Bestuurder - Bestuurder mag niet null zijn");
                if (_repo.BestaatBestuurder(bestuurder.BestuurderId)) {
                    _repo.UpdateBestuurder(bestuurder);
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
                if (_repo.BestaatBestuurder(bestuurder.BestuurderId)) {
                    _repo.VerwijderBestuurder(bestuurder);
                } else {
                    throw new BestuurderManagerException("Bestuurder - Bestuurder bestaat niet!");
                }
            } catch (Exception ex) {

                throw new BestuurderManagerException(ex.Message);
            }
        }

        public Bestuurder VoegBestuurderToe(Bestuurder bestuurder) {
            try {
                if (bestuurder == null) throw new BestuurderManagerException("Bestuurder - Bestuurder mag niet null zijn");

                if (!_repo.BestaatRijksRegisterNummer(bestuurder.RijksRegisterNummer)) {
                    return _repo.VoegBestuurderToe(bestuurder);
                } else {
                    throw new BestuurderManagerException("Rijksregisternummer bestaat al");
                }
            } catch (Exception ex) {

                throw new BestuurderManagerException(ex.Message);
            }
        }
        
        //public Bestuurder ZoekBestuurder(int bestuurderid) {
        //    try {
        //        if (bestuurderid < 1) throw new BestuurderManagerException("Bestuurder id kan niet kleiner dan 0 zijn");
        //        if (_repo.BestaatBestuurder(bestuurderid)) {
        //            return _repo.ZoekBestuurder(bestuurderid);
        //        } else {
        //            throw new BestuurderManagerException("Bestuurder - Bestaat niet!");
        //        }
        //    } catch (Exception ex) {

        //        throw new BestuurderManagerException(ex.Message);
        //    }

        //}

        //public IReadOnlyList<Bestuurder> ZoekBestuurders(int? id, string voornaam, string achternaam, string geboortedatum, Adres adres) {
        //    throw new NotImplementedException();
        //}

        public IReadOnlyList<Bestuurder> SelecteerOpBestuurdersNaam(string achterNaamEnVoornaam)
        {
            try
            {
                if (achterNaamEnVoornaam == null)
                {
                    throw new BestuurderManagerException("filteren op naam mag niet null zijn");
                }

                return _repo.SelecteerOpBestuurdersNaam(achterNaamEnVoornaam);
            }
            catch (Exception ex)
            {
                throw new BestuurderManagerException(ex.Message);
            }
        }

        public IReadOnlyList<Bestuurder> FilterOpBestuurdersNaam(string achterNaamEnVoornaam)
        {
            try
            {
                if (achterNaamEnVoornaam == null)
                {
                    throw new BestuurderManagerException("filteren op naam mag niet null zijn");
                }

                return _repo.FilterOpBestuurdersNaam(achterNaamEnVoornaam);
            }
            catch (Exception ex)
            {
                throw new BestuurderManagerException(ex.Message);
            }
        }

        public Bestuurder ZoekBestuurder(string rijksRegisterNummer)
        {
            //Controleer rijksregisternummer op aantal digits
            if (Regex.IsMatch(rijksRegisterNummer.ToUpper(), @"^[0-9]{11}$"))
            {
                return _repo.ZoekBestuurder(rijksRegisterNummer);
            }
            else
            {
                throw new BestuurderManagerException("Rijksregisternummer is niet juist");
            }
        }
    }
}
