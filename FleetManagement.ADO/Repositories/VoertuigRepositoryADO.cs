using FleetManagement.ADO.RepositoryExceptions;
using FleetManagement.Filters;
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


        public bool BestaatVoertuig(Voertuig voertuig)
        {
            string query = "SELECT count(*) FROM Voertuig WHERE voertuigid=@voertuigid";
            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@voertuigid", SqlDbType.NVarChar));

                    command.Parameters["@voertuigid"].Value = voertuig.VoertuigId;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("BestaatVoertuig - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        //checks voordat een post kan plaatsvinden
        public bool BestaatChassisnummer(string chassisnummer)
        {
            string query = "SELECT count(*) FROM Voertuig WHERE chassisnummer=@chassisnummer"; 
            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@chassisnummer", SqlDbType.NVarChar));

                    command.Parameters["@chassisnummer"].Value = chassisnummer;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("Op plaatnummer zoeken - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public bool BestaatNummerplaat(string nummerplaat)
        {
            string query = "SELECT count(*) FROM Voertuig WHERE nummerplaat=@nummerplaat";
            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.NVarChar));

                    command.Parameters["@nummerplaat"].Value = nummerplaat;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("Op plaatnummer zoeken - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public Voertuig ZoekOpChassisNummer(string chassisnummer)
        {
            string query = "SELECT * FROM Voertuig v" +
                "JOIN AutoModel a ON v.automodelid" +
                "JOIN Brandstoftype br ON v.brandstoftypeid = br.brandstofid" +
                "WHERE v.chassisnummer=@chassisnummer";

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@chassisnummer", chassisnummer);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            //Maak AutoModeL
                            AutoModel autoModelDB = new(
                                (string)dataReader["automodelid"],
                                (string)dataReader["automodelnaam"],
                                new AutoType((string)dataReader["autotype"]) 
                            );

                            //Maak brandstof
                            BrandstofVoertuig brandstofVoertuigDB = new(
                                (int)dataReader["brandstofid"],
                                (string)dataReader["brandstofnaam"],
                                (bool)dataReader["hybride"]
                            );

                            //Maak voertuig
                            Voertuig voertuigDB = new(
                                   autoModelDB,
                                   (string)dataReader["chassisnummer"],
                                   (string)dataReader["nummerplaat"],
                                   brandstofVoertuigDB
                            );

                            //is kleur aanwezig
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("kleurnaam")))
                            {
                                voertuigDB.VoertuigKleur = new Kleur((string)dataReader["kleurnaam"]);
                            } 

                            //is aantal deuren aanwezig + casting naar enum
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("aantal_deuren")))
                            {
                                voertuigDB.AantalDeuren = Enum.IsDefined(typeof(AantalDeuren), (string)dataReader["aantal_deuren"])
                                    ? (AantalDeuren)Enum.Parse(typeof(AantalDeuren), (string)dataReader["aantal_deuren"])
                                    : throw new BrandstofRepositoryADOException("Aantal deuren - gefaald");
                            }

                            return voertuigDB;
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new BrandstofRepositoryADOException("Zoek of chassis - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }
    

        public Voertuig ZoekOpNummerplaat(string nummerplaat)
        {
            string query = "SELECT * FROM Voertuig v" +
                "JOIN AutoModel a ON v.automodelid" +
                "JOIN Brandstoftype br ON v.brandstoftypeid = br.brandstofid" +
                "WHERE v.nummerplaat=@nummerplaat";

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@nummerplaat", nummerplaat);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            //Maak AutoModeL
                            AutoModel autoModelDB = new(
                                (string)dataReader["automodelid"],
                                (string)dataReader["automodelnaam"],
                                new AutoType((string)dataReader["autotype"])
                            );

                            //Maak brandstof
                            BrandstofVoertuig brandstofVoertuigDB = new(
                                (int)dataReader["brandstofid"],
                                (string)dataReader["brandstofnaam"],
                                (bool)dataReader["hybride"]
                            );

                            //Maak voertuig
                            Voertuig voertuigDB = new(
                                   autoModelDB,
                                   (string)dataReader["chassisnummer"],
                                   (string)dataReader["nummerplaat"],
                                   brandstofVoertuigDB
                            );

                            //is kleur aanwezig
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("kleurnaam")))
                            {
                                voertuigDB.VoertuigKleur = new Kleur((string)dataReader["kleurnaam"]);
                            }

                            //is aantal deuren aanwezig + casting naar enum
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("aantal_deuren")))
                            {
                                voertuigDB.AantalDeuren = Enum.IsDefined(typeof(AantalDeuren), (string)dataReader["aantal_deuren"])
                                    ? (AantalDeuren)Enum.Parse(typeof(AantalDeuren), (string)dataReader["aantal_deuren"])
                                    : throw new BrandstofRepositoryADOException("Aantal deuren - gefaald");
                            }

                            return voertuigDB;
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new BrandstofRepositoryADOException("Zoek of nummerplaat - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }


        public IReadOnlyList<Voertuig> GeefAlleVoertuigenFilter(string autonaam)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Voertuig> GeefAlleVoertuigenFilter(string autonaam, Filter filter)
        {
            throw new NotImplementedException();
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

        public Voertuig VoegVoertuigToe(Voertuig voertuig) {

            string query = "INSERT INTO Voertuig (automodelid,brandstoftypeid,hybride,kleurnaam,aantal_deuren,chassisnummer,nummerplaat) " +
               " OUTPUT INSERTED.voertuigId VALUES (@automodelid,@brandstoftypeid,@hybride,@kleurnaam,@aantaldeuren,@chassisnummer,@nummerplaat)";

            Connection.Open();

            using (SqlCommand command = Connection.CreateCommand()) {
                try {

                    command.Parameters.Add(new SqlParameter("@automodelid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@brandstoftypeid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@hybride", SqlDbType.Bit));
                    command.Parameters.Add(new SqlParameter("@chassisnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.NVarChar));

                    command.Parameters.Add(new SqlParameter("@aantaldeuren", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@kleurnaam", SqlDbType.NVarChar));

                    if (voertuig.VoertuigKleur == null)
                    {
                        command.Parameters["@kleurnaam"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@kleurnaam"].Value = voertuig.VoertuigKleur.KleurNaam;
                    }

                    if (voertuig.AantalDeuren.HasValue)
                    {
                        command.Parameters["@aantaldeuren"].Value = voertuig.AantalDeuren.Value.ToString();
                    }
                    else
                    {
                        command.Parameters["@aantaldeuren"].Value = DBNull.Value;
                    }

                    command.Parameters["@automodelid"].Value = voertuig.AutoModel.AutoModelId;
                    command.Parameters["@brandstoftypeid"].Value = voertuig.Brandstof.BrandstofTypeId;
                    command.Parameters["@hybride"].Value = voertuig.Brandstof.Hybride;
                    command.Parameters["@chassisnummer"].Value = voertuig.ChassisNummer;
                    command.Parameters["@nummerplaat"].Value = voertuig.NummerPlaat;

                    command.CommandText = query;

                    int newID = (int)command.ExecuteScalar();
                    voertuig.VoegIdToe(newID);
                    return voertuig;

                } catch (Exception ex) {
                    throw new VoertuigRepositoryADOException("VoegVoertuig - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }
    }
}
