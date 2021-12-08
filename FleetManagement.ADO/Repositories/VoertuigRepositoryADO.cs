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

        public VoertuigRepositoryADO(string connectionstring) : base(connectionstring)  { }

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
                    throw new TankkaartRepositoryADOException("Op chassisnummer zoeken - gefaald", ex);
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
                    throw new TankkaartRepositoryADOException("Op nummerplaat zoeken - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public Voertuig ZoekOpNummerplaatOfChassisNummer(string NummerplaatOfChassis)
        {
            string query = "SELECT * FROM Voertuig v " +
                "JOIN AutoModel a ON v.automodelid = a.automodelid " +
                "JOIN Brandstoftype br ON v.brandstoftypeid = br.brandstoftypeid " +
                "LEFT JOIN Bestuurder b ON v.voertuigid = b.voertuigid " +
                "LEFT JOIN adres ad ON b.adresId = ad.adresId " +
                "WHERE v.nummerplaat=@NummerplaatOfChassis OR v.chassisnummer=@NummerplaatOfChassis " +
                "ORDER BY a.automodelnaam ASC, a.merknaam ASC " +
                "OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@NummerplaatOfChassis", NummerplaatOfChassis);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();

                            //Instantieer AutoModeL
                            AutoModel autoModelDB = new(
                                (int)dataReader["automodelid"],
                                (string)dataReader["merknaam"],
                                (string)dataReader["automodelnaam"],
                                new AutoType((string)dataReader["autotype"])
                            );

                            //Instantieer brandstof
                            BrandstofVoertuig brandstofVoertuigDB = new(
                                (int)dataReader["brandstoftypeid"],
                                (string)dataReader["brandstofnaam"],
                                (bool)dataReader["hybride"]
                            );

                            //Instantieer voertuig
                            Voertuig voertuigDB = new(
                                    (int)dataReader["Voertuigid"],
                                    autoModelDB,
                                    (string)dataReader["chassisnummer"],
                                    (string)dataReader["nummerplaat"],
                                    brandstofVoertuigDB
                            );

                            //is kleur aanwezig
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("kleurnaam")))
                            {
                                voertuigDB.VoertuigKleur = new Kleur(
                                    (string)dataReader["kleurnaam"]
                                );
                            }

                            //is aantal deuren aanwezig + casting naar enum
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("aantal_deuren")))
                            {
                                voertuigDB.AantalDeuren = Enum.IsDefined(typeof(AantalDeuren), (string)dataReader["aantal_deuren"])
                                    ? (AantalDeuren)Enum.Parse(typeof(AantalDeuren), (string)dataReader["aantal_deuren"])
                                    : throw new BrandstofRepositoryADOException("Aantal deuren - gefaald");
                            }

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("bestuurderid")))
                            { 
                                Bestuurder bestuurderDB = new(
                                        (int)dataReader["bestuurderid"],
                                        (string)dataReader["voornaam"],
                                        (string)dataReader["achternaam"],
                                        (string)dataReader["geboortedatum"],
                                        (string)dataReader["rijbewijstype"],
                                        (string)dataReader["rijksregisternummer"]
                                    );

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("adresId")))
                                {
                                    Adres adresDB = new(
                                        (string)dataReader["straat"],
                                        (string)dataReader["nummer"],
                                        (string)dataReader["postcode"],
                                        (string)dataReader["gemeente"]
                                    );
                                    adresDB.VoegIdToe((int)dataReader["adresId"]);
                                    bestuurderDB.Adres = adresDB;
                                }

                                voertuigDB.VoegBestuurderToe(bestuurderDB);
                            }

                            return voertuigDB;
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new BrandstofRepositoryADOException("Zoek op chassisnummer of nummerplaat - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public IReadOnlyList<Voertuig> GeefAlleVoertuigenFilter(string autonaam, Filter filter)
        {
            string queryHybride = "";
            if(filter.Hybride)
            {
                queryHybride = $"AND v.hybride = 1 ";
            }

            string queryKleur = "";
            if(filter.Kleuren.Count > 0)
            {
                string kleur = "";
                for (int i = 1; i <= filter.Kleuren.Count; i++)
                {
                    kleur += "@kleur" + i + ", ";  //"@kleur1, @kleur2, etc.."
                }

                queryKleur = $"AND v.kleurnaam IN({kleur[0..^2]}) ";
            }

            
            string queryAutoType = "";
            if (filter.AutoTypes.Count > 0)
            {
                string autotype = "";
                for (int i = 1; i <= filter.AutoTypes.Count; i++)
                {
                    autotype += "@autotype" + i + ", ";  //"@autotype1, @autotype2, etc.."
                }

                queryAutoType = $"AND a.autotype IN({autotype[0..^2]}) ";
            }

            
            string queryBrandstof = "";
            if (filter.Brandstoffen.Count > 0)
            {
                string brandstof = "";
                for (int i = 1; i <= filter.Brandstoffen.Count; i++)
                {
                    brandstof += "@brandstof" + i + ", ";  //"@brandstof1, @brandstof2, etc.."
                }

                queryBrandstof = $"AND br.brandstofnaam IN({brandstof[0..^2]}) ";
            }

            string query = "SELECT * FROM Voertuig v " +
                 "JOIN AutoModel a ON v.automodelid = a.automodelid " +
                 "JOIN Brandstoftype br ON v.brandstoftypeid = br.brandstoftypeid " +
                 "LEFT JOIN Bestuurder b ON v.voertuigid = b.voertuigid " +
                 "LEFT JOIN adres ad ON b.adresId = ad.adresId " +
                 "WHERE concat(a.merknaam, ' ', a.automodelnaam) LIKE @autonaam + '%' " +
                 $"{queryHybride}" +
                 $"{queryKleur}" +
                 $"{queryAutoType}" +
                 $"{queryBrandstof}" +
                 "ORDER BY a.automodelnaam ASC, a.merknaam ASC " +
                 "OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@autonaam", autonaam);
           
                    if(filter.Kleuren.Count > 0)
                    {
                        int i = 1;  
                        filter.Kleuren.ForEach(kleur =>
                        {
                            _ = command.Parameters.AddWithValue("@kleur" + i, kleur);
                            i++;
                        });
                    }

                    if (filter.AutoTypes.Count > 0)
                    {
                        int i = 1;
                        filter.AutoTypes.ForEach(autotype =>
                        {
                            _ = command.Parameters.AddWithValue("@autotype" + i, autotype);
                            i++;
                        });
                    }

                    if (filter.Brandstoffen.Count > 0)
                    {
                        int i = 1;
                        filter.Brandstoffen.ForEach(brandstof =>
                        {
                            _ = command.Parameters.AddWithValue("@brandstof" + i, brandstof);
                            i++;
                        });
                    }

                    Connection.Open();

                    List<Voertuig> voertuigenDB = new();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                //Instantieer AutoModeL
                                AutoModel autoModelDB = new(
                                    (int)dataReader["automodelid"],
                                    (string)dataReader["merknaam"],
                                    (string)dataReader["automodelnaam"],
                                    new AutoType((string)dataReader["autotype"])
                                );

                                //Instantieer brandstof
                                BrandstofVoertuig brandstofVoertuigDB = new(
                                    (int)dataReader["brandstoftypeid"],
                                    (string)dataReader["brandstofnaam"],
                                    (bool)dataReader["hybride"]
                                );

                                //Instantieer voertuig
                                Voertuig voertuigDB = new(
                                        (int)dataReader["Voertuigid"],
                                        autoModelDB,
                                        (string)dataReader["chassisnummer"],
                                        (string)dataReader["nummerplaat"],
                                        brandstofVoertuigDB
                                );

                                //is kleur aanwezig
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("kleurnaam")))
                                {
                                    voertuigDB.VoertuigKleur = new Kleur(
                                        (string)dataReader["kleurnaam"]
                                    );
                                }

                                //is aantal deuren aanwezig + casting naar enum
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("aantal_deuren")))
                                {
                                    voertuigDB.AantalDeuren = Enum.IsDefined(typeof(AantalDeuren), (string)dataReader["aantal_deuren"])
                                        ? (AantalDeuren)Enum.Parse(typeof(AantalDeuren), (string)dataReader["aantal_deuren"])
                                        : throw new BrandstofRepositoryADOException("Aantal deuren - gefaald");
                                }

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("bestuurderid")))
                                {
                                    Bestuurder bestuurderDB = new(
                                            (int)dataReader["bestuurderid"],
                                            (string)dataReader["voornaam"],
                                            (string)dataReader["achternaam"],
                                            (string)dataReader["geboortedatum"],
                                            (string)dataReader["rijbewijstype"],
                                            (string)dataReader["rijksregisternummer"]
                                        );

                                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("adresId")))
                                    {
                                        Adres adresDB = new(
                                            (string)dataReader["straat"],
                                            (string)dataReader["nummer"],
                                            (string)dataReader["postcode"],
                                            (string)dataReader["gemeente"]
                                        );
                                        adresDB.VoegIdToe((int)dataReader["adresId"]);
                                        bestuurderDB.Adres = adresDB;
                                    }

                                    voertuigDB.VoegBestuurderToe(bestuurderDB);
                                }

                                voertuigenDB.Add(voertuigDB);
                            };
                        }

                        return voertuigenDB;
                    }
                }
                catch (Exception ex)
                {
                    throw new BrandstofRepositoryADOException("Geef Alle Voertuigen met filter - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        //Update voertuig 
        public void UpdateVoertuig(Voertuig voertuig) {

            string query = "UPDATE Voertuig " +
                           "SET automodelid=@automodelid, brandstoftypeid=@brandstoftypeid, hybride=@hybride, kleurnaam=kleurnaam, " +
                           "aantal_deuren=@aantal_deuren, inboekdatum=@inboekdatum " +
                           "WHERE voertuigid=@voertuigid";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();

                    command.Parameters.Add(new SqlParameter("@voertuigid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@automodelid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@brandstoftypeid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@hybride", SqlDbType.Bit));
                    command.Parameters.Add(new SqlParameter("@kleurnaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@aantaldeuren", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@inboekdatum", SqlDbType.NVarChar));

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

                    if (voertuig.InBoekDatum.HasValue)
                    {
                        command.Parameters["@inboekdatum"].Value = voertuig.InBoekDatum.Value;
                    }
                    else
                    {
                        command.Parameters["@inboekdatum"].Value = DBNull.Value;
                    }

                    command.Parameters["@voertuigid"].Value = voertuig.VoertuigId;
                    command.Parameters["@automodelid"].Value = voertuig.AutoModel.AutoModelId;
                    command.Parameters["@brandstoftypeid"].Value = voertuig.Brandstof.BrandstofTypeId;
                    command.Parameters["@hybride"].Value = voertuig.Brandstof.Hybride;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new VoertuigRepositoryADOException("UpdateVoertuig - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        //Update met ander chassis en nummerplaat
        public void UpdateVoertuig(Voertuig voertuig, string anderChassisnummer, string anderNummerplaat)
        {
            string query = "UPDATE Voertuig " +
                           "SET automodelid=@automodelid, brandstoftypeid=@brandstoftypeid, hybride=@hybride, kleurnaam=kleurnaam, " +
                           "chassisnummer=@chassisnummer, nummerplaat=@nummerplaat, aantal_deuren=@aantal_deuren, inboekdatum=@inboekdatum " +
                           "WHERE voertuigid=@voertuigid";

            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();

                    command.Parameters.Add(new SqlParameter("@voertuigid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@automodelid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@brandstoftypeid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@hybride", SqlDbType.Bit));
                    command.Parameters.Add(new SqlParameter("@chassisnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@kleurnaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@aantaldeuren", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@inboekdatum", SqlDbType.NVarChar));

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

                    if (voertuig.InBoekDatum.HasValue)
                    {
                        command.Parameters["@inboekdatum"].Value = voertuig.InBoekDatum.Value;
                    }
                    else
                    {
                        command.Parameters["@inboekdatum"].Value = DBNull.Value;
                    }

                    command.Parameters["@automodelid"].Value = voertuig.VoertuigId;
                    command.Parameters["@automodelid"].Value = voertuig.AutoModel.AutoModelId;
                    command.Parameters["@brandstoftypeid"].Value = voertuig.Brandstof.BrandstofTypeId;
                    command.Parameters["@hybride"].Value = voertuig.Brandstof.Hybride;
                    command.Parameters["@chassisnummer"].Value = anderChassisnummer;
                    command.Parameters["@nummerplaat"].Value = anderNummerplaat;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new VoertuigRepositoryADOException("UpdateVoertuig - gefaald", ex);
                }
                finally
                {
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
                    throw new VoertuigRepositoryADOException("VerwijderVoertuig - gefaald", ex);
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
                    command.Parameters.Add(new SqlParameter("@kleurnaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@aantaldeuren", SqlDbType.NVarChar));

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

        public IReadOnlyList<Voertuig> SelecteerZonderBestuurderFilter(string autonaam)
        {
            string query = "SELECT * FROM Voertuig v " +
                 "LEFT JOIN Bestuurder b ON b.voertuigid = v.voertuigid " +
                 "JOIN AutoModel a ON v.automodelid = a.automodelid " +
                 "JOIN Brandstoftype br ON v.brandstoftypeid = br.brandstoftypeid " +
                 "WHERE concat(a.merknaam, ' ', a.automodelnaam) LIKE @autonaam +'%' " +
                 "AND b.voertuigid IS NULL " +
                 "ORDER BY v.chassisnummer ASC " +
                 "OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@autonaam", autonaam);
                    Connection.Open();

                    List<Voertuig> voertuigenDB = new();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                //Instantieer AutoModeL
                                AutoModel autoModelDB = new(
                                    (int)dataReader["automodelid"],
                                    (string)dataReader["merknaam"],
                                    (string)dataReader["automodelnaam"],
                                    new AutoType((string)dataReader["autotype"])
                                );

                                //Instantieer brandstof
                                BrandstofVoertuig brandstofVoertuigDB = new(
                                    (int)dataReader["brandstoftypeid"],
                                    (string)dataReader["brandstofnaam"],
                                    (bool)dataReader["hybride"]
                                );

                                //Instantieer voertuig
                                Voertuig voertuigDB = new(
                                        (int)dataReader["Voertuigid"],
                                        autoModelDB,
                                        (string)dataReader["chassisnummer"],
                                        (string)dataReader["nummerplaat"],
                                        brandstofVoertuigDB
                                );

                                //is kleur aanwezig
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("kleurnaam")))
                                {
                                    voertuigDB.VoertuigKleur = new Kleur(
                                        (string)dataReader["kleurnaam"]
                                    );
                                }

                                //is aantal deuren aanwezig + casting naar enum
                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("aantal_deuren")))
                                {
                                    voertuigDB.AantalDeuren = Enum.IsDefined(typeof(AantalDeuren), (string)dataReader["aantal_deuren"])
                                        ? (AantalDeuren)Enum.Parse(typeof(AantalDeuren), (string)dataReader["aantal_deuren"])
                                        : throw new AantalDeurenRepositoryException("Aantal deuren - gefaald");
                                }

                                voertuigenDB.Add(voertuigDB);
                            };
                        }

                        return voertuigenDB;
                    }
                }
                catch (Exception ex)
                {
                    throw new BrandstofRepositoryADOException("Selecteer alle voertuigen zonder bestuurder - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }
    }
}
