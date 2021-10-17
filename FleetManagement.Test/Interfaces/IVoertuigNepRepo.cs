using FleetManagement.Model;
using FleetManagement.Test.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Test.Interfaces
{
    interface IVoertuigNepRepo
    {
        public Voertuig GeefVoertuig(string chassisNummer);
        public bool IsVoertuigAanwezig(string chassisNummer);
    }
}