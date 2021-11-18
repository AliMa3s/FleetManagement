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
        public ITankkaartRepository TankkaartRepo { get;  }
   
        /* Onderstaande ToDo: we moeten onderstaande minstens kunnen ophalen in DB om in het formulier te tonen 
         * Deze zijn zuiver CRUD
         * Toevoegen, wijzigen, tonen en verwijderen
         */

        //public IAutoModelRepository AutoModelRepo {get; private set; }

        //public IKleurRepository KleurRepo {get; private set; }

        //public IBrandstofTypeRepository BrandstofRepo {get; private set; }
    }
}
