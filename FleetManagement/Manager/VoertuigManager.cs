using FleetManagement.CheckFormats;
using FleetManagement.Interfaces;
using FleetManagement.Manager.Interfaces;
using FleetManagement.ManagerExceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager {
    public class VoertuigManager : IVoertuigManager
    {
        private readonly IVoertuigRepository _repo;

        public IEnumerable<AantalDeuren> AantalDeuren => Enum.GetValues(typeof(AantalDeuren)).Cast<AantalDeuren>();

        public IEnumerable<AutoType> AutoTypes => Enum.GetValues(typeof(AutoType)).Cast<AutoType>();

        public VoertuigManager(IVoertuigRepository repo) {
            this._repo = repo;
        }

        public bool BestaatVoertuig(Voertuig voertuig) {
            try {

                if (voertuig == null) throw new VoertuigManagerException("Voertuig mag niet null zijn");
                if (!_repo.BestaatVoertuig(voertuig)) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                throw new VoertuigManagerException("Voertuig - BestaatVoertuig - Foutief", ex);
            }
        }

        public bool BestaatVoertuig(Voertuig voertuig, string chasisnummer, string nummerplaat) {
            try {

                if (voertuig == null) throw new VoertuigManagerException("Voertuig mag niet null zijn");
                if (!_repo.BestaatVoertuig(voertuig) && CheckFormats.CheckFormat.IsChassisNummerGeldig(chasisnummer)
                    && CheckFormats.CheckFormat.IsNummerplaatGeldig(nummerplaat)) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                throw new VoertuigManagerException("Voertuig - BestaatVoertuig - Foutief", ex);
            }
        }

        //Versie toegevoegd filip (hetzelfde als hierboven met mijn kijk op de zaak)
        public bool bestaatChassisOfNummerplaat(string chassisnummer, string nummerplaat)
        {
            try
            {
                if(CheckFormat.IsChassisNummerGeldig(chassisnummer) 
                    && CheckFormat.IsNummerplaatGeldig(nummerplaat))
                {
                    if (_repo.bestaatChassisOfNummerplaat(chassisnummer, nummerplaat))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new VoertuigManagerException("Voertuig - BestaatVoertuig - Foutief", ex);
            }
        }

        public IReadOnlyList<Voertuig> GeefAlleVoerTuig() {
            return _repo.GeefAlleVoerTuig();
        }

        public Voertuig GetVoertuig(int voertuigId) {
            if (voertuigId < 1) throw new VoertuigManagerException("VoertuigId mag niet null zijn");
            return _repo.GetVoertuig(voertuigId);
        }

        public void UpdateVoertuig(Voertuig voertuig) {
            try {
                if (voertuig == null) throw new VoertuigManagerException("Voertuig - Voertuig mag niet null zijn");
                if (_repo.BestaatVoertuig(voertuig)) {
                    _repo.UpdateVoertuig(voertuig);
                } else {
                    throw new VoertuigManagerException("Voertuig - bestaat niet!");
                }
            } catch (Exception ex) {

                throw new VoertuigManagerException(ex.Message);
            }
        }

        public void VerwijderVoertuig(Voertuig voertuig) {
            try {
                if (voertuig == null) throw new VoertuigManagerException("Voertuig - Voertuig mag niet null zijn");
                if (_repo.BestaatVoertuig(voertuig)) {
                    _repo.VerwijderVoertuig(voertuig);
                } else {
                    throw new VoertuigManagerException("Voertuig - Voertuig bestaat niet!");
                }
            } catch (Exception ex) {

                throw new VoertuigManagerException(ex.Message);
            }
        }

        public void VoegVoertuigToe(Voertuig voertuig) {
            try {
                if (voertuig == null) throw new VoertuigManagerException("Voertuig - Voertuig mag niet null zijn");
                if (!_repo.BestaatVoertuig(voertuig)) {
                    _repo.VoegVoertuigToe(voertuig);
                } else {
                    throw new VoertuigManagerException("Voertuig Bestaat al");
                }
            } catch (Exception ex) {

                throw new VoertuigManagerException(ex.Message);
            }
        }

        public Voertuig ZoekVoertuig(int? voertuigId, AutoModel automodel, string chassisNumber, string nummerPlaat, BrandstofType brandstof, Kleur kleur, AantalDeuren aantalDeuren, Bestuurder bestuurder) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Voertuig> ZoekVoertuigen(int? voertuigId, AutoModel automodel, string chassisNumber, string nummerPlaat, BrandstofType brandstof, Kleur kleur, AantalDeuren aantalDeuren, Bestuurder bestuurder) {
            throw new NotImplementedException();
        }
    }
}
