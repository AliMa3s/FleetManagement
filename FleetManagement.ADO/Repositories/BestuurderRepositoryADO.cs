using FleetManagement.ADO.Repositories.Instances;
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


        private readonly string _connectionString = @"YourConnectionStringhere";
        public BestuurderRepositoryADO(string connectionstring) {
            this._connectionString = connectionstring;
        }

        private SqlConnection getConnection() {
            SqlConnection connection = new SqlConnection(_connectionString);
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
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                try {
                    connection.Open();
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
                    connection.Close();
                }
            }
        }

        public void UpdateBestuurder(Bestuurder bestuurder) {
            SqlConnection connection = getConnection();

            string query = "UPDATE Bestuurder" +
                           "SET voornaam=@voornaam, achternaam=@achternaam, geboortedatum=@geboortedatum, rijksregisternummer=@rijksregisternummer, " +
                           " rijbewijstype=@rijbewijstype, rijbewijsnummer=@rijbewijsnummer" +
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

            string query = "INSERT INTO Bestuurder (voornaam, achternaam, geboortedatum, rijksregisternummer,rijbewijstype,rijbewijsnummer)" +
                           "VALUES (@voornaam, @achternaam, @geboortedatum, @rijksregisternummer, @rijbewijstype, @rijbewijsnummer)";

            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
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
        public Bestuurder ZoekBestuurder(string rijksRegisterNummer) {

            string queryBestuurder = "SELECT * FROM bestuurders b" +
                "LEFT JOIN adressen a ON b.adresId = a.adresId" +
                "LEFT JOIN voertuigen v ON b.bestuurderId = v.bestuurderId" +
                "LEFT JOIN automodellen a ON v.autoModelId = a.autoModelId" +
                "LEFT JOIN tankkaarten t ON b.bestuurderId = t.bestuurderId" +
                "WHERE b.rijksRegisterNummer = @rijksRegisterNummer";

            SqlConnection connection = getConnection();

            using (SqlCommand command = new(queryBestuurder, connection)) {
                try {
                    command.Parameters.AddWithValue("@rijksRegisterNummer", rijksRegisterNummer);
                    connection.Open();

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
                                AutoType autoType = (AutoType)Enum.Parse(typeof(AutoType), (string)dataReader["autotype"]);

                                //Kleur verschuift naar DB
                                Kleur? kleur = (Kleur?)Enum.Parse(typeof(Kleur?), (string)dataReader["kleurnaam"]);
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

                            connection.Close();

                            //Bestuurder met alle mogelijke objecten die gereleateerd zijn
                            return bestuurderDB;
                        } else {
                            return null; //Geen bestuurder gevonden met dit rijksRegisterNummer
                        }
                    }
                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("ZoekBestuurder op rijksregisternummer - gefaald", ex);
                } finally {
                    connection?.Dispose();
                    connection = null;
                }
            }
        }

        //public PaginaLijst<Bestuurder> FilterOpBestuurdersNaam(string voornaam, string achternaam) {
        //    throw new NotImplementedException();
        //}

        //public PaginaLijst<Bestuurder> AlleBestuurders(SorteerOptie sorteerOptie) {

        //    if(sorteerOptie == null)
        //    {
        //        throw new BestuurderRepositoryADOException("SorteerOptie mag niet null zijn");
        //    }

        //    string concatQuery = "";
        //    concatQuery += sorteerOptie.OrderBy switch
        //    {
        //        "bestuurderId" => "ORDER BY bestuurderId ",
        //        "voornaam" => "ORDER BY voornaam ",
        //        "achternaam" => "ORDER BY achternaam ",
        //        "geboorteDatum" => "ORDER BY geboorteDatum ",
        //        "AanmaakDatum" => "ORDER BY AanmaakDatum ",
        //        _ => "ORDER BY bestuurderId ",
        //    };

        //    concatQuery += sorteerOptie.Sort;
        //    concatQuery += $" LIMIT {sorteerOptie.AantalPerPagina * (sorteerOptie.HuidigePaginaNummer - 1)}, " +
        //        $"{sorteerOptie.AantalPerPagina}";

        //    string queryBestuurder = "SELECT * FROM bestuurders b" +
        //    "LEFT JOIN adressen a ON b.adresId = a.adresId" +
        //    "LEFT JOIN voertuigen v ON b.bestuurderId = v.bestuurderId" +
        //    "LEFT JOIN automodellen a ON v.autoModelId = a.autoModelId" +
        //    "LEFT JOIN tankkaarten t ON b.bestuurderId = t.bestuurderId" +
        //    concatQuery;

        //    string totalCount = "SELECT count(bestuurderId) AS totaal FROM Bestuurders"; 

        //    SqlConnection connection = getConnection();

        //    using (SqlCommand command = new(totalCount, connection))
        //    {
        //        try
        //        {
        //            connection.Open();

        //            using (SqlDataReader dataReaderCount = command.ExecuteReader())
        //            {
        //                dataReaderCount.Read();

        //                int totaalResulaten = (int)dataReaderCount["totaal"];
        //                List<Bestuurder> bestuurdersDB = new();

        //                if (totaalResulaten > 0)
        //                {
        //                    command.CommandText = queryBestuurder;
        //                    using (SqlDataReader dataReader = command.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            //Bestuurder gevonden
        //                            Bestuurder bestuurderDB = BouwInstance.BestuurderInstance(dataReader);

        //                            //Heeft bestuurder Adres
        //                            if (dataReader["adresId"] != null)
        //                            {
        //                                Adres adresDB = BouwInstance.AdresInstance(dataReader);
        //                                bestuurderDB.Adres = adresDB;
        //                            }

        //                            //Is bestuurder gekoppeld aan een voertuig
        //                            if (dataReader["voertuigId"] != null)
        //                            {
        //                                Voertuig voertuigDB = BouwInstance.VoertuigInstance(dataReader);
        //                                bestuurderDB.VoegVoertuigToe(voertuigDB);
        //                            }

        //                            //Heeft de bestuurder een Tankkaart
        //                            if (dataReader["tankkaartNummer"] != null)
        //                            {
        //                                TankKaart tankKaartDB = BouwInstance.TankkaartInstance(dataReader);

        //                                //voeg toe aan bestuurder / zonder opvulling tankkaart anders te veel connecties
        //                                //Vraag brandstoffen op stuk voor stuk indien nodig in detail
        //                                bestuurderDB.VoegTankKaartToe(tankKaartDB);
        //                            }

        //                            connection.Close();

        //                            //Bestuurder toevoegen aan lijst
        //                            bestuurdersDB.Add(bestuurderDB);
        //                        }

        //                        //Geef List<T> terug met info over de resultaten
        //                        return new PaginaLijst<Bestuurder>(bestuurdersDB, totaalResulaten, sorteerOptie.HuidigePaginaNummer, 
        //                            sorteerOptie.AantalPerPagina);
        //                    }
        //                }
        //                else
        //                {
        //                    return new PaginaLijst<Bestuurder>(bestuurdersDB, totaalResulaten, sorteerOptie.HuidigePaginaNummer, 
        //                        sorteerOptie.AantalPerPagina);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new BestuurderRepositoryADOException("Zoek bestuurders met sorteerOptie - gefaald", ex);
        //        }
        //        finally
        //        {
        //            connection?.Dispose();
        //            connection = null;
        //        }
        //    }
        //}
    }
}
