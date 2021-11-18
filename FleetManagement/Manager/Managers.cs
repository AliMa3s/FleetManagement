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
    public class Managers
    {
        //Managers
        public AdresManager AdresManager { get; private set; }
        public BestuurderManager BestuurderManager { get; private set; }
        public VoertuigManager VoertuigManager { get; private set; }
        public TankkaartManager TankkaartManager { get; private set; }

        //Klaarzetten voor Autotype in te laden via configFile
        public IEnumerable<KeyValuePair<string, string>> AutoTypes { get; set; }

        //Klaarzetten voor Kleur

        public Managers(IRepositories repos)
        {
            //ADO
            AdresManager = new AdresManager(repos.AdresRepo);
            BestuurderManager = new BestuurderManager(repos.BestuurderRepo);
            VoertuigManager = new VoertuigManager(repos.VoertuigRepo);
            TankkaartManager = new TankkaartManager(repos.TankkaartRepo);
        }
    }
}
