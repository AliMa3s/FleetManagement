using FleetManagement.Interfaces;
using FleetManagement.ManagerExceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager
{
    public class AutoModelManager : IAutoModelRepository
    {
        private readonly IAutoModelRepository _repo;

        public AutoModelManager(IAutoModelRepository repo)
        {
            _repo = repo;
        }

        public bool BestaatAutoModelNaam(AutoModel autoModel) {
            try
            {
                return autoModel == null
                    ? throw new AutoModelManagerException("AutoModel mag niet null zijn")
                    : _repo.BestaatAutoModelNaam(autoModel);
            }
            catch (Exception ex) {
                throw new AutoModelManagerException(ex.Message);
            }
        }

        public IReadOnlyList<AutoModel> FilterOpAutoModelNaam(string autoModelNaam)
        {
            try
            {
                return autoModelNaam == null
                   ? throw new AutoModelManagerException("AutoModel mag niet null zijn")
                   : _repo.FilterOpAutoModelNaam(autoModelNaam);
            }
            catch (Exception ex)
            {
                throw new AutoModelManagerException(ex.Message);
            }
        }

        public void UpdateAutoModel(AutoModel autoModel) {
            try {
                if (autoModel == null) { throw new AutoModelManagerException("AutoModel - AutoModel mag niet null zijn"); }
                
                if (BestaatAutoModel(autoModel.AutoModelId)) {

                    if(!BestaatAutoModelNaam(autoModel))
                    {
                        _repo.UpdateAutoModel(autoModel);
                    }
                    else
                    {
                        throw new AutoModelManagerException("AutoModel bestaat al");
                    }

                } else {
                    throw new AutoModelManagerException("AutoModel bestaat niet!");
                }
            } catch (Exception ex) {

                throw new AutoModelManagerException(ex.Message);
            }
        }

        public void VerwijderAutoModel(AutoModel autoModel) {
            try {
                if (autoModel == null) { throw new AutoModelManagerException("AutoModel - AutoModel mag niet null zijn"); }

                if (BestaatAutoModel(autoModel.AutoModelId)) {

                    if(!IsAutoModelInGebruik(autoModel))
                    {
                        _repo.VerwijderAutoModel(autoModel);
                    }
                    else
                    {
                        throw new AutoModelManagerException("Kan automodel niet verwijderen omdat het nog in gebruik is");
                    }
                    
                } else {
                    throw new AutoModelManagerException("AutoModel - bestaat niet!");
                }
            } catch (Exception ex) {

                throw new AutoModelManagerException(ex.Message);
            }
        }

        public void VoegAutoModelToe(AutoModel autoModel) {
            try {
                if (autoModel == null) { throw new AutoModelManagerException("AutoModel - autoModel mag niet null zijn"); }

                if (!BestaatAutoModelNaam(autoModel)) {
                    _repo.VoegAutoModelToe(autoModel);
                } else {
                    throw new AutoModelManagerException("AutoModel bestaat al");
                }
            } catch (Exception ex) {

                throw new AutoModelManagerException(ex.Message);
            }
        }

        public bool BestaatAutoModel(int automodelid) {
            try {
                return automodelid < 1
                    ? throw new AutoModelManagerException("AutoModelId moet meer zijn dan 0")
                    : _repo.BestaatAutoModel(automodelid);
            }
            catch (Exception ex) {
                throw new AutoModelManagerException(ex.Message);
            }
        }

        public bool IsAutoModelInGebruik(AutoModel autoModel)
        {
            try
            {
                return autoModel == null
                    ? throw new AutoModelManagerException("AutoModel - autoModel mag niet null zijn")
                    : _repo.IsAutoModelInGebruik(autoModel);
            }
            catch (Exception ex)
            {
                throw new AutoModelManagerException(ex.Message);
            }
        }

        public IReadOnlyList<AutoModel> ZoekOpAutoType(AutoType autoType, string autoModelnaam)
        {
            if (autoType == null) throw new AutoModelManagerException("AutoType mag niet null zijn");

            //mag wel leeg zijn omwille van filter
            return autoType == null
                ? throw new AutoModelManagerException("automodel naam mag niet null zijn")
                : _repo.ZoekOpAutoType(autoType, autoModelnaam);
        }
    }
}
