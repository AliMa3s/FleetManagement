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
    public class VoertuigRepositoryADO : RepoConnection, IVoertuigRepository {

        public VoertuigRepositoryADO(string connectionstring) : base(connectionstring) { }

        public bool BestaatVoertuig(Voertuig voertuig) {

            string query = "SELECT count(*) FROM Voertuig WHERE aantal_deuren=@aantal_deuren AND chassisnummer=@chassisnummer AND nummerplaat=@nummerplaat ";
            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@aantal_deuren", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@chassisnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.NVarChar));
                    //command.Parameters.Add(new SqlParameter("@inboekdatum", SqlDbType.Timestamp));

                    command.Parameters["@aantal_deuren"].Value = voertuig.AantalDeuren;
                    command.Parameters["@chassisnummer"].Value = voertuig.ChassisNummer;
                    command.Parameters["@nummerplaat"].Value = voertuig.NummerPlaat;
                    //command.Parameters["@inboekdatum"].Value = voertuig.InBoekDatum;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new VoertuigRepositoryADOException("BestaatVoertuig - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public IReadOnlyList<Voertuig> GeefAlleVoerTuig() {
            throw new NotImplementedException();
        }

        public Voertuig GetVoertuig(int voertuigid) {
            string query = "SELECT * FROM Voertuig WHERE voertuigid=@voertuigid";

            using (SqlCommand command = new SqlCommand(query, Connection)) {
                try {
                    Connection.Open();
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
                    Connection.Close();
                }
            }
        }

        public void UpdateVoertuig(Voertuig voertuig) {

            string query = "UPDATE Voertuig " +
                           " SET aantal_deuren=@aantal_deuren, chassisnummer=@chassisnummer, nummerplaat=@nummerplaat, " +
                           " WHERE voertuigid=@voertuigid";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();

                    command.Parameters.Add(new SqlParameter("@aantal_deuren", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@chassisnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.NVarChar));
                    //command.Parameters.Add(new SqlParameter("@inboekdatum", SqlDbType.Timestamp));
                    command.Parameters.Add(new SqlParameter("@voertuigid", SqlDbType.Int));

                    command.Parameters["@aantal_deuren"].Value = voertuig.AantalDeuren;
                    command.Parameters["@chassisnummer"].Value = voertuig.ChassisNummer;
                    command.Parameters["@nummerplaat"].Value = voertuig.NummerPlaat;
                    //command.Parameters["@inboekdatum"].Value = voertuig.InBoekDatum;
                    command.Parameters["@voertuigid"].Value = voertuig.VoertuigId;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("UpdateBestuurder - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public void VerwijderVoertuig(Voertuig voertuig) {

            string query = "DELETE FROM Voertuig WHERE voertuigid=@voertuigid";

            using (SqlCommand command = new SqlCommand(query, Connection)) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@voertuigid", SqlDbType.Int));
                    command.Parameters["@voertuigid"].Value = voertuig.VoertuigId;
                    //command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("VerwijderVoertuig - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        /* 
         * Idee: We zouden OUTPUT INSERTED.voertuigId kunnen toevoegen 
         * We krijgen dan de nieuwe VoertuigId terug
        */
        public void VoegVoertuigToe(Voertuig voertuig) {

            string query = "INSERT INTO Voertuig (automodelid,brandstoftypeid,aantal_deuren,chassisnummer,nummerplaat) " +
                           " VALUES (@automodelid,@brandstoftypeid,@aantal_deuren,@chassisnummer,@nummerplaat)";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.AddWithValue("@automodelid", voertuig.AutoModel.AutoModelId);
                    command.Parameters.AddWithValue("@brandstoftypeid", voertuig.BrandstofType.BrandstofTypeId);
                    command.Parameters.AddWithValue("@aantal_deuren", voertuig.AantalDeuren);
                    command.Parameters.AddWithValue("@chassisnummer", voertuig.ChassisNummer);
                    command.Parameters.AddWithValue("@nummerplaat", voertuig.NummerPlaat);


                    command.CommandText = query;
                    command.ExecuteNonQuery();
                    
                } catch (Exception ex) {
                    throw new VoertuigRepositoryADOException("VoegVoertuig - gefaald", ex);
                } finally {
                    Connection.Close();
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

            string query = "SELECT count(*) FROM Voertuig WHERE chassisnummer=@chassisnummer AND nummerplaat=@nummerplaat " +
                " AND voertuigid=@voertuigid";
            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
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
                    Connection.Close();
                }
            }
        }

        //Versie toegevoegd filip
        //Vergeet niet: DB => Voertuigen (en niet Voertuig)
        public bool bestaatChassisOfNummerplaat(string chassisNummer, string nummerPlaat) {
            string queryVoertuig = "SELECT chassisnummer, nummperplaat FROM voertuigen " +
                " WHERE chassisnummer = @chassisNummer || nummerplaat = @nummerPlaat";

            using (SqlCommand command = new(queryVoertuig, Connection)) {
                try {
                    command.Parameters.AddWithValue("@chassisNummer", chassisNummer);
                    command.Parameters.AddWithValue("@nummerPlaat", nummerPlaat);

                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader()) {
                        if (dataReader.HasRows) {
                            return true;
                        }

                        
                        return false;
                    }
                } catch (Exception ex) {
                    throw new VoertuigRepositoryADOException("Bestaat Voertruig op chassisnummer & plaatnummer - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }
    }
}
