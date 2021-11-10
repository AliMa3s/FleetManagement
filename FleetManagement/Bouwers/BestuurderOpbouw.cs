using FleetManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Bouwers
{
    public class BestuurderOpbouw
    {
        private readonly IBestuurderRepository _repo;

        public BestuurderOpbouw(IBestuurderRepository repo)
        {
            _repo = repo;
        }
    }
}
