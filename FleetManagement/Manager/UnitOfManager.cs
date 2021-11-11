using FleetManagement.Bouwers;
using FleetManagement.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager
{
    //Global Manager met apart beheer van repos & bouwers
    public class UnitOfManager : IUnitOfManager
    {
        //ADO.net repos
        public IAdresRepository AdresRepo { get; private set ;}
        public IBestuurderRepository BestuurderRepo { get; private set; }
        public IVoertuigRepository VoertuigRepo { get; private set; }
        public ITankkaartRepository TankkaartRepo { get; private set; }      
        
        //Logica om een geldige instanties op te leveren
        public VoertuigBouwer VoertuigBouwer { get; private set; }
        public BestuurderOpbouw BestuurderOpbouw { get; private set; }
        public TankkaartOpbouw TankkaartOpbouw { get; private set; }

        public Array AantalDeuren { get; private set; }
        public Array AutoTypes { get; private set; }

        public UnitOfManager(IUnitOfRepository repos)
        {
            //ADO
            AdresRepo = new AdresManager(repos.AdresRepo);
            BestuurderRepo = new BestuurderManager(repos.BestuurderRepo);
            VoertuigRepo = new VoertuigManager(repos.VoertuigRepo);
            TankkaartRepo = new TankkaartManager(repos.TankkaartRepo);

            //Interne variabelen inladen
            AantalDeuren = Enum.GetValues(new AantalDeuren().GetType());
            AutoTypes = Enum.GetValues(new AutoType().GetType());

            //Bouwers
            VoertuigBouwer = new(repos.VoertuigRepo);
            BestuurderOpbouw = new(repos.BestuurderRepo);
            TankkaartOpbouw = new(repos.TankkaartRepo);
        }
    }
}
