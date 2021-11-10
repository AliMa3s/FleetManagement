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
        public IAdresRepository AdresRepo { get; private set; }
        public IBestuurderRepository BestuurderRepo { get; private set; }
        public IVoertuigRepository VoertuigRepo { get; private set; }
        public ITankkaartRepository TankkaartRepo { get; private set; }

        public UnitOfRepository(string connectionString)
        {
            AdresRepo = new AdresRepositoryADO(connectionString);
            BestuurderRepo = new BestuurderRepositoryADO(connectionString);
            VoertuigRepo = new VoertuigRepositoryADO(connectionString);
            TankkaartRepo = new TankkaartRepositoryADO(connectionString);
        }
    }
}
