using FleetManagement.Interfaces;
using FleetManagement.ManagerExceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager {
    public class AdresManager : IAdresRepository
    {
        private readonly IAdresRepository _repo;

        public AdresManager(IAdresRepository repo) {
            this._repo = repo;
        }

        public bool BestaatAdres(Adres adres) {
            try {
                if (adres == null) throw new AdresManagerException("Adres mag niet null zijn");
                if (!_repo.BestaatAdres(adres)) {
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
                if (BestaatAdres(adres)) {
                    _repo.UpdateAdres(adres);
                } else {
                    throw new AdresManagerException("Adres - bestaat niet!");
                }
            } catch (Exception ex) {

                throw new AdresManagerException(ex.Message);
            }
        }
    }
}
