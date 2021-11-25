using FleetManagement.Exceptions;
using FleetManagement.Interfaces;
using FleetManagement.ManagerExceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager {
    public class TankkaartManager : ITankkaartRepository
    {
        private readonly ITankkaartRepository _repo;
        public TankkaartManager(ITankkaartRepository repo) {
            this._repo = repo;
        }
        public bool BestaatTankKaart(TankKaart tankkaart) {
            try {
                if (tankkaart == null) throw new TankKaartManagerException("Tankkaart mag niet null zijn");
                if (_repo.BestaatTankKaart(tankkaart)) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                throw new TankKaartManagerException("Tankkaart - BestaatTankkaart - Foutief", ex);
            }
        }

        public IReadOnlyList<TankKaart> GeefAlleTankkaart() {
            return _repo.GeefAlleTankkaart();
        }

        public TankKaart GetTankKaart(string tankkaartNr) {
            if (string.IsNullOrWhiteSpace(tankkaartNr)) throw new TankKaartManagerException("TankkaartNr - Foutief");
            return _repo.GetTankKaart(tankkaartNr);
        }

        public void UpdateTankKaart(TankKaart tankkaart) {
            try {
                if (tankkaart == null) throw new TankKaartManagerException("Tankkaart - tankkaart mag niet null zijn");
                if (_repo.BestaatTankKaart(tankkaart)) {
                    _repo.UpdateTankKaart(tankkaart);
                } else {
                    throw new TankKaartManagerException("Tankkaart - bestaat niet!");
                }
            } catch (Exception ex) {

                throw new TankKaartManagerException(ex.Message);
            }
        }

        public void VerwijderTankKaart(TankKaart tankkaart) {
            try {
                if (tankkaart == null) throw new TankKaartManagerException("Tankkaart - tankkaart mag niet null zijn");
                if (tankkaart.TankKaartNummer == null) throw new TankKaartManagerException("Tankkaart - tankkaart moet een kaartnummer hebben");

                if (_repo.BestaatTankKaart(tankkaart)) {
                    _repo.VerwijderTankKaart(tankkaart);
                } else {
                    throw new TankKaartManagerException("Tankkaart - Tankkaart bestaat niet!");
                }
            } catch (Exception ex) {

                throw new TankKaartManagerException(ex.Message);
            }
        }

        public void VoegTankKaartToe(TankKaart tankkaart) {
            try {
                if (tankkaart == null) throw new TankKaartManagerException("TankKaart - Tankkaart mag niet null zijn");
                if (!_repo.BestaatTankKaart(tankkaart)) {
                    _repo.VoegTankKaartToe(tankkaart);
                } else {
                    throw new TankKaartManagerException("TankKaart Bestaat al");
                }
            } catch (Exception ex) {

                throw new TankKaartManagerException(ex.Message);
            }
        }

        public TankKaart ZoekTankKaart(string tankkaartNr, BrandstofType branstof) {
            if (string.IsNullOrWhiteSpace(tankkaartNr)) throw new TankKaartManagerException("TankkaartNr - Foutief");
            if (string.IsNullOrWhiteSpace(branstof.BrandstofNaam)) throw new TankKaartManagerException("BrandstofNaam - Foutief");
            return _repo.ZoekTankKaart(tankkaartNr, branstof);

        }

        public IReadOnlyList<TankKaart> ZoekTankKaarten(string tankkaartNr, BrandstofType brandstof) {
            throw new NotImplementedException();
        }
    }
}
