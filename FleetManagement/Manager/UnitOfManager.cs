using FleetManagement.Bouwers;
using FleetManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager
{
    //Global Manager met apart beheer van repos & bouwers
    public class UnitOfManager : IUnitOfRepository, IUnitOfManager
    {
        //ADO.net repos
        public IAdresRepository Adressen { get; private set ;}
        public IBestuurderRepository Bestuurders { get; private set; }
        public IVoertuigRepository Voertuigen { get; private set; }
        public ITankkaartRepository Tankkaarten { get; private set; }      
        
        //Logica om een geldige instanties op te leveren
        public VoertuigBouwer VoertuigBouwer { get; private set; }
        public BestuurderOpbouw BestuurderOpbouw { get; private set; }
        public TankkaartOpbouw TankkaartOpbouw { get; private set; }

        public UnitOfManager(IUnitOfRepository globalRepo)
        {
            Adressen = new AdresManager(globalRepo.Adressen);
            Bestuurders = new BestuurderManager(globalRepo.Bestuurders);
            Voertuigen = new VoertuigManager(globalRepo.Voertuigen);
            Tankkaarten = new TankkaartManager(globalRepo.Tankkaarten);

            VoertuigBouwer = new(globalRepo.Voertuigen);
            BestuurderOpbouw = new(globalRepo.Bestuurders);
            TankkaartOpbouw = new(globalRepo.Tankkaarten);
        }
    }
}
