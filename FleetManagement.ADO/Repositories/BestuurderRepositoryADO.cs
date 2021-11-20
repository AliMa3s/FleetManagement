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
    public class BestuurderRepositoryADO : RepoConnection, IBestuurderRepository
    {
        public BestuurderRepositoryADO(string connectionstring) : base(connectionstring) { }

        public bool BestaatBestuurder(int id) {

            string query = "SELECT count(*) FROM Bestuurder WHERE bestuurderid=@bestuurderid";
            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));

                    command.Parameters["@bestuurderid"].Value = id;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new AdresRepositoryADOException("BestaatBestuurder(int-id)- gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public bool BestaatRijksRegisterNummer(string rijksRegisterNr) {

            string query = $"SELECT * FROM Bestuurder WHERE rijksregisternummer=@rijksregisternummer;";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@rijksregisternummer", SqlDbType.NVarChar));

                    command.Parameters["@rijksregisternummer"].Value = rijksRegisterNr;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new AdresRepositoryADOException("BestaatRijksRegisterNummer - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public IReadOnlyList<Bestuurder> GeefAlleBestuurder() {

            string query = "SELECT * FROM Bestuurder";
            List<Bestuurder> bestuurderLijst = new List<Bestuurder>();

            using (SqlCommand command = new SqlCommand(query, Connection)) {
                try {
                    Connection.Open();
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
                    Connection.Close();
                }
            }
            return bestuurderLijst.AsReadOnly();
        }

        public Bestuurder GetBestuurderId(int id) {

            string query = "SELECT * FROM Bestuurder WHERE bestuurderid=@bestuurderid";

            using (SqlCommand command = new SqlCommand(query, Connection)) {
                try {
                    Connection.Open();
                    command.Parameters.AddWithValue("@bestuurderid", id);
                    IDataReader dataReader = command.ExecuteReader();
                    dataReader.Read();
                    Bestuurder b = new Bestuurder((int)dataReader["bestuurderid"], (string)dataReader["voornaam"], (string)dataReader["achternaam"],
                        (string)dataReader["geboortedatum"], (string)dataReader["rijbewijstype"], (string)dataReader["rijbewijsnummer"],
                        (string)dataReader["rijksregisternummer"]);
                    dataReader.Close();
                    return b;
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("GetBestuurderid - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public void UpdateBestuurder(Bestuurder bestuurder) {

            string query = "UPDATE Bestuurder" +
                           " SET voornaam=@voornaam, achternaam=@achternaam, geboortedatum=@geboortedatum, rijksregisternummer=@rijksregisternummer, " +
                           " rijbewijstype=@rijbewijstype, rijbewijsnummer=@rijbewijsnummer" +
                           " WHERE bestuurderid=@bestuurderid";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@voornaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@achternaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geboortedatum", SqlDbType.Date));
                    command.Parameters.Add(new SqlParameter("@rijksregisternummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijstype", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijsnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));

                    command.Parameters["@voornaam"].Value = bestuurder.Voornaam;
                    command.Parameters["@achternaam"].Value = bestuurder.Achternaam;
                    command.Parameters["@geboortedatum"].Value = bestuurder.GeboorteDatum;
                    command.Parameters["@rijksregisternummer"].Value = bestuurder.RijksRegisterNummer;
                    command.Parameters["@rijbewijstype"].Value = bestuurder.TypeRijbewijs;
                    command.Parameters["@rijbewijsnummer"].Value = bestuurder.RijBewijsNummer;
                    command.Parameters["@bestuurderid"].Value = bestuurder.BestuurderId;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("UpdateBestuurder - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public void VerwijderBestuurder(Bestuurder bestuurder) {

            string query = "DELETE FROM Bestuurder WHERE bestuurderid=@bestuurderid";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));
                    command.Parameters["@bestuurderid"].Value = bestuurder.BestuurderId;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("Verwijderbestuurder - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public void VoegBestuurderToe(Bestuurder bestuurder) {

            string query = "INSERT INTO Bestuurder (voornaam, achternaam, geboortedatum, rijksregisternummer,rijbewijstype,rijbewijsnummer)" +
                           "VALUES (@voornaam, @achternaam, @geboortedatum, @rijksregisternummer, @rijbewijstype, @rijbewijsnummer)";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@voornaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@achternaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geboortedatum", SqlDbType.Date));
                    command.Parameters.Add(new SqlParameter("@rijksregisternummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijstype", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijsnummer", SqlDbType.NVarChar));

                    command.Parameters["@voornaam"].Value = bestuurder.Voornaam;
                    command.Parameters["@achternaam"].Value = bestuurder.Achternaam;
                    command.Parameters["@geboortedatum"].Value = bestuurder.GeboorteDatum;
                    command.Parameters["@rijksregisternummer"].Value = bestuurder.RijksRegisterNummer;
                    command.Parameters["@rijbewijstype"].Value = bestuurder.TypeRijbewijs;
                    command.Parameters["@rijbewijsnummer"].Value = bestuurder.RijBewijsNummer;

                    command.CommandText = query;
                    command.ExecuteNonQuery();

                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("VoegBestuurderToe(bestuurder)- gefaald", ex);
                } finally {
                    Connection.Close();
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
        public Bestuurder ZoekBestuurder(string rijksRegisterNummer) {

            string queryBestuurder = "SELECT * FROM bestuurders b" +
                "LEFT JOIN adressen a ON b.adresId = a.adresId" +
                "LEFT JOIN voertuigen v ON b.bestuurderId = v.bestuurderId" +
                "LEFT JOIN automodellen a ON v.autoModelId = a.autoModelId" +
                "LEFT JOIN tankkaarten t ON b.bestuurderId = t.bestuurderId" +
                "WHERE b.rijksRegisterNummer = @rijksRegisterNummer";

            using (SqlCommand command = new(queryBestuurder, Connection)) {
                try {
                    command.Parameters.AddWithValue("@rijksRegisterNummer", rijksRegisterNummer);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader()) {
                        if (dataReader.HasRows) {
                            dataReader.Read();

                            //Bestuurder gevonden
                            Bestuurder bestuurderDB = new(
                                (int)dataReader["bestuurderId"],
                                (string)dataReader["voornaam"],
                                (string)dataReader["achternaam"],
                                (string)dataReader["geboorteDatum"],
                                (string)dataReader["rijbewijsType"],
                                (string)dataReader["rijbewijsNummer"],
                                (string)dataReader["rijksRegisterNummer"]
                            ) {
                                AanMaakDatum = (DateTime)dataReader["aanmaakDatum"]
                            };

                            //Heeft bestuurder Adres
                            if (dataReader["adresId"] != null) {
                                Adres adresDB = new(
                                    (int)dataReader["adresId"],
                                    (string)dataReader["straat"],
                                    (string)dataReader["nr"],
                                    (string)dataReader["postcode"],
                                    (string)dataReader["gemeente"]
                                );
                                bestuurderDB.Adres = adresDB;
                            }

                            //Is bestuurder gekoppeld aan een voertuig
                            if (dataReader["voertuigId"] != null) {

                                //AutoType kan nog veranderen naar ConfigFile
                                AutoType autoType = new ((string)dataReader["autotype"]);

                                //Kleur verschuift naar DB
                                Kleur kleur = new ((string)dataReader["kleurnaam"]);

                                AantalDeuren? aantalDeuren = (AantalDeuren?)Enum.Parse(typeof(AantalDeuren?), (string)dataReader["aantalDeuren"]);

                                Voertuig voertuigDB = new(
                                    new AutoModel(
                                        (int)dataReader["autoModelId"],
                                        (string)dataReader["merk"],
                                        (string)dataReader["autoModelNaam"],
                                        autoType
                                    ),
                                    (string)dataReader["chassisNummer"],
                                    (string)dataReader["nummerPlaat"],
                                    new BrandstofVoertuig((string)dataReader["brandstofNaam"], (bool)dataReader["hybride"])
                                ) {
                                    AantalDeuren = aantalDeuren,
                                    VoertuigKleur = kleur,
                                    InBoekDatum = (DateTime)dataReader["inboekDatum"]
                                };

                                bestuurderDB.VoegVoertuigToe(voertuigDB);
                            }

                            //Heeft de bestuurder een Tankkaart
                            if (dataReader["tankkaartNummer"] != null) {
                                TankKaart tankKaartDB = new(
                                    (string)dataReader["tankKaartNummer"],
                                    (bool)dataReader["actief"],
                                    (DateTime)dataReader["geldigheidsDatum"],
                                    (string)dataReader["pincode"]
                                ) {
                                    UitgeefDatum = (DateTime)dataReader["uitgeefDatum"]
                                };

                                string queryTankkaartOpVullen = "SELECT * FROM tankkaarten_brandstoftypes t" +
                                    "JOIN brandstofType b ON t.brandstofTypeId = b.brandstoftypeId" +
                                    "where t.TankKaartNummer = @tankkaartNummer";

                                command.CommandText = queryTankkaartOpVullen;
                                command.Parameters.AddWithValue("@tankkaartNummer", dataReader["tankkaartNummer"]);

                                command.ExecuteReader();

                                if (dataReader.HasRows) {
                                    while (dataReader.Read()) {
                                        tankKaartDB.VoegBrandstofToe(
                                            new BrandstofType((string)dataReader["brandstofNaam"])
                                        );
                                    }
                                }

                                //voeg toe aan bestuurder
                                bestuurderDB.VoegTankKaartToe(tankKaartDB);
                            }
                            //Bestuurder met alle mogelijke objecten die gereleateerd zijn
                            return bestuurderDB;
                        } else {
                            return null; //Geen bestuurder gevonden met dit rijksRegisterNummer
                        }
                    }
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("ZoekBestuurder op rijksregisternummer - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public IReadOnlyList<Bestuurder> FilterOpBestuurdersNaam(string achterNaamEnVoornaam, bool bestuurdersZonderVoertuig)
        {
            string zonderVoertuig = bestuurdersZonderVoertuig ? " b.voertuigid IS NULL AND " : null;

            string query = "SELECT b.bestuurderid, b.achternaam, b.voornaam, b.geboortedatum, b.rijksregisternummer, " +
                "b.rijbewijsnummer, b.rijbewijstype, b.aanmaakDatum, a.* FROM Bestuurder AS b " +
                   " LEFT JOIN adres AS a " +
                   " ON b.adresId = a.adresId " +
                   $" WHERE {zonderVoertuig} concat(b.achternaam, ' ', b.voornaam) LIKE @achterNaamEnVoornaam + '%'";

            List<Bestuurder> bestuurders = new();

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@achterNaamEnVoornaam", achterNaamEnVoornaam);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {                    
                                Bestuurder bestuurderDB = new(
                                    (int)dataReader["bestuurderid"],
                                    (string)dataReader["voornaam"],
                                    (string)dataReader["achternaam"],
                                    (string)dataReader["geboortedatum"],
                                    (string)dataReader["rijbewijstype"],
                                    (string)dataReader["rijbewijsnummer"],
                                    (string)dataReader["rijksregisternummer"]
                                );

                                if(!dataReader.IsDBNull(dataReader.GetOrdinal("adresId")))
                                {
                                    Adres adresDB = new(
                                        (int)dataReader["adresId"],
                                        (string)dataReader["straat"],
                                        (string)dataReader["nummer"],
                                        (string)dataReader["postcode"],
                                        (string)dataReader["gemeente"]
                                    );
                                    bestuurderDB.Adres = adresDB;
                                }

                                bestuurders.Add(bestuurderDB);
                            }
                        }

                        return bestuurders;
                    }
                }
                catch (Exception ex)
                {
                    throw new BestuurderRepositoryADOException("Filteren op naam - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }
    }
}
