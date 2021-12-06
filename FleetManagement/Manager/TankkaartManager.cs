using FleetManagement.CheckFormats;
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
        #warning nog geen enkele manager heeft een unit test.
        private readonly ITankkaartRepository _repo;
        public TankkaartManager(ITankkaartRepository repo) {
            this._repo = repo;
        }
        
        /// <summary>
        ///  als tankkaart gevonden wordt moet er een true gegeven worden i.p.v false.
        /// </summary>
        /// <param name="tankkaart"></param>
        /// <returns></returns>
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

        public bool BestaatTankkaart(string tankkaartNummer)
        {
            try
            {
                if (tankkaartNummer == null) throw new TankKaartManagerException("Tankkaart mag niet null zijn");
                if (string.IsNullOrEmpty(tankkaartNummer)) throw new TankKaartManagerException("Tankkaart mag niet null zijn");
                if (_repo.BestaatTankkaart(tankkaartNummer))
                {
                    return true;
                } else
                    return false;
            }
            catch(Exception ex)
            {
                throw new TankKaartManagerException("Tankkaart - BestaatTankkaart - Foutief", ex);
            }
        }

        public IReadOnlyList<TankKaart> GeefAlleTankkaarten() {

            try
            {
                return _repo.GeefAlleTankkaarten();
            }
            catch (Exception ex)
            {
                throw new TankKaartManagerException("TankKaart - GeefAlleTankkaarten - gefaald", ex);
            }
        }

        public TankKaart ZoekTankKaart(string tankkaartNr) {

            try
            {
                if (string.IsNullOrWhiteSpace(tankkaartNr)) throw new TankKaartManagerException("TankkaartNr - Foutief");

                return _repo.ZoekTankKaart(tankkaartNr);
            }
            catch (Exception ex)
            {
                throw new TankKaartManagerException("TankKaart - ZoekTankkaart - gefaald", ex);
            }
        }

        public void UpdateTankKaart(TankKaart tankkaart) {
            try {

                //Brandstoffen update moet nog ingevoegd worden
                //Haal brandstoffen op => Check wat verwijderd moeten worden en geupdatet moet worden
                //Voer dan uit na de update zodat brandstoffen in DB up to date zijn

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
            try
            {
                return _repo.TankaartenZonderBestuurder().Where(t => t.Actief == true).ToList();
            }
            catch (Exception ex)
            {
                throw new TankKaartManagerException("TankKaart - TankkaartZonderBestuurder - gefaald", ex);
            }
        }

        public IReadOnlyList<TankKaart> ZoekTankKaarten(bool isGeldig) {

            try
            {
                return _repo.ZoekTankKaarten(isGeldig).Where(t => t.Actief == isGeldig).ToList();
            }
            catch (Exception ex)
            {
                throw new TankKaartManagerException("TankKaart - ZoekTankkaarten - gefaald", ex);
            }     
        }

        public IReadOnlyList<BrandstofType> BrandstoffenVoorTankaart(TankKaart tankkaart)
        {
            try
            {
                if (tankkaart == null) throw new TankKaartManagerException("TankKaart - Tankkaart mag niet null zijn");

                return _repo.BrandstoffenVoorTankaart(tankkaart);
            }
            catch (Exception ex)
            {
                throw new TankKaartManagerException("TankKaart - BrandstoffenVoorTankaart - gefaald", ex);
            }
        }

        //Apart verwijderen en apart toevoegen maakt het eenvoudiger voor updaten
        public void VerwijderBrandstoffen(TankKaart tankKaart)
        {
            try
            {
                if (tankKaart == null) throw new TankKaartManagerException("TankKaart - Tankkaart mag niet null zijn");

                _repo.VerwijderBrandstoffen(tankKaart);
            }
            catch (Exception ex)
            {
                throw new TankKaartManagerException("TankKaart - VerwijderBrandstoffen - gefaald", ex);
            }
        }

        public void VoegTankkaartBrandstofToe(TankKaart tankkaart, BrandstofType brandstof)
        {
            try
            {
                if (tankkaart == null) throw new TankKaartManagerException("TankKaart - Tankkaart mag niet null zijn");
                if (brandstof == null) throw new TankKaartManagerException("TankKaart - Brandstof mag niet null zijn");

                _repo.VoegTankkaartBrandstofToe(tankkaart, brandstof);
            }
            catch (Exception ex)
            {
                throw new TankKaartManagerException("TankKaart - VoegTankkaartBrandstoffenToe - gefaald", ex);
            }
        }

        public bool BestaatTankkaartBrandstof(TankKaart tankkaart, BrandstofType brandstof)
        {
            try
            {
                if (tankkaart == null) throw new TankKaartManagerException("TankKaart - Tankkaart mag niet null zijn");
                if (brandstof == null) throw new TankKaartManagerException("TankKaart - Brandstof mag niet null zijn");

                return _repo.BestaatTankkaartBrandstof(tankkaart, brandstof);
            }
            catch (Exception ex)
            {
                throw new TankKaartManagerException("TankKaart - BestaatTankkaartBrandstof - gefaald", ex);
            }
        }

        public TankKaart UpdateTankKaart(TankKaart tankkaart, string AnderTankkaartNummer)
        {
            try
            {
                //Brandstoffen update moet nog ingevoegd worden
                //Haal brandstoffen op => Check wat verwijderd moeten worden en geupdatet moet worden
                //Voer dan uit na de update zodat brandstoffen in DB up to date zijn

                if (!CheckFormat.IsTankKaartNummerGeldig(AnderTankkaartNummer)) { }

                if(tankkaart.TankKaartNummer == AnderTankkaartNummer) 
                    throw new TankKaartManagerException("TankKaart - Huidige tankkaartnummer en ander tankkaartnummer moet verschillend zijn");

                if (_repo.BestaatTankKaart(tankkaart))
                {
                    return _repo.UpdateTankKaart(tankkaart, AnderTankkaartNummer);
                }
                else
                {
                    throw new TankKaartManagerException("Tankkaart - bestaat niet!");
                }
            }
            catch (Exception ex)
            {
                throw new TankKaartManagerException("TankKaart - UpdateTankKaart - gefaald", ex);
            }
        }

        
    }
}
