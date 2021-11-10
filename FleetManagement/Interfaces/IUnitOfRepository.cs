using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces
{
    public interface IUnitOfRepository
    {
        public IAdresRepository AdresRepo { get; }  //Gaan we volgens mij (Filip) niet nodig hebben
        public IBestuurderRepository BestuurderRepo { get; } //Als we bestuurder aanmaken, bevat deze (mogelijk) het adres (JOIN Adres)
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
