using FleetManagement.Interfaces;
using FleetManagement.ManagerExceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager {
    public class VoertuigManager : IVoertuigRepository {

        private IVoertuigRepository repo;
        public VoertuigManager(IVoertuigRepository repo) {
            this.repo = repo;
        }

        public bool BestaatVoertuig(Voertuig voertuig) {
            try {
                if (voertuig == null) throw new VoertuigManagerException("Voertuig mag niet null zijn");
                if (!repo.BestaatVoertuig(voertuig)) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                throw new VoertuigManagerException("Voertuig - BestaatVoertuig - Foutief", ex);
            }
        }

        public IReadOnlyList<Voertuig> GeefAlleVoerTuig() {
            return repo.GeefAlleVoerTuig();
        }

        public Voertuig GetVoertuig(int voertuigId) {
            if (voertuigId < 1) throw new VoertuigManagerException("VoertuigId mag niet null zijn");
            return repo.GetVoertuig(voertuigId);
        }

        public void UpdateVoertuig(Voertuig voertuig) {
            try {
                if (voertuig == null) throw new VoertuigManagerException("Voertuig - Voertuig mag niet null zijn");
                if (repo.BestaatVoertuig(voertuig)) {
                    repo.UpdateVoertuig(voertuig);
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
                if (repo.BestaatVoertuig(voertuig)) {
                    repo.VerwijderVoertuig(voertuig);
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
                if (!repo.BestaatVoertuig(voertuig)) {
                    repo.VoegVoertuigToe(voertuig);
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
