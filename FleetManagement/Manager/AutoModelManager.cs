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

        public bool BestaatAutoModel(AutoModel autoModel) {
            try { 
            if (autoModel == null) throw new AutoModelManagerException("AutoModel mag niet null zijn");
            if (!_repo.BestaatAutoModel(autoModel)) {
                return false;
            } else {
                return true;
            }
        } catch (Exception ex) {
                throw new AutoModelManagerException("AutoModel - BestaatAutoModel - Foutief", ex);
    }
}

        public IReadOnlyList<AutoModel> FilterOpAutoModelNaam(string autoModelNaam)
        {
            try
            {
               if(autoModelNaam == null) throw new AutoModelManagerException("AutoModel mag niet null zijn");

                return _repo.FilterOpAutoModelNaam(autoModelNaam);
            }
            catch (Exception ex)
            {
                throw new AutoModelManagerException("AutoModel - FilterOpAutoModelNaam - Foutief", ex);
            }
        }

        public void UpdateAutoModel(AutoModel autoModel) {
            try {
                if (autoModel == null) throw new AutoModelManagerException("AutoModel - AutoModel mag niet null zijn");
                if (_repo.BestaatAutoModel(autoModel)) {
                    _repo.UpdateAutoModel(autoModel);
                } else {
                    throw new AutoModelManagerException("AutoModel - bestaat niet!");
                }
            } catch (Exception ex) {

                throw new AutoModelManagerException(ex.Message);
            }
        }

        public void VerwijderAutoModel(AutoModel autoModel) {
            try {
                if (autoModel == null) throw new AutoModelManagerException("AutoModel - AutoModel mag niet null zijn");
                if (_repo.BestaatAutoModel(autoModel)) {
                    _repo.VerwijderAutoModel(autoModel);
                } else {
                    throw new AutoModelManagerException("AutoModel - bestaat niet!");
                }
            } catch (Exception ex) {

                throw new AutoModelManagerException(ex.Message);
            }
        }

        public void VoegAutoModelToe(AutoModel autoModel) {
            try {
                if (autoModel == null) throw new AutoModelManagerException("autoModel - autoModel mag niet null zijn");
                if (!_repo.BestaatAutoModel(autoModel.AutoModelId)) {
                    _repo.VoegAutoModelToe(autoModel);
                } else {
                    throw new AutoModelManagerException("autoModel Bestaat al");
                }
            } catch (Exception ex) {

                throw new AutoModelManagerException(ex.Message);
            }
        }


        public bool BestaatAutoModel(int automodelid) {
            try {
                if (automodelid < 1) throw new AutoModelManagerException("autoModel id kan niet kleiner dan 0 zijn");
                if (!_repo.BestaatAutoModel(automodelid)) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                throw new AutoModelManagerException("autoModel - BestaatAutoModel - Foutief", ex);
            }
        }

    }
}
