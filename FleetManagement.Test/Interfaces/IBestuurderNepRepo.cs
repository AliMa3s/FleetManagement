using FleetManagement.Model;
using FleetManagement.Test.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Test.Interfaces
{
    interface IBestuurderNepRepo
    {
        public Bestuurder GeefBestuurder(string rijksRegisterNummer);
        public bool IsBestuurderAanwezig(string rijksRegisterNummer);
    }
}