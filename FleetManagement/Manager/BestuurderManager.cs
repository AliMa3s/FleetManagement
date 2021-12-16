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
                throw new BestuurderManagerException(ex.Message);
            }
        }

        public bool BestaatRijksRegisterNummer(string rijksRegisterNr) {
            try {

                //Controleer rijksregisternummer en gooit eigen exception 
                if (!CheckFormat.IsRijksRegisterGeldig(rijksRegisterNr)) { }

                if (!_repo.BestaatRijksRegisterNummer(rijksRegisterNr)) {
                    return false;
                }
                else {
                    return true;
                }

            } catch (Exception ex) {
                throw new BestuurderManagerException(ex.Message);
            }
        }

        public Bestuurder UpdateBestuurder(Bestuurder bestuurder) {
            try {
                if (bestuurder == null) throw new BestuurderManagerException("Bestuurder - Bestuurder mag niet null zijn");

                if (BestaatBestuurder(bestuurder.BestuurderId)) {
                    return _repo.UpdateBestuurder(bestuurder);
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

                StringBuilder mess = new();

                if(bestuurder.HeeftBestuurderTankKaart) mess.Append("Kan bestuurder met tankkaart niet verwijderen");
                if(bestuurder.HeeftBestuurderVoertuig) mess.AppendLine(Environment.NewLine + "Kan bestuurder met voertuig niet verwijderen");

                if (!string.IsNullOrWhiteSpace(mess.ToString()))
                {
                    throw new BestuurderManagerException(mess.ToString());
                }

                if (BestaatBestuurder(bestuurder.BestuurderId)) {
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

                if (!BestaatRijksRegisterNummer(bestuurder.RijksRegisterNummer)) {
                    return _repo.VoegBestuurderToe(bestuurder);
                } else {
                    throw new BestuurderManagerException("Rijksregisternummer bestaat al");
                }
            } catch (Exception ex) {

                throw new BestuurderManagerException(ex.Message);
            }
        }
        
        public IReadOnlyList<Bestuurder> SelecteerBestuurdersZonderVoertuig(string achterNaamEnVoornaam)
        {
            try
            {
                if (achterNaamEnVoornaam == null)
                {
                    throw new BestuurderManagerException("filteren op naam mag niet null zijn");
                }

                return _repo.SelecteerBestuurdersZonderVoertuig(achterNaamEnVoornaam);
            }
            catch (Exception ex)
            {
                throw new BestuurderManagerException(ex.Message);
            }
        }

        public IReadOnlyList<Bestuurder> SelecteerBestuurdersZondertankkaart(string achterNaamEnVoornaam)
        {
            try
            {
                if (achterNaamEnVoornaam == null)
                {
                    throw new BestuurderManagerException("filteren op naam mag niet null zijn");
                }

                return _repo.SelecteerBestuurdersZondertankkaart(achterNaamEnVoornaam);
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
            try
            {
                if (!CheckFormat.IsRijksRegisterGeldig(rijksRegisterNummer)) { }

                return _repo.ZoekBestuurder(rijksRegisterNummer);
            }
            catch (Exception ex)
            {
                throw new BestuurderManagerException(ex.Message);
            }
        }

        public Bestuurder UpdateBestuurder(Bestuurder bestuurder, string anderRijksregisterNummer)
        {
            try
            {
                if (bestuurder == null) throw new BestuurderManagerException("Bestuurder - bestuurder mag niet null zijn");

                if (!CheckFormat.IsRijksRegisterGeldig(anderRijksregisterNummer, bestuurder.GeboorteDatum)) { }

                if (BestaatBestuurder(bestuurder.BestuurderId))
                {
                    if (BestaatRijksRegisterNummer(anderRijksregisterNummer))
                    {
                        throw new BestuurderManagerException("Update: Rijksregisternummer bestaat al");
                    }

                    return _repo.UpdateBestuurder(bestuurder, anderRijksregisterNummer);
                }
                else
                {
                    throw new BestuurderManagerException("Bestuurder - bestaat niet!");
                }
            }
            catch (Exception ex)
            {
                throw new BestuurderManagerException(ex.Message);
            }
        }

        public bool HeeftBestuurderAdres(Bestuurder bestuurder)
        {
            try
            {
                if (bestuurder == null) throw new BestuurderManagerException("Bestuurder - Bestuurder mag niet null zijn");

                return _repo.HeeftBestuurderAdres(bestuurder);
            }
            catch (Exception ex)
            {
                throw new BestuurderManagerException(ex.Message);
            }            
        }

        public void VerwijderBestuurderAdres(Bestuurder bestuurder)
        {
            try
            {
                if (bestuurder == null) throw new BestuurderManagerException("Bestuurder - Bestuurder mag niet null zijn");

                if(HeeftBestuurderAdres(bestuurder))
                {
                    _repo.VerwijderBestuurderAdres(bestuurder);
                }
                else
                {
                    throw new BestuurderManagerException("BestuurderAdres - bestaat niet");
                }
            }
            catch (Exception ex)
            {
                throw new BestuurderManagerException(ex.Message);
            }
        }
    }
}
