using FleetManagement.Exceptions;
using FleetManagement.Interfaces;
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
            if (autoModel == null) throw new AutoModelException("AutoModel mag niet null zijn");
            if (!_repo.BestaatAutoModel(autoModel)) {
                return false;
            } else {
                return true;
            }
        } catch (Exception ex) {
                throw new AutoModelException("AutoModel - BestaatAutoModel - Foutief", ex);
    }
}

        public IReadOnlyList<AutoModel> FilterOpAutoModelNaam(string autoModelNaam)
        {
            return _repo.FilterOpAutoModelNaam(autoModelNaam);
        }

        public void UpdateAutoModel(AutoModel autoModel) {
            try {
                if (autoModel == null) throw new AutoModelException("AutoModel - AutoModel mag niet null zijn");
                if (_repo.BestaatAutoModel(autoModel)) {
                    _repo.UpdateAutoModel(autoModel);
                } else {
                    throw new AutoModelException("AutoModel - bestaat niet!");
                }
            } catch (Exception ex) {

                throw new AutoModelException(ex.Message);
            }
        }

        public void VerwijderAutoModel(AutoModel autoModel) {
            try {
                if (autoModel == null) throw new AutoModelException("AutoModel - AutoModel mag niet null zijn");
                if (_repo.BestaatAutoModel(autoModel)) {
                    _repo.VerwijderAutoModel(autoModel);
                } else {
                    throw new AutoModelException("AutoModel - bestaat niet!");
                }
            } catch (Exception ex) {

                throw new AutoModelException(ex.Message);
            }
        }

        public void VoegAutoModelToe(AutoModel autoModel) {
            try {
                if (autoModel == null) throw new AutoModelException("autoModel - autoModel mag niet null zijn");
                if (!_repo.BestaatAutoModel(autoModel.AutoModelId)) {
                    _repo.VoegAutoModelToe(autoModel);
                } else {
                    throw new AutoModelException("autoModel Bestaat al");
                }
            } catch (Exception ex) {

                throw new AutoModelException(ex.Message);
            }
        }


        public bool BestaatAutoModel(int automodelid) {
            try {
                if (automodelid < 1) throw new AutoModelException("autoModel id kan niet kleiner dan 0 zijn");
                if (!_repo.BestaatAutoModel(automodelid)) {
                    return false;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                throw new AutoModelException("autoModel - BestaatAutoModel - Foutief", ex);
            }
        }

    }
}
