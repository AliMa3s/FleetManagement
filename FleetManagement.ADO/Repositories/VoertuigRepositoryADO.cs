using FleetManagement.Interfaces;
using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.Repositories {
    public class VoertuigRepositoryADO : IVoertuigRepository {

        private string connectionString = @"YourConnectionStringhere";
        public VoertuigRepositoryADO(string connectionstring) {
            this.connectionString = connectionstring;
        }

        private SqlConnection getConnection() {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public bool BestaatVoertuig(Voertuig voertuig) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Voertuig> GeefAlleVoerTuig() {
            throw new NotImplementedException();
        }

        public Voertuig GetVoertuig(int voertuigId) {
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

        public bool BestaatVoertuig(Voertuig voertuig, string chasisnummer, string nummerplaat) {
            throw new NotImplementedException();
        }
    }
}
