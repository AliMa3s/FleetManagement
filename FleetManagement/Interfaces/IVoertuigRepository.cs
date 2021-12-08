using FleetManagement.Filters;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces {
    public interface IVoertuigRepository {

        bool BestaatVoertuig(Voertuig voertuig);
        bool BestaatNummerplaat(string nummerPlaat);
        bool BestaatChassisnummer(string chassisNummer);
        Voertuig VoegVoertuigToe(Voertuig voertuig);
        void UpdateVoertuig(Voertuig voertuig);
        void UpdateVoertuig(Voertuig voertuig, string anderChassisNummer, string anderNummerplaat);
        void VerwijderVoertuig(Voertuig voertuig);
        IReadOnlyList<Voertuig> GeefAlleVoertuigenFilter(string autonaam, Filter filter);
        Voertuig ZoekOpNummerplaatOfChassisNummer(string plaatnummerOfChassis);
        IReadOnlyList<Voertuig> SelecteerZonderBestuurderFilter(string autonaam); 
    }
}
