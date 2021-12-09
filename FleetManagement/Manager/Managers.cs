using FleetManagement.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager
{
    public class Managers
    {
        //Managers
        public AdresManager AdresManager { get; private set; }
        public BestuurderManager BestuurderManager { get; private set; }
        public VoertuigManager VoertuigManager { get; private set; }
        public TankkaartManager TankkaartManager { get; private set; }
        public AutoModelManager AutoModelManager { get; private set; }
        private KleurManager KleurManager { get; set; }
        private BrandstofManager BrandstofManager { get; set; }

        //Klaarzetten voor Autotype in te laden via configFile
        public IEnumerable<KeyValuePair<string, string>> AutoTypes { get; set; } 

        //Kleur
        public IList<Kleur> Kleuren { get; set; }

        //Brandstoffen
        public IList<BrandstofType> Brandstoffen { get; set; }

        public Managers(IRepositories repos)
        {
            //ADO
            AdresManager = new AdresManager(repos.AdresRepo);
            BestuurderManager = new BestuurderManager(repos.BestuurderRepo);
            VoertuigManager = new VoertuigManager(repos.VoertuigRepo);
            TankkaartManager = new TankkaartManager(repos.TankkaartRepo);
            KleurManager = new KleurManager(repos.KleurRepo);
            AutoModelManager = new AutoModelManager(repos.AutoModelRepo);
            BrandstofManager = new BrandstofManager(repos.BrandstofRepo);

            //Kleuren éénmaal inladen
            Kleuren = KleurManager.GeefAlleVoertuigKleuren().ToList();

            //Brandstoffen éénmaal inladen
            Brandstoffen = BrandstofManager.GeeAlleBrandstoffen().ToList();
        }
    }
}
