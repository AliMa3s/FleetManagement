using FleetManagement.CheckFormats;
using FleetManagement.Filters;
using FleetManagement.Interfaces;
using FleetManagement.ManagerExceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager {
    public class VoertuigManager : IVoertuigRepository
    {
        private readonly IVoertuigRepository _repo;

        public static IEnumerable<AantalDeuren> AantalDeuren => Enum.GetValues(typeof(AantalDeuren)).Cast<AantalDeuren>();

        public static IEnumerable<AutoType> AutoTypes => Enum.GetValues(typeof(AutoType)).Cast<AutoType>();

        public VoertuigManager(IVoertuigRepository repo) {
            this._repo = repo;
        }

        public void VoegVoertuigToe(Voertuig voertuig)
        {
            try
            {
                if (voertuig == null) throw new VoertuigManagerException("Voertuig - Voertuig mag niet null zijn");

                if(voertuig.Brandstof.BrandstofTypeId < 1) throw new VoertuigManagerException("Voertuig - brandstof is niet geslecteerd uit een lijst");

                if (voertuig.AutoModel != null && voertuig.AutoModel.AutoModelId > 0)
                {
                    if (_repo.BestaatChassisnummer(voertuig.ChassisNummer))
                        throw new VoertuigManagerException("Chassisnummer bestaat al");

                    if (_repo.BestaatNummerplaat(voertuig.NummerPlaat))
                        throw new VoertuigManagerException("Nummerplaat bestaat al");

                    _repo.VoegVoertuigToe(voertuig);
                }
                else
                {
                    throw new VoertuigManagerException("AutoModel is niet gelecteerd uit de lijst");
                }

            }
            catch (Exception ex)
            {

                throw new VoertuigManagerException(ex.Message);
            }
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
                if (BestaatVoertuig(voertuig)) {
                    _repo.VerwijderVoertuig(voertuig);
                } else {
                    throw new VoertuigManagerException("Voertuig - Voertuig bestaat niet!");
                }
            } catch (Exception ex) {

                throw new VoertuigManagerException(ex.Message);
            }
        }

        public bool BestaatVoertuig(Voertuig voertuig)
        {
            if (voertuig == null) throw new VoertuigManagerException("Voertuig mag niet null zijn");

            return _repo.BestaatVoertuig(voertuig);
        }

        public bool BestaatNummerplaat(string nummerPlaat) 
        {
            //gooit eigen exception
            if (!CheckFormat.IsNummerplaatGeldig(nummerPlaat)) { }

            return  _repo.BestaatNummerplaat(nummerPlaat);
        }

        public bool BestaatChassisnummer(string chassisNummer) 
        {
            //gooit eigen exception
            if (!CheckFormat.IsChassisNummerGeldig(chassisNummer)) { }

            return _repo.BestaatChassisnummer(chassisNummer);
        }

        public IReadOnlyList<Voertuig> GeefAlleVoertuigenFilter(string autonaam) 
        { 
            if(autonaam == null) throw new VoertuigManagerException("Autonaam mag niet null zijn");

            return _repo.GeefAlleVoertuigenFilter(autonaam);
        }

        public IReadOnlyList<Voertuig> GeefAlleVoertuigenFilter(string autonaam, Filter filter) 
        {
            if (autonaam == null) throw new VoertuigManagerException("Autonaam mag niet null zijn");
            if (filter == null) throw new VoertuigManagerException("Filter mag niet null zijn");

            return _repo.GeefAlleVoertuigenFilter(autonaam, filter);
        }

        public Voertuig ZoekOpNummerplaat(string plaatnummer) 
        {
            if (plaatnummer == null) throw new VoertuigManagerException("EPlaatnummer mag niet null zijn");

            return _repo.ZoekOpNummerplaat(plaatnummer);
        }

        public Voertuig ZoekOpChassisNummer(string chassisnummer) 
        {
            if (chassisnummer == null) throw new VoertuigManagerException("Chassisnummer mag niet null zijn");

            return _repo.ZoekOpChassisNummer(chassisnummer);
        }
    }

}
