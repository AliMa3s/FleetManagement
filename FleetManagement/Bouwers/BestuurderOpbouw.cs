using FleetManagement.Interfaces;
using FleetManagement.Manager.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Bouwers
{
    public class BestuurderOpbouw
    {
        private readonly IBestuurderManager _bestuurderManager;

        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string GeboorteDatum { get; }
        public Adres Adres { get; set; }
        public string TypeRijbewijs { get; set; }
        public string RijBewijsNummer { get; }
        public string RijksRegisterNummer { get; }
        public Voertuig Voertuig { get; set; }
        public TankKaart TankKaart { get; set; }

        public BestuurderOpbouw(IBestuurderManager bestuurderManager)
        {
            _bestuurderManager = bestuurderManager;
        }
    }
}
