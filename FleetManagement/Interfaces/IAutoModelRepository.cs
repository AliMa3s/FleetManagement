using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces
{
    public interface IAutoModelRepository
    {
        public IReadOnlyList<AutoModel> FilterOpAutoModelNaam(string autoModelNaam);
        void VoegAutoModelToe(AutoModel autoModel);
        void VerwijderAutoModel(AutoModel autoModel);
        void UpdateAutoModel(AutoModel autoModel);
        bool BestaatAutoModel(AutoModel autoModel);
        bool BestaatAutoModel(int automodelId);
    }
}
