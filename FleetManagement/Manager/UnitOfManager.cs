using FleetManagement.Bouwers;
using FleetManagement.Interfaces;
using FleetManagement.Manager.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager
{
    //Global Manager met apart beheer van repos & bouwers
    public class UnitOfManager
    {
        //Managers
        public IAdresManager AdresManager { get; private set ;}
        public IBestuurderManager BestuurderManager { get; private set; }
        public IVoertuigManager VoertuigManager { get; private set; }
        public ITankkaartManager TankkaartManager { get; private set; }
        
        //Logica om een geldige instanties op te leveren
        public VoertuigBouwer VoertuigBouwer { get; set; }
        public BestuurderOpbouw BestuurderOpbouw { get; set; }
        public TankkaartOpbouw TankkaartOpbouw { get; set; }


        public UnitOfManager(IUnitOfRepository repos)
        {
            //ADO
            AdresManager = new AdresManager(repos.AdresRepo);
            BestuurderManager = new BestuurderManager(repos.BestuurderRepo);
            VoertuigManager = new VoertuigManager(repos.VoertuigRepo);
            TankkaartManager = new TankkaartManager(repos.TankkaartRepo);
        }
    }
}
