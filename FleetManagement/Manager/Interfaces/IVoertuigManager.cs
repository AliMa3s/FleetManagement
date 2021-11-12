using FleetManagement.Interfaces;
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
    }
}
