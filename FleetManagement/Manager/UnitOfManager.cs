using FleetManagement.Bouwers;
using FleetManagement.Interfaces;
using FleetManagement.Manager.Interfaces;
using FleetManagement.Manager.Roles;
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
        public IBestuurderManager BestuurderManager { get; private set; }
        public IVoertuigManager VoertuigManager { get; private set; }
        public ITankkaartManager TankkaartManager { get; private set; }

        //Authenticatie
        public Authenticatie Auth { get; private set; }
        public Role LoggedIn { get; set; }

        //Klaarzetten voor Autotype in te laden via configFile
        public IEnumerable<KeyValuePair<string, string>> AutoTypes { get; set; }

        //Klaarzetten voor Kleur

        public UnitOfManager(IUnitOfRepository repos)
        {
            //ADO
            BestuurderManager = new BestuurderManager(repos.BestuurderRepo);
            VoertuigManager = new VoertuigManager(repos.VoertuigRepo);
            TankkaartManager = new TankkaartManager(repos.TankkaartRepo);

            //Roles ophalen
            Auth = new();
        }
    }
}
