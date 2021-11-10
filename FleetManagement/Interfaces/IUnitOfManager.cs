using FleetManagement.Bouwers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces
{
    public interface IUnitOfManager : IUnitOfRepository
    {
        public VoertuigBouwer VoertuigBouwer { get; }
        public BestuurderOpbouw BestuurderOpbouw { get; }
        public TankkaartOpbouw TankkaartOpbouw { get; }
    }
}
