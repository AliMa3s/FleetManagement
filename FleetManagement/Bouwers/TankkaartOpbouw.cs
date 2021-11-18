using FleetManagement.Interfaces;
using FleetManagement.Manager;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Bouwers
{
    public class TankkaartOpbouw
    {
        private readonly TankkaartManager _tankkaartManager;

        public string TankKaartNummer { get; set; }
        public DateTime GeldigheidsDatum { get; set;  }
        public DateTime UitgeefDatum { get; set; }
        public string Pincode { get; set; }
        public bool Actief { get; private set; } = true;
        public string Brandstoffen { get; set; }
        public Bestuurder Bestuurder { get; set; }

        public TankkaartOpbouw(TankkaartManager tankkaartManager)
        {
            _tankkaartManager = tankkaartManager;
        }
    }
}
