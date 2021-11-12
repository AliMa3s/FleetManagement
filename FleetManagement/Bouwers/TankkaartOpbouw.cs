using FleetManagement.Interfaces;
using FleetManagement.Manager;
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

        public TankkaartOpbouw(TankkaartManager tankkaartManager)
        {
            _tankkaartManager = tankkaartManager;
        }
    }
}
