using FleetManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Bouwers
{
    public class TankkaartOpbouw
    {
        private readonly ITankkaartRepository _repo;

        public TankkaartOpbouw(ITankkaartRepository repo)
        {
            _repo = repo;
        }
    }
}
