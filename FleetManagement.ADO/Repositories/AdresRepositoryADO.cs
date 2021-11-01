using FleetManagement.ADO.RepositoryExceptions;
using FleetManagement.Interfaces;
using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM Adres WHERE straat=@straat AND nummer=@nummer AND postcode=@postcode AND gemeente=@gemeente";
            using (SqlCommand command = connection.CreateCommand()) {
                connection.Open();
                try {
                    command.Parameters.Add(new SqlParameter("@straat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@postalCode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@gemeente", SqlDbType.NVarChar));

                    command.Parameters["@straat"].Value = adres.Straat;
                    command.Parameters["@nummer"].Value = adres.Nr;
                    command.Parameters["@postcode"].Value = adres.Postcode;
                    command.Parameters["@gemeente"].Value = adres.Gemeente;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new AdresRepositoryADOException("BestaatAdres- gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }

        public bool BestaatAdres(int adresId) {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM Adres WHERE adresId=@adresId";
            using (SqlCommand command = connection.CreateCommand()) {
                connection.Open();
                try {
                    command.Parameters.Add(new SqlParameter("@adresId", SqlDbType.Int));

                    command.Parameters["@adresId"].Value = adresId;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new AdresRepositoryADOException("BestaatAdres(int-id)- gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
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
