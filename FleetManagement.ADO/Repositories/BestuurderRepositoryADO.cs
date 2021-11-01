using FleetManagement.ADO.RepositoryExceptions;
using FleetManagement.Helpers;
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
    public class BestuurderRepositoryADO : IBestuurderRepository {


        private string connectionString = @"YourConnectionStringhere";
        public BestuurderRepositoryADO(string connectionstring) {
            this.connectionString = connectionstring;
        }

        private SqlConnection getConnection() {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public bool BestaatBestuurder(int id) {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM Bestuurder WHERE bestuurderid=@bestuurderid";
            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));

                    command.Parameters["@bestuurderid"].Value = id;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new AdresRepositoryADOException("BestaatBestuurder(int-id)- gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }

        public bool BestaatRijksRegisterNummer(string rijksRegisterNr) {
            string query = $"SELECT * FROM Bestuurder WHERE rijksregisternummer=@rijksregisternummer;";
            SqlConnection connection = getConnection();
            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@rijksregisternummer", SqlDbType.NVarChar));

                    command.Parameters["@rijksregisternummer"].Value = rijksRegisterNr;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new AdresRepositoryADOException("BestaatRijksRegisterNummer - gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }

        public IReadOnlyList<Bestuurder> GeefAlleBestuurder() {
            throw new NotImplementedException();
        }

        public Bestuurder GetBestuurderId(int id) {
            throw new NotImplementedException();
        }

        public void UpdateBestuurder(Bestuurder bestuurder) {
            throw new NotImplementedException();
        }

        public void VerwijderBestuurder(Bestuurder bestuurder) {
            throw new NotImplementedException();
        }

        public void VoegBestuurderToe(Bestuurder bestuurder) {
            throw new NotImplementedException();
        }

        public Bestuurder ZoekBestuurder(int bestuurderid) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Bestuurder> ZoekBestuurders(int? id, string voornaam, string achternaam, string geboortedatum, Adres adres) {
            throw new NotImplementedException();
        }


        //bezig idee uitwerking Filip
        public Bestuurder ZoekBestuurder(string RijksRegisterNummer) {
            throw new NotImplementedException();
        }

        public PaginaLijst<Bestuurder> FilterBestuurders(string voornaam, string achternaam) {
            throw new NotImplementedException();
        }

        public PaginaLijst<Bestuurder> AlleBestuurders(SorteerOptie sorteer) {
            throw new NotImplementedException();
        }

        public PaginaLijst<Bestuurder> BestuurdersZonderVoertuig() {
            throw new NotImplementedException();
        }
    }
}
