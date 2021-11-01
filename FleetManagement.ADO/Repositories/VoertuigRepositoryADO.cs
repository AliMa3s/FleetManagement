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
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM Voertuig WHERE aantal_deuren=@aantal_deuren AND chassisnummer=@chassisnummer AND nummerplaat=@nummerplaat " +
                " AND inboekdatum=@inboekdatum";
            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@aantal_deuren", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@chassisnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@inboekdatum", SqlDbType.Timestamp));

                    command.Parameters["@aantal_deuren"].Value = voertuig.AantalDeuren;
                    command.Parameters["@chassisnummer"].Value = voertuig.ChassisNummer;
                    command.Parameters["@nummerplaat"].Value = voertuig.NummerPlaat;
                    command.Parameters["@inboekdatum"].Value = voertuig.InBoekDatum;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new VoertuigRepositoryADOException("BestaatVoertuig - gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }

        public IReadOnlyList<Voertuig> GeefAlleVoerTuig() {
            throw new NotImplementedException();
        }

        public Voertuig GetVoertuig(int voertuigid) {
            string query = "SELECT * FROM Voertuig WHERE voertuigid=@voertuigid";
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                try {
                    connection.Open();
                    command.Parameters.AddWithValue("@voertuigid", voertuigid);
                    IDataReader dataReader = command.ExecuteReader();
                    dataReader.Read();
                    Voertuig v = new Voertuig((int)dataReader["voertuigid"], (AutoModel)dataReader["automodelid"], (string)dataReader["chassisnummer"],
                        (string)dataReader["nummerplaat"], (BrandstofVoertuig)dataReader["brandstoftypeid"]);
                    dataReader.Close();
                    return v;
                } catch (Exception ex) {
                    throw new VoertuigRepositoryADOException("GetVoertuig - gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }

        public void UpdateVoertuig(Voertuig voertuig) {
            SqlConnection connection = getConnection();

            string query = "UPDATE Voertuig" +
                           "SET aantal_deuren=@aantal_deuren, chassisnummer=@chassisnummer, nummerplaat=@nummerplaat, inboekdatum=@inboekdatum, " +
                           " WHERE voertuigid=@voertuigid";

            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@aantal_deuren", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@chassisnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@inboekdatum", SqlDbType.Timestamp));
                    command.Parameters.Add(new SqlParameter("@voertuigid", SqlDbType.Int));

                    command.Parameters["@aantal_deuren"].Value = voertuig.AantalDeuren;
                    command.Parameters["@chassisnummer"].Value = voertuig.ChassisNummer;
                    command.Parameters["@nummerplaat"].Value = voertuig.NummerPlaat;
                    command.Parameters["@inboekdatum"].Value = voertuig.InBoekDatum;
                    command.Parameters["@voertuigid"].Value = voertuig.VoertuigId;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("UpdateBestuurder - gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }

        public void VerwijderVoertuig(Voertuig voertuig) {
            SqlConnection connection = getConnection();
            string query = "DELETE FROM Voertuig WHERE voertuigid=@voertuigid";

            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@voertuigid", SqlDbType.Int));
                    command.Parameters["@voertuigid"].Value = voertuig.VoertuigId;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("VerwijderVoertuig - gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }

        public void VoegVoertuigToe(Voertuig voertuig) {
            SqlConnection connection = getConnection();

            string query = "INSERT INTO Voertuig (aantal_deuren,chassisnummer,nummerplaat,inboekdatum)" +
                           "VALUES (@aantal_deuren,@chassisnummer,@nummerplaat,@inboekdatum)";

            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@aantal_deuren", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@chassisnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@inboekdatum", SqlDbType.Timestamp));

                    command.Parameters["@aantal_deuren"].Value = voertuig.AantalDeuren;
                    command.Parameters["@chassisnummer"].Value = voertuig.ChassisNummer;
                    command.Parameters["@nummerplaat"].Value = voertuig.NummerPlaat;
                    command.Parameters["@inboekdatum"].Value = voertuig.InBoekDatum;

                    command.CommandText = query;
                    command.ExecuteNonQuery();

                } catch (Exception ex) {
                    throw new VoertuigRepositoryADOException("VoegVoertuig - gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }

        public Voertuig ZoekVoertuig(int? voertuigId, AutoModel automodel, string chassisNumber, string nummerPlaat, BrandstofType brandstof, Kleur kleur, AantalDeuren aantalDeuren, Bestuurder bestuurder) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Voertuig> ZoekVoertuigen(int? voertuigId, AutoModel automodel, string chassisNumber, string nummerPlaat, BrandstofType brandstof, Kleur kleur, AantalDeuren aantalDeuren, Bestuurder bestuurder) {
            throw new NotImplementedException();
        }

        public bool BestaatVoertuig(Voertuig voertuig, string chasisnummer, string nummerplaat) {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM Voertuig WHERE chassisnummer=@chassisnummer AND nummerplaat=@nummerplaat " +
                " AND voertuigid=@voertuigid";
            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@chassisnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@voertuigid", SqlDbType.Int));

                    command.Parameters["@chassisnummer"].Value = voertuig.ChassisNummer;
                    command.Parameters["@nummerplaat"].Value = voertuig.NummerPlaat;
                    command.Parameters["@voertuigid"].Value = voertuig.VoertuigId;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new VoertuigRepositoryADOException("BestaatVoertuig - gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }
    }
}
