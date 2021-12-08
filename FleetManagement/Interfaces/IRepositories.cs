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
        IAdresRepository AdresRepo { get; }
        IBestuurderRepository BestuurderRepo { get; }
        IVoertuigRepository VoertuigRepo { get; }
        ITankkaartRepository TankkaartRepo { get; }
        IAutoModelRepository AutoModelRepo {get; }
        IVoertuigKleurRepository KleurRepo {get; }
        IBrandstofRepository BrandstofRepo { get; }
    }
}
