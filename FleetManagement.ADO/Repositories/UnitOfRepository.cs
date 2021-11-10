using FleetManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.Repositories
{
    public class UnitOfRepository : IUnitOfRepository
    {
        public IAdresRepository Adressen { get; private set; }
        public IBestuurderRepository Bestuurders { get; private set; }
        public IVoertuigRepository Voertuigen { get; private set; }
        public ITankkaartRepository Tankkaarten { get; private set; }

        public UnitOfRepository(string connectionString)
        {
            Adressen = new AdresRepositoryADO(connectionString);
            Bestuurders = new BestuurderRepositoryADO(connectionString);
            Voertuigen = new VoertuigRepositoryADO(connectionString);
            Tankkaarten = new TankkaartRepositoryADO(connectionString);
        }
    }
}
