using FleetManagement.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager.Interfaces
{
    public interface  IVoertuigManager : IVoertuigRepository
    {
        //Implementeert en profileert zichzelf als Manager

        //interne variabelen inladen
        public IEnumerable<AantalDeuren> AantalDeuren { get; }
        public IEnumerable<AutoType> AutoTypes { get; }
    }
}
