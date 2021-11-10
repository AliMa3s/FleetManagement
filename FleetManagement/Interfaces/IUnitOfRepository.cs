using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces
{
    public interface IUnitOfRepository
    {
        public IAdresRepository Adressen { get; }
        public IBestuurderRepository Bestuurders { get; }
        public IVoertuigRepository Voertuigen { get; }
        public ITankkaartRepository Tankkaarten { get;  }
    }
}
