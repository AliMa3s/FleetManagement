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
        private readonly ITankkaartRepository _repo;
        public TankkaartManager(ITankkaartRepository repo) {
            this._repo = repo;
        }
        
        /// <summary>
        ///  als tankkaart gevonden wordt moet er een true gegeven worden i.p.v false.
        /// </summary>
        /// <param name="tankkaart"></param>
        /// <returns></returns>

        public bool BestaatTankkaart(string tankkaartNummer)
        {
            try
            {
                if(!CheckFormat.IsTankKaartNummerGeldig(tankkaartNummer)) { }

                if (_repo.BestaatTankkaart(tankkaartNummer))
                {
                    return true;
                } else
                    return false;
            }
            catch(Exception ex)
            {
                throw new TankKaartManagerException(ex.Message);
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
                throw new TankKaartManagerException(ex.Message);
            }
        }

        public void UpdateTankKaart(TankKaart tankkaart) {
            try {

                //Brandstoffen update moet nog ingevoegd worden
                //Haal brandstoffen op => Check wat verwijderd moeten worden en geupdatet moet worden
                //Voer dan uit na de update zodat brandstoffen in DB up to date zijn

                if (tankkaart == null) throw new TankKaartManagerException("Tankkaart - tankkaart mag niet null zijn");

                if (BestaatTankkaart(tankkaart.TankKaartNummer)) 
                {
                    IReadOnlyList<BrandstofType> brandstoffen = BrandstoffenVoorTankaart(tankkaart);
                    if (brandstoffen.Count > 0)
                    {
                        VerwijderBrandstoffen(tankkaart);
                    }

                    _repo.UpdateTankKaart(tankkaart);
                } 
                else 
                {
                    throw new TankKaartManagerException("Tankkaart - bestaat niet!");
                }
            } catch (Exception ex) {

                throw new TankKaartManagerException(ex.Message);
            }
        }

        public void VoegTankKaartToe(TankKaart tankkaart) {
            try {
                if (tankkaart == null) throw new TankKaartManagerException("TankKaart - Tankkaart mag niet null zijn");
                if (!BestaatTankkaart(tankkaart.TankKaartNummer)) {
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
                throw new TankKaartManagerException(ex.Message);
            }
        }

        //Apart verwijderen en apart toevoegen maakt het eenvoudiger voor updaten
        public void VerwijderBrandstoffen(TankKaart tankkaart)
        {
            try
            {
                if (tankkaart == null) throw new TankKaartManagerException("TankKaart - Tankkaart mag niet null zijn");

                _repo.VerwijderBrandstoffen(tankkaart);
            }
            catch (Exception ex)
            {
                throw new TankKaartManagerException(ex.Message);
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
                    throw new TankKaartManagerException("Huidige tankkaartnummer en ander tankkaartnummer moet verschillend zijn");

                if (BestaatTankkaart(AnderTankkaartNummer))
                    throw new TankKaartManagerException("Gewijzigd tankkaartnummer bestaat al");

                if (BestaatTankkaart(tankkaart.TankKaartNummer))
                {
                    IReadOnlyList<BrandstofType> brandstoffen = BrandstoffenVoorTankaart(tankkaart);
                    if(brandstoffen.Count > 0)
                    {
                        VerwijderBrandstoffen(tankkaart);
                    }

                    return _repo.UpdateTankKaart(tankkaart, AnderTankkaartNummer);
                }
                else
                {
                    throw new TankKaartManagerException("Tankkaart - bestaat niet!");
                }
            }
            catch (Exception ex)
            {
                throw new TankKaartManagerException(ex.Message);
            }
        }

        
    }
}
