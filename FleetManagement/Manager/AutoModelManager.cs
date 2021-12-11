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
                if (autoModel == null) throw new AutoModelManagerException("AutoModel mag niet null zijn");

                if (!_repo.BestaatAutoModelNaam(autoModel)) {
                    return false;
                }
                else {
                    return true;
                }
            }
            catch (Exception ex) {
                throw new AutoModelManagerException(ex.Message);
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
                throw new AutoModelManagerException(ex.Message);
            }
        }

        public void UpdateAutoModel(AutoModel autoModel) {
            try {
                if (autoModel == null) throw new AutoModelManagerException("AutoModel - AutoModel mag niet null zijn");
                
                if (BestaatAutoModel(autoModel.AutoModelId)) {

                    if(!BestaatAutoModelNaam(autoModel))
                    {
                        _repo.UpdateAutoModel(autoModel);
                    }
                    else
                    {
                        throw new AutoModelManagerException("Update - AutoModel bestaat al");
                    }

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
                if (BestaatAutoModelNaam(autoModel)) {
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
                if (!_repo.BestaatAutoModel(automodelid)) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                throw new AutoModelManagerException(ex.Message);
            }
        }

    }
}
