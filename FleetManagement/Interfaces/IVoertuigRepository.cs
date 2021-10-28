using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces {
    public interface IVoertuigRepository {
        IReadOnlyList<Voertuig> GeefAlleVoerTuig();
        IReadOnlyList<Voertuig> ZoekVoertuigen(int? voertuigId, AutoModel automodel, string chassisNumber, string nummerPlaat, BrandstofType brandstof, Kleur kleur, AantalDeuren aantalDeuren, Bestuurder bestuurder);
        Voertuig GeefVoertuig();
        Voertuig ZoekVoertuig(int? voertuigId, AutoModel automodel, string chassisNumber, string nummerPlaat, BrandstofType brandstof, Kleur kleur, AantalDeuren aantalDeuren, Bestuurder bestuurder);
        bool BestaatVoertuig(Voertuig voertuig);
        void VoegVoertuigToe(Voertuig voertuig);
        void UpdateVoertuig(Voertuig voertuig);
        void VerwijderVoertuig(Voertuig voertuig);
    }
}
