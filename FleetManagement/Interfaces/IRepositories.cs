using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces
{
    public interface IRepositories
    {
        public IAdresRepository AdresRepo { get; }
        public IBestuurderRepository BestuurderRepo { get; }
        public IVoertuigRepository VoertuigRepo { get; }
        public ITankkaartRepository TankkaartRepo { get; }
        public IAutoModelRepository AutoModelRepo {get; }
        public IVoertuigKleurRepository KleurRepo {get; }
        public IBrandstofRepository BrandstofRepo { get; }
    }
}
