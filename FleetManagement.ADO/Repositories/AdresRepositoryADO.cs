using FleetManagement.Interfaces;
using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.Repositories {
    public class AdresRepositoryADO : IAdresRepository {

        private string connectionString = @"YourConnectionStringhere";
        public AdresRepositoryADO(string connectionstring) {
            this.connectionString = connectionstring;
        }

        private SqlConnection getConnection() {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public bool BestaatAdres(Adres adres) {
            throw new NotImplementedException();
        }

        public bool BestaatAdres(int adresId) {
            throw new NotImplementedException();
        }

        public void UpdateAdres(Adres adres) {
            throw new NotImplementedException();
        }

        public void VerwijderAders(Adres adres) {
            throw new NotImplementedException();
        }

        public void VoegAdresToe(Adres adres) {
            throw new NotImplementedException();
        }
    }
}
