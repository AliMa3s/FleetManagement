using FleetManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.Repositories
{
    public class Repositories : IRepositories
    {
        public IAdresRepository AdresRepo { get; private set; }
        public IBestuurderRepository BestuurderRepo { get; private set; }
        public IVoertuigRepository VoertuigRepo { get; private set; }
        public ITankkaartRepository TankkaartRepo { get; private set; }
        public IAutoModelRepository AutoModelRepo { get; private set; }
        public IVoertuigKleurRepository KleurRepo { get; private set; }
        public IBrandstofRepository BrandstofRepo { get; private set; }

        public Repositories(string connectionString)
        {
            AdresRepo = new AdresRepositoryADO(connectionString);
            BestuurderRepo = new BestuurderRepositoryADO(connectionString);
            VoertuigRepo = new VoertuigRepositoryADO(connectionString);
            AutoModelRepo = new AutoModelRepositoryADO(connectionString);
            KleurRepo = new KleurRepositoryADO(connectionString);
            BrandstofRepo = new  BrandstofRepositoryADO(connectionString);
        }
    }
}
