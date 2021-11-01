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
            string query = "SELECT * FROM Bestuurder";
            List<Bestuurder> bestuurderLijst = new List<Bestuurder>();
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                try {
                    connection.Open();
                    IDataReader dataReader = command.ExecuteReader();
                    Bestuurder b = null;
                    while (dataReader.Read()) {
                        if (b == null) b = new Bestuurder((int)dataReader["bestuurderid"], (string)dataReader["voornaam"], (string)dataReader["achternaam"],
                        (string)dataReader["geboortedatum"], (string)dataReader["rijbewijstype"], (string)dataReader["rijbewijsnummer"],
                        (string)dataReader["rijksregisternummer"]);
                        bestuurderLijst.Add(b);
                    }
                    dataReader.Close();
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("GetAlleBestuurders niet gelukt", ex);
                } finally {
                    connection.Close();
                }
            }
            return bestuurderLijst.AsReadOnly();
        }

        public Bestuurder GetBestuurderId(int id) {
            string query = "SELECT * FROM Bestuurder WHERE bestuurderid=@bestuurderid";
            SqlConnection conn = getConnection();
            using (SqlCommand command = new SqlCommand(query, conn)) {
                try {
                    conn.Open();
                    command.Parameters.AddWithValue("@bestuurderid", id);
                    IDataReader dataReader = command.ExecuteReader();
                    dataReader.Read();
                    Bestuurder b = new Bestuurder((int)dataReader["bestuurderid"], (string)dataReader["voornaam"], (string)dataReader["achternaam"],
                        (string)dataReader["geboortedatum"], (string)dataReader["rijbewijstype"], (string)dataReader["rijbewijsnummer"],
                        (string)dataReader["rijksregisternummer"]);
                    dataReader.Close();
                    return b;
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("GetBestuurderid niet gefaald", ex);
                } finally {
                    conn.Close();
                }
            }
        }

        public void UpdateBestuurder(Bestuurder bestuurder) {
            SqlConnection connection = getConnection();

            string query = "UPDATE Bestuurder" +
                           "SET voornaam=@voornaam, achternaam=@achternaam, geboortedatum=@geboortedatum, rijksregisternummer=@rijksregisternummer," +
                           " rijbewijstype=@rijbewijstype, rijbewijsnummer=@rijbewijsnummer, aanmaakDatum=@aanmaakDatum " +
                           " WHERE bestuurderid=@bestuurderid";

            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@voornaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@achternaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geboortedatum", SqlDbType.Date));
                    command.Parameters.Add(new SqlParameter("@rijksregisternummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijstype", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijsnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@aanmaakDatum", SqlDbType.Timestamp));
                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));

                    command.Parameters["@voornaam"].Value = bestuurder.Voornaam;
                    command.Parameters["@achternaam"].Value = bestuurder.Achternaam;
                    command.Parameters["@geboortedatum"].Value = bestuurder.GeboorteDatum;
                    command.Parameters["@rijksregisternummer"].Value = bestuurder.RijksRegisterNummer;
                    command.Parameters["@rijbewijstype"].Value = bestuurder.TypeRijbewijs;
                    command.Parameters["@rijbewijsnummer"].Value = bestuurder.RijBewijsNummer;
                    command.Parameters["@aanmaakDatum"].Value = bestuurder.AanMaakDatum;
                    command.Parameters["@bestuurderid"].Value = bestuurder.BestuurderId;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("UpdateBestuurder - gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }

        public void VerwijderBestuurder(Bestuurder bestuurder) {
            SqlConnection connection = getConnection();
            string query = "DELETE FROM Bestuurder WHERE bestuurderid=@bestuurderid";

            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));
                    command.Parameters["@bestuurderid"].Value = bestuurder.BestuurderId;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("Verwijderbestuurder - gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }

        public void VoegBestuurderToe(Bestuurder bestuurder) {
            SqlConnection connection = getConnection();

            string query = "INSERT INTO Bestuurder (voornaam, achternaam, geboortedatum, rijksregisternummer,rijbewijstype,rijbewijsnummer,aanmaakDatum)" +
                           "VALUES (@voornaam, @achternaam, @geboortedatum, @rijksregisternummer, @rijbewijstype, @rijbewijsnummer, @aanmaakDatum)";

            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@voornaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@achternaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geboortedatum", SqlDbType.Date));
                    command.Parameters.Add(new SqlParameter("@rijksregisternummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijstype", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijsnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@aanmaakDatum", SqlDbType.Timestamp));

                    command.Parameters["@voornaam"].Value = bestuurder.Voornaam;
                    command.Parameters["@achternaam"].Value = bestuurder.Achternaam;
                    command.Parameters["@geboortedatum"].Value = bestuurder.GeboorteDatum;
                    command.Parameters["@rijksregisternummer"].Value = bestuurder.RijksRegisterNummer;
                    command.Parameters["@rijbewijstype"].Value = bestuurder.TypeRijbewijs;
                    command.Parameters["@rijbewijsnummer"].Value = bestuurder.RijBewijsNummer;
                    command.Parameters["@aanmaakDatum"].Value = bestuurder.AanMaakDatum;

                    command.CommandText = query;
                    command.ExecuteNonQuery();

                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("VoegBestuurderToe(bestuurder)- gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
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
