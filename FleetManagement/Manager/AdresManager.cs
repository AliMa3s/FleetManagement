using FleetManagement.Interfaces;
using FleetManagement.ManagerExceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager {
    public class AdresManager : IAdresRepository {
        private IAdresRepository repo;

        public AdresManager(IAdresRepository repo) {
            this.repo = repo;
        }

        public bool BestaatAdres(Adres adres) {
            try {
                if (adres == null) throw new AdresManagerException("Adres mag niet null zijn");
                if (!repo.BestaatAdres(adres)) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                throw new AdresManagerException("Adres - BestaatAdres - Foutief", ex);
            }
        }

        public bool BestaatAdres(int adresId) {
            try {
                if (adresId < 1) throw new AdresManagerException("Adres id kan niet kleiner dan 0 zijn");
                if (!repo.BestaatAdres(adresId)) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                throw new AdresManagerException("Adres - BestaatAdres - Foutief", ex);
            }
        }

        public void UpdateAdres(Adres adres) {
            try {
                if (adres == null) throw new AdresManagerException("Adres - Adres mag niet null zijn");
                if (repo.BestaatAdres(adres)) {
                    repo.UpdateAdres(adres);
                } else {
                    throw new AdresManagerException("Adres - bestaat niet!");
                }
            } catch (Exception ex) {

                throw;
            }
        }

        public void VerwijderAders(Adres adres) {
            try {
                if (adres == null) throw new AdresManagerException("Adres - Adres mag niet null zijn");
                if (repo.BestaatAdres(adres)) {
                    repo.VerwijderAders(adres);
                } else {
                    throw new AdresManagerException("Adres - bestaat niet!");
                }
            } catch (Exception ex) {

                throw new AdresManagerException(ex.Message);
            }
        }

        public void VoegAdresToe(Adres adres) {
            try {
                if (adres == null) throw new AdresManagerException("Adres - Adres mag niet null zijn");
                if (!repo.BestaatAdres(adres.AdresId)) {
                    repo.VoegAdresToe(adres);
                } else {
                    throw new AdresManagerException("Adres Bestaat al");
                }
            } catch (Exception ex) {

                throw new AdresManagerException(ex.Message);
            }
        }
    }
}
