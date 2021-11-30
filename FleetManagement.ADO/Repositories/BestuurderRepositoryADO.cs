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

            string query = "SELECT count(*) FROM Bestuurder WHERE rijksregisternummer=@rijksregisternummer;";

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

        public void UpdateBestuurder(Bestuurder bestuurder) {

            string query = "UPDATE Bestuurder" +
                           " SET adresid=@adresid, voornaam=@voornaam, achternaam=@achternaam, geboortedatum=@geboortedatum, rijksregisternummer=@rijksregisternummer, " +
                           " rijbewijstype=@rijbewijstype, rijbewijsnummer=@rijbewijsnummer, voertuigid=@voertuigid" +
                           " WHERE bestuurderid=@bestuurderid";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@voornaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@achternaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geboortedatum", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijksregisternummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijstype", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijsnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@voertuigid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@adresid", SqlDbType.Int));

                    command.Parameters["@voornaam"].Value = bestuurder.Voornaam;
                    command.Parameters["@achternaam"].Value = bestuurder.Achternaam;
                    command.Parameters["@geboortedatum"].Value = bestuurder.GeboorteDatum;
                    command.Parameters["@rijksregisternummer"].Value = bestuurder.RijksRegisterNummer;
                    command.Parameters["@rijbewijstype"].Value = bestuurder.TypeRijbewijs;
                    command.Parameters["@rijbewijsnummer"].Value = bestuurder.RijBewijsNummer;
                    command.Parameters["@bestuurderid"].Value = bestuurder.BestuurderId;

                    if (bestuurder.HeeftBestuurderVoertuig)
                    {
                        command.Parameters["@voertuigid"].Value = bestuurder.Voertuig.VoertuigId;
                    }
                    else
                    {
                        command.Parameters["@voertuigid"].Value = DBNull.Value;
                    }

                    if (bestuurder.Adres == null)
                    {
                         command.Parameters["@adresid"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@adresid"].Value = bestuurder.Adres.AdresId;
                    }

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

        public Bestuurder VoegBestuurderToe(Bestuurder bestuurder)
        {
            Connection.Open();
            using (SqlTransaction transaction = Connection.BeginTransaction())
            {
                try
                {
                    int newId = TransactionAdres(bestuurder, Connection, transaction);

                    if (newId > 0)
                        bestuurder.Adres.VoegIdToe(newId);

                    Bestuurder bestuurderDB = VoegBestuurderToe(bestuurder, Connection, transaction);
                    transaction.Commit();

                    return bestuurderDB;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new BestuurderRepositoryADOException("Voeg Bestuurder toe - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        private Bestuurder VoegBestuurderToe(Bestuurder bestuurder, SqlConnection sqlConnection = null, SqlTransaction transaction = null) {

            if (sqlConnection != null)
            {
                Connection = sqlConnection;
            }

            string query = "INSERT INTO Bestuurder (adresid, voornaam, achternaam, geboortedatum, rijksregisternummer,rijbewijstype, rijbewijsnummer) " +
                           "OUTPUT INSERTED.bestuurderid VALUES (@adresid, @voornaam, @achternaam, @geboortedatum, @rijksregisternummer, @rijbewijstype, @rijbewijsnummer)";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {

                    if (transaction != null) command.Transaction = transaction;
                    if (Connection.State != ConnectionState.Open) Connection.Open();

                    command.Parameters.Add(new SqlParameter("@adresid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@voornaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@achternaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geboortedatum", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijksregisternummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijstype", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijsnummer", SqlDbType.NVarChar));

                    if (bestuurder.Adres == null)
                        command.Parameters["@adresid"].Value = DBNull.Value;
                    else
                        command.Parameters["@adresid"].Value = bestuurder.Adres.AdresId;

                    command.Parameters["@voornaam"].Value = bestuurder.Voornaam;
                    command.Parameters["@achternaam"].Value = bestuurder.Achternaam;
                    command.Parameters["@geboortedatum"].Value = bestuurder.GeboorteDatum;
                    command.Parameters["@rijksregisternummer"].Value = bestuurder.RijksRegisterNummer;
                    command.Parameters["@rijbewijstype"].Value = bestuurder.TypeRijbewijs;
                    command.Parameters["@rijbewijsnummer"].Value = bestuurder.RijBewijsNummer;

                    command.CommandText = query;

                    bestuurder.VoegIdToe((int)command.ExecuteScalar());
                    return bestuurder;

                } catch (Exception ex) {
                    throw new BestuurderRepositoryADOException("Voeg Bestuurder Toe - gefaald", ex);
                } finally {
                    if (sqlConnection is null) Connection.Close();
                }
            }
        }

        private int TransactionAdres(Bestuurder bestuurder, SqlConnection sqlConnection = null, SqlTransaction transaction = null)
        {
            if (bestuurder.Adres != null)
            {
                if (sqlConnection != null)
                {
                    Connection = sqlConnection;
                }

                string query = "INSERT INTO Adres (straat, nummer, postcode, gemeente)" +
                               "OUTPUT INSERTED.adresid VALUES (@straat, @nummer, @postcode, @gemeente)";

                using (SqlCommand command = Connection.CreateCommand())
                {
                    try
                    {
                        if (transaction != null) command.Transaction = transaction;
                        if (Connection.State != ConnectionState.Open) Connection.Open();

                        command.Parameters.Add(new SqlParameter("@straat", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@nummer", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@postcode", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@gemeente", SqlDbType.NVarChar));

                        if (bestuurder.Adres.Straat == null)
                            command.Parameters["@straat"].Value = DBNull.Value;
                        else
                            command.Parameters["@straat"].Value = bestuurder.Voornaam;

                        if (bestuurder.Adres.Nr == null)
                            command.Parameters["@nummer"].Value = DBNull.Value;
                        else
                            command.Parameters["@nummer"].Value = bestuurder.Voornaam;

                        if (bestuurder.Adres.Postcode == null)
                            command.Parameters["@postcode"].Value = DBNull.Value;
                        else
                            command.Parameters["@postcode"].Value = bestuurder.Voornaam;

                        if (bestuurder.Adres.Gemeente == null)
                            command.Parameters["@gemeente"].Value = DBNull.Value;
                        else
                            command.Parameters["@gemeente"].Value = bestuurder.Voornaam;

                        command.CommandText = query;
                        return (int)command.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw new BestuurderRepositoryADOException("Voeg Bestuurdersadres Toe - gefaald", ex);
                    }
                    finally
                    {
                        if (sqlConnection is null) Connection.Close();
                    }
                }
            }

            return 0;
        }

        //bezig idee uitwerking Filip
        public Bestuurder ZoekBestuurder(string rijksRegisterNummer) {

            string queryBestuurder = "SELECT * FROM Bestuurder b" +
                "LEFT JOIN Adres a ON b.adresId = a.adresId" +
                "LEFT JOIN Voertuig v ON b.bestuurderId = v.bestuurderId" +
                "LEFT JOIN AutoModel a ON v.autoModelId = a.autoModelId" +
                "LEFT JOIN Tankkaart t ON b.bestuurderId = t.bestuurderId" +
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
                                AanmaakDatum = (DateTime)dataReader["aanmaakDatum"]
                            };

                            //Heeft bestuurder Adres
                            if (dataReader["adresId"] != null) {
                                Adres adresDB = new(
                                    (string)dataReader["straat"],
                                    (string)dataReader["nr"],
                                    (string)dataReader["postcode"],
                                    (string)dataReader["gemeente"]
                                );

                                adresDB.VoegIdToe((int)dataReader["adresId"]);
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

            string query = "SELECT * FROM Bestuurder AS b " +
                   "LEFT JOIN adres AS a ON b.adresId = a.adresId " +
                   "LEFT JOIN Voertuig v ON b.voertuigId = v.voertuigId " +
                   "LEFT JOIN AutoModel au ON v.autoModelId = au.autoModelId " +
                   "LEFT JOIN Brandstoftype br ON v.brandstoftypeid = br.brandstofid " +
                   "LEFT JOIN Tankkaart t ON b.bestuurderId = t.bestuurderId " +
                   $"WHERE {zonderVoertuig} concat(b.achternaam, ' ', b.voornaam) LIKE @achterNaamEnVoornaam + '%' " +
                   "ORDER BY achternaam ASC " +
                   "OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

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
                                        (string)dataReader["straat"],
                                        (string)dataReader["nummer"],
                                        (string)dataReader["postcode"],
                                        (string)dataReader["gemeente"]
                                    );
                                    adresDB.VoegIdToe((int)dataReader["adresId"]);
                                    bestuurderDB.Adres = adresDB;
                                }

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("voertuigid")) 
                                    && !dataReader.IsDBNull(dataReader.GetOrdinal("automodelid")) 
                                    && !dataReader.IsDBNull(dataReader.GetOrdinal("brandstoftypeid")))
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

                                    bestuurderDB.VoegVoertuigToe(voertuigDB);
                                }

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("tankkaartnummer")))
                                {
                                    TankKaart tankKaartDB = new(
                                        (string)dataReader["tankkaartnummer"],
                                        (bool)dataReader["actief"],
                                        dataReader.GetDateTime(dataReader.GetOrdinal("geldigheidsdatum"))
                                    );

                                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("pincode")))
                                    {
                                        tankKaartDB.VoegPincodeToe((string)dataReader["pincode"]);
                                    };

                                    bestuurderDB.VoegTankKaartToe(tankKaartDB);
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
