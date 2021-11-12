using FleetManagement.Interfaces;
using FleetManagement.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Bouwers
{
    public class BestuurderOpbouw
    {
        private readonly IBestuurderManager _bestuurderManager;

        public BestuurderOpbouw(IBestuurderManager bestuurderManager)
        {
            _bestuurderManager = bestuurderManager;
        }
    }
}
