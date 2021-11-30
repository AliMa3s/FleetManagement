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

        public IReadOnlyList<TankKaart> GeefAlleTankkaarten() {
            return _repo.GeefAlleTankkaarten();
        }

        public TankKaart ZoekTankKaart(string tankkaartNr) {
            if (string.IsNullOrWhiteSpace(tankkaartNr)) throw new TankKaartManagerException("TankkaartNr - Foutief");

            return _repo.ZoekTankKaart(tankkaartNr);
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

        public IReadOnlyList<TankKaart> TankaartenZonderBestuurder()
        {
            return _repo.TankaartenZonderBestuurder().Where(t => t.Actief == true).ToList();
        }

        public IReadOnlyList<TankKaart> ZoekTankKaarten(bool isGeldig) {
            return _repo.ZoekTankKaarten(isGeldig).Where(t => t.Actief == isGeldig).ToList();
        }

        public IReadOnlyList<BrandstofType> BrandstoffenVoorTankaart(TankKaart tankkaart)
        {
            if (tankkaart == null) throw new TankKaartManagerException("TankKaart - Tankkaart mag niet null zijn");

            return _repo.BrandstoffenVoorTankaart(tankkaart);
        }

        //Apart verwijderen en apart toevoegen maakt het eenvoudiger voor updaten
        public void VerwijderBrandstoffen(TankKaart tankKaart)
        {
            if (tankKaart == null) throw new TankKaartManagerException("TankKaart - Tankkaart mag niet null zijn");

            _repo.VerwijderBrandstoffen(tankKaart);
        }

        public void VoegTankkaartBrandstofToe(TankKaart tankkaart, BrandstofType brandstof)
        {
            if (tankkaart == null) throw new TankKaartManagerException("TankKaart - Tankkaart mag niet null zijn");
            if (brandstof == null) throw new TankKaartManagerException("TankKaart - Brandstof mag niet null zijn");

            _repo.VoegTankkaartBrandstofToe(tankkaart, brandstof);

        }

        public bool BestaatTankkaartBrandstof(TankKaart tankkaart, BrandstofType brandstof)
        {
            if (tankkaart == null) throw new TankKaartManagerException("TankKaart - Tankkaart mag niet null zijn");
            if (brandstof == null) throw new TankKaartManagerException("TankKaart - Brandstof mag niet null zijn");

            return _repo.BestaatTankkaartBrandstof(tankkaart, brandstof);
        }
    }
}
