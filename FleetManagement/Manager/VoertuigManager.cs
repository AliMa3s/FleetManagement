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

        public Voertuig VoegVoertuigToe(Voertuig voertuig)
        {
            try
            {
                if (voertuig == null) throw new VoertuigManagerException("Voertuig - Voertuig mag niet null zijn");

                if(voertuig.Brandstof.BrandstofTypeId < 1) throw new VoertuigManagerException("Voertuig - brandstof is niet geslecteerd uit een lijst");

                if (voertuig.AutoModel.AutoModelId > 0)
                {
                    if (BestaatChassisnummer(voertuig.ChassisNummer))
                        throw new VoertuigManagerException("Chassisnummer bestaat al");

                    if (BestaatNummerplaat(voertuig.NummerPlaat))
                        throw new VoertuigManagerException("Nummerplaat bestaat al");

                   return _repo.VoegVoertuigToe(voertuig);
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

                if (voertuig.Brandstof.BrandstofTypeId < 1) throw new VoertuigManagerException("Voertuig - brandstof is niet geslecteerd uit een lijst");

                if (voertuig.AutoModel.AutoModelId > 0)
                {
                    if (BestaatVoertuig(voertuig)) {
                    _repo.UpdateVoertuig(voertuig);
                    } else {
                    throw new VoertuigManagerException("Voertuig - bestaat niet!");
                    }
                }
                else
                {
                    throw new VoertuigManagerException("AutoModel is niet gelecteerd uit de lijst");
                }

            } catch (Exception ex) {

                throw new VoertuigManagerException(ex.Message);
            }
        }

        public void UpdateVoertuig(Voertuig voertuig, string anderChassisNummer, string anderNummerplaat)
        {
            try
            {
                if(voertuig == null) throw new VoertuigManagerException("Voertuig - Voertuig mag niet null zijn");
                if(!CheckFormat.IsChassisNummerGeldig(anderChassisNummer)) { }
                if(!CheckFormat.IsNummerplaatGeldig(anderNummerplaat)) { }

                if (voertuig.Brandstof.BrandstofTypeId < 1) 
                    throw new VoertuigManagerException("Voertuig - brandstof is niet geslecteerd uit een lijst");

                if (voertuig.AutoModel.AutoModelId > 0)
                {
                    if (BestaatVoertuig(voertuig))
                    {
                        if (_repo.BestaatChassisnummer(anderChassisNummer))
                            throw new VoertuigManagerException("Update: chassisnummer bestaat al");

                        if (_repo.BestaatNummerplaat(anderNummerplaat))
                            throw new VoertuigManagerException("Update: nummerplaat bestaat al");

                        _repo.UpdateVoertuig(voertuig, anderChassisNummer, anderNummerplaat);
                    }
                    else
                    {
                        throw new VoertuigManagerException("Voertuig - bestaat niet!");
                    }
                }
                else
                {
                    throw new VoertuigManagerException("AutoModel is niet gelecteerd uit de lijst");
                }

            }
            catch (Exception ex)
            {
                throw new VoertuigManagerException("Voertuig - UpdateVoertuig - gefaald", ex);
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
            try
            {
                if (voertuig == null) throw new VoertuigManagerException("Voertuig mag niet null zijn");

                return _repo.BestaatVoertuig(voertuig);
            }
            catch (Exception ex)
            {
                throw new VoertuigManagerException("Voertuig - BestaatVoertuig - gefaald", ex);
            }
        }

        public bool BestaatNummerplaat(string nummerPlaat) 
        {
            try
            {
                //gooit eigen exception
                if (!CheckFormat.IsNummerplaatGeldig(nummerPlaat)) { }

                return  _repo.BestaatNummerplaat(nummerPlaat);
            }
            catch (Exception ex)
            {
                throw new VoertuigManagerException("Voertuig - BestaatNummerplaat - gefaald", ex);
            }
        }

        public bool BestaatChassisnummer(string chassisNummer) 
        {
            try
            {
                //gooit eigen exception
                if (!CheckFormat.IsChassisNummerGeldig(chassisNummer)) { }

                return _repo.BestaatChassisnummer(chassisNummer);
            }
            catch (Exception ex)
            {
                throw new VoertuigManagerException("Voertuig - chassisNummer - gefaald", ex);
            }
        }

        public IReadOnlyList<Voertuig> GeefAlleVoertuigenFilter(string autonaam, Filter filter) 
        {
            try
            {
                //lege string is toegestaan anders kan er niet gefilterd worden
                if (autonaam == null) throw new VoertuigManagerException("Autonaam mag niet null zijn");
                if (filter == null) throw new VoertuigManagerException("Filter mag niet null zijn");

                return _repo.GeefAlleVoertuigenFilter(autonaam, filter);
            }
            catch (Exception ex)
            {
                throw new VoertuigManagerException("Voertuig - GeefAlleVoertuigenFilter - gefaald", ex);
            }
        }

        public Voertuig ZoekOpNummerplaatOfChassisNummer(string plaatnummerOfChassis) 
        {
            try
            {
                //lege string is toegestaan anders kan er niet gefilterd worden
                if (plaatnummerOfChassis == null) throw new VoertuigManagerException("Plaatnummer mag niet null zijn");

                return _repo.ZoekOpNummerplaatOfChassisNummer(plaatnummerOfChassis);
            }
            catch (Exception ex)
            {
                throw new VoertuigManagerException("Voertuig - ZoekOpNummerplaatOfChassisNummer - gefaald", ex);
            }
        }

        public IReadOnlyList<Voertuig> SelecteerZonderBestuurderFilter(string autonaam)
        {
            try
            {
                //lege string is toegestaan anders kan er niet gefilterd worden
                if (autonaam == null) throw new VoertuigManagerException("Plaatnummer mag niet null zijn");

                return _repo.SelecteerZonderBestuurderFilter(autonaam);
            }
            catch (Exception ex)
            {
                throw new VoertuigManagerException("Voertuig - SelecteerZonderBestuurderFilter - gefaald", ex);
            }
        }
    }
}
