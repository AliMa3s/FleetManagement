using FleetManagement.Filters;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces {
    public interface IVoertuigRepository {

        public bool BestaatVoertuig(Voertuig voertuig);
        public bool BestaatNummerplaat(string nummerPlaat);
        public bool BestaatChassisnummer(string chassisNummer);
        public Voertuig VoegVoertuigToe(Voertuig voertuig);
        public void UpdateVoertuig(Voertuig voertuig);
        public void VerwijderVoertuig(Voertuig voertuig);
        public IReadOnlyList<Voertuig> GeefAlleVoertuigenFilter(string autonaam);
        public IReadOnlyList<Voertuig> GeefAlleVoertuigenFilter(string autonaam, Filter filter);
        public Voertuig ZoekOpNummerplaat(string plaatnummer);
        public Voertuig ZoekOpChassisNummer(string chassisnummer);
    }
}
