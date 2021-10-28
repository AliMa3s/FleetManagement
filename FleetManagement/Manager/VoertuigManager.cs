using FleetManagement.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager {
    public class VoertuigManager : IVoertuigRepository {
        public bool BestaatVoertuig(Voertuig voertuig) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Voertuig> GeefAlleVoerTuig() {
            throw new NotImplementedException();
        }

        public Voertuig GeefVoertuig() {
            throw new NotImplementedException();
        }

        public void UpdateVoertuig(Voertuig voertuig) {
            throw new NotImplementedException();
        }

        public void VerwijderVoertuig(Voertuig voertuig) {
            throw new NotImplementedException();
        }

        public void VoegVoertuigToe(Voertuig voertuig) {
            throw new NotImplementedException();
        }

        public Voertuig ZoekVoertuig(int? voertuigId, AutoModel automodel, string chassisNumber, string nummerPlaat, BrandstofType brandstof, Kleur kleur, AantalDeuren aantalDeuren, Bestuurder bestuurder) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Voertuig> ZoekVoertuigen(int? voertuigId, AutoModel automodel, string chassisNumber, string nummerPlaat, BrandstofType brandstof, Kleur kleur, AantalDeuren aantalDeuren, Bestuurder bestuurder) {
            throw new NotImplementedException();
        }
    }
}
