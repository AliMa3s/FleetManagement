using FleetManagement.ADO.RepositoryExceptions;
using FleetManagement.Interfaces;
using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace FleetManagement.ADO.Repositories
{
    public class BestuurderRepositoryADO : RepoConnection, IBestuurderRepository
    {
        public BestuurderRepositoryADO(string connectionstring) : base(connectionstring) { }

        public bool BestaatBestuurder(int id)
        {

            string query = "SELECT count(*) FROM Bestuurder WHERE bestuurderid=@bestuurderid";
            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));

                    command.Parameters["@bestuurderid"].Value = id;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new AdresRepositoryADOException("BestaatBestuurder(int-id)- gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public bool BestaatRijksRegisterNummer(string rijksRegisterNr)
        {

            string query = "SELECT count(*) FROM Bestuurder WHERE rijksregisternummer=@rijksregisternummer;";

            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@rijksregisternummer", SqlDbType.NVarChar));

                    command.Parameters["@rijksregisternummer"].Value = rijksRegisterNr;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new AdresRepositoryADOException("BestaatRijksRegisterNummer - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        private void UpdateBestuurder(Bestuurder bestuurder, string rijksregisterNummer, SqlConnection sqlConnection = null, 
            SqlTransaction transaction = null)
        {
            if (sqlConnection != null)
            {
                Connection = sqlConnection;
            }

            string query = "UPDATE Bestuurder " +
               "SET adresid=@adresid, voornaam=@voornaam, achternaam=@achternaam, geboortedatum=@geboortedatum, " +
               "rijbewijstype=@rijbewijstype, rijksregisternummer=@rijksregisternummer, voertuigid=@voertuigid, aanmaakdatum=@aanmaakdatum " +
               "WHERE bestuurderid=@bestuurderid";

            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    if (transaction != null) command.Transaction = transaction;
                    if (Connection.State != ConnectionState.Open) Connection.Open();

                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@voornaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@achternaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geboortedatum", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijksregisternummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijstype", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@voertuigid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@adresid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@aanmaakdatum", SqlDbType.DateTime));

                    command.Parameters["@voornaam"].Value = bestuurder.Voornaam;
                    command.Parameters["@achternaam"].Value = bestuurder.Achternaam;
                    command.Parameters["@geboortedatum"].Value = bestuurder.GeboorteDatum;
                    command.Parameters["@rijksregisternummer"].Value = rijksregisterNummer;
                    command.Parameters["@rijbewijstype"].Value = bestuurder.TypeRijbewijs;
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

                    if (bestuurder.AanmaakDatum.HasValue)
                    {
                        command.Parameters["@aanmaakdatum"].Value = bestuurder.AanmaakDatum.Value;
                    }
                    else
                    {
                        command.Parameters["@aanmaakdatum"].Value = DBNull.Value;
                    }

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new BestuurderRepositoryADOException("UpdateBestuurder - gefaald", ex);
                }
                finally
                {
                    if (sqlConnection is null) Connection.Close();
                }
            }
        }

        public Bestuurder UpdateBestuurder(Bestuurder bestuurder)
        {
            try
            {
                return UpdateBestuurder(bestuurder, bestuurder.RijksRegisterNummer);
            }
            catch (Exception ex)
            {
                throw new BestuurderRepositoryADOException("UpdateBestuurder - gefaald", ex);
            }
        }

        //Updaten met nieuwe rijksregister & geboortedatum
        public Bestuurder UpdateBestuurder(Bestuurder bestuurder, string anderRijksregisterNummer)
        {
            Connection.Open();
            using (SqlTransaction transaction = Connection.BeginTransaction())
            {
                try
                {
                    if(bestuurder.Adres != null && bestuurder.Adres.AdresId > 0)
                    {
                        UpdateBestuurderAdres(bestuurder, Connection, transaction);
                    }
                    else {

                        int newId = VoegBestuurderAdresToe(bestuurder, Connection, transaction);

                        if (newId > 0)
                            bestuurder.Adres.VoegIdToe(newId);
                    }

                    UpdateBestuurder(bestuurder, anderRijksregisterNummer, Connection, transaction);
                    transaction.Commit();

                    return bestuurder;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new BestuurderRepositoryADOException("UpdateBestuurder - gefaald", ex);

                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public void VerwijderBestuurder(Bestuurder bestuurder)
        {
            string query = "DELETE FROM Bestuurder WHERE bestuurderid=@bestuurderid";

            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));
                    command.Parameters["@bestuurderid"].Value = bestuurder.BestuurderId;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new BestuurderRepositoryADOException("Verwijderbestuurder - gefaald", ex);
                }
                finally
                {
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
                    int newId = VoegBestuurderAdresToe(bestuurder, Connection, transaction);

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

        private Bestuurder VoegBestuurderToe(Bestuurder bestuurder, SqlConnection sqlConnection = null, SqlTransaction transaction = null)
        {

            if (sqlConnection != null)
            {
                Connection = sqlConnection;
            }

            string query = "INSERT INTO Bestuurder (adresid, voornaam, achternaam, geboortedatum, rijksregisternummer, rijbewijstype, voertuigid) " +
                "OUTPUT INSERTED.bestuurderid VALUES (@adresid, @voornaam, @achternaam, @geboortedatum, @rijksregisternummer, @rijbewijstype, @voertuigid)";

            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {

                    if (transaction != null) command.Transaction = transaction;
                    if (Connection.State != ConnectionState.Open) Connection.Open();

                    command.Parameters.Add(new SqlParameter("@adresid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@voornaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@achternaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geboortedatum", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijbewijstype", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@rijksregisternummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@voertuigid", SqlDbType.Int));

                    if (bestuurder.Adres == null)
                    {
                        command.Parameters["@adresid"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@adresid"].Value = bestuurder.Adres.AdresId;
                    }

                    if (bestuurder.HeeftBestuurderVoertuig)
                    {
                        command.Parameters["@voertuigid"].Value = bestuurder.Voertuig.VoertuigId;
                    }
                    else
                    {
                        command.Parameters["@voertuigid"].Value = DBNull.Value;
                    }

                    command.Parameters["@voornaam"].Value = bestuurder.Voornaam;
                    command.Parameters["@achternaam"].Value = bestuurder.Achternaam;
                    command.Parameters["@geboortedatum"].Value = bestuurder.GeboorteDatum;
                    command.Parameters["@rijksregisternummer"].Value = bestuurder.RijksRegisterNummer;
                    command.Parameters["@rijbewijstype"].Value = bestuurder.TypeRijbewijs;

                    command.CommandText = query;

                    bestuurder.VoegIdToe((int)command.ExecuteScalar());
                    return bestuurder;

                }
                catch (Exception ex)
                {
                    throw new BestuurderRepositoryADOException("Voeg Bestuurder Toe - gefaald", ex);
                }
                finally
                {
                    if (sqlConnection is null) Connection.Close();
                }
            }
        }

        private int VoegBestuurderAdresToe(Bestuurder bestuurder, SqlConnection sqlConnection = null, SqlTransaction transaction = null)
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
                            command.Parameters["@straat"].Value = bestuurder.Adres.Straat;

                        if (bestuurder.Adres.Nr == null)
                            command.Parameters["@nummer"].Value = DBNull.Value;
                        else
                            command.Parameters["@nummer"].Value = bestuurder.Adres.Nr;

                        if (bestuurder.Adres.Postcode == null)
                            command.Parameters["@postcode"].Value = DBNull.Value;
                        else
                            command.Parameters["@postcode"].Value = bestuurder.Adres.Postcode;

                        if (bestuurder.Adres.Gemeente == null)
                            command.Parameters["@gemeente"].Value = DBNull.Value;
                        else
                            command.Parameters["@gemeente"].Value = bestuurder.Adres.Gemeente;

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

        private void UpdateBestuurderAdres(Bestuurder bestuurder, SqlConnection sqlConnection = null, SqlTransaction transaction = null)
        {
            if (bestuurder.Adres != null && bestuurder.Adres.AdresId > 0)
            {
                if (sqlConnection != null)
                {
                    Connection = sqlConnection;
                }

                string query = "UPDATE Adres " +
                   "SET straat=@straat, nummer=@nummer, postcode=@postcode, gemeente=@gemeente " +
                   "WHERE adresid=@adresid";

                using (SqlCommand command = Connection.CreateCommand())
                {
                    try
                    {
                        if (transaction != null) command.Transaction = transaction;
                        if (Connection.State != ConnectionState.Open) Connection.Open();

                        command.Parameters.Add(new SqlParameter("@adresid", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@straat", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@nummer", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@postcode", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@gemeente", SqlDbType.NVarChar));

                        command.Parameters["@adresid"].Value = bestuurder.Adres.AdresId;

                        if (bestuurder.Adres.Straat == null)
                            command.Parameters["@straat"].Value = DBNull.Value;
                        else
                            command.Parameters["@straat"].Value = bestuurder.Adres.Straat;

                        if (bestuurder.Adres.Nr == null)
                            command.Parameters["@nummer"].Value = DBNull.Value;
                        else
                            command.Parameters["@nummer"].Value = bestuurder.Adres.Nr;

                        if (bestuurder.Adres.Postcode == null)
                            command.Parameters["@postcode"].Value = DBNull.Value;
                        else
                            command.Parameters["@postcode"].Value = bestuurder.Adres.Postcode;

                        if (bestuurder.Adres.Gemeente == null)
                            command.Parameters["@gemeente"].Value = DBNull.Value;
                        else
                            command.Parameters["@gemeente"].Value = bestuurder.Adres.Gemeente;

                        command.CommandText = query;
                        command.ExecuteNonQuery();                      
                    }
                    catch (Exception ex)
                    {
                        throw new BestuurderRepositoryADOException("UpdateAdres - gefaald", ex);
                    }
                    finally
                    {
                        if (sqlConnection is null) Connection.Close();
                    }
                }
            }
        }

        //Op rijksregisternummer
        public Bestuurder ZoekBestuurder(string rijksRegisterNummer)
        {
            string query = "SELECT * FROM Bestuurder AS b " +
                   "LEFT JOIN adres AS a ON b.adresId = a.adresId " +
                   "LEFT JOIN Voertuig v ON b.voertuigId = v.voertuigId " +
                   "LEFT JOIN AutoModel au ON v.autoModelId = au.autoModelId " +
                   "LEFT JOIN Brandstoftype br ON v.brandstoftypeid = br.brandstoftypeid " +
                   "LEFT JOIN Tankkaart t ON b.bestuurderId = t.bestuurderId " +
                   "WHERE b.rijksregisternummer=@rijksregisternummer";

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@rijksregisternummer", rijksRegisterNummer);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();

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

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("voertuigid"))
                                    && !dataReader.IsDBNull(dataReader.GetOrdinal("automodelid"))
                                    && !dataReader.IsDBNull(dataReader.GetOrdinal("brandstoftypeid")))
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

                                bestuurderDB.VoegVoertuigToe(voertuigDB);
                            }

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("tankkaartnummer")))
                            {
                                TankKaart tankKaartDB = new(
                                    (string)dataReader["tankkaartnummer"],
                                    (bool)dataReader["actief"],
                                    dataReader.GetDateTime(dataReader.GetOrdinal("geldigheidsdatum"))
                                );

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("uitgeefdatum")))
                                {
                                    tankKaartDB.UitgeefDatum = dataReader.GetDateTime(dataReader.GetOrdinal("uitgeefdatum"));
                                }

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("pincode")))
                                {
                                    tankKaartDB.VoegPincodeToe((string)dataReader["pincode"]);
                                };

                                bestuurderDB.VoegTankKaartToe(tankKaartDB);
                            }

                            return bestuurderDB;
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new BestuurderRepositoryADOException("Zoeken op rijksregisternummer - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        //Om bestuurders te selecteren zonder voertuig
        public IReadOnlyList<Bestuurder> SelecteerBestuurdersZonderVoertuig(string achterNaamEnVoornaam)
        {
            string query = "SELECT * FROM Bestuurder AS b " +
                   "LEFT JOIN adres AS a ON b.adresId = a.adresId " +
                   "LEFT JOIN Tankkaart t ON b.bestuurderId = t.bestuurderId " +
                   "WHERE b.voertuigid IS NULL AND concat(b.achternaam, ' ', b.voornaam) LIKE @achterNaamEnVoornaam + '%' " +
                   "ORDER BY b.achternaam ASC , b.voornaam ASC " +
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

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("tankkaartnummer")))
                                {
                                    TankKaart tankKaartDB = new(
                                        (string)dataReader["tankkaartnummer"],
                                        (bool)dataReader["actief"],
                                        dataReader.GetDateTime(dataReader.GetOrdinal("geldigheidsdatum"))
                                    );

                                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("uitgeefdatum")))
                                    {
                                        tankKaartDB.UitgeefDatum = dataReader.GetDateTime(dataReader.GetOrdinal("uitgeefdatum"));
                                    }

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

        //Om bestuurders te selecteren zonder tankkaart
        public IReadOnlyList<Bestuurder> SelecteerBestuurdersZondertankkaart(string achterNaamEnVoornaam)
        {
            string query = "SELECT * FROM Bestuurder AS b " +
                   "LEFT JOIN adres AS a ON b.adresId = a.adresId " +
                   "LEFT JOIN Tankkaart t ON b.bestuurderId = t.bestuurderId " +
                   "WHERE t.bestuurderid IS NULL AND concat(b.achternaam, ' ', b.voornaam) LIKE @achterNaamEnVoornaam + '%' " +
                   "ORDER BY b.achternaam ASC , b.voornaam ASC " +
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

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("tankkaartnummer")))
                                {
                                    TankKaart tankKaartDB = new(
                                        (string)dataReader["tankkaartnummer"],
                                        (bool)dataReader["actief"],
                                        dataReader.GetDateTime(dataReader.GetOrdinal("geldigheidsdatum"))
                                    );

                                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("uitgeefdatum")))
                                    {
                                        tankKaartDB.UitgeefDatum = dataReader.GetDateTime(dataReader.GetOrdinal("uitgeefdatum"));
                                    }

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

        //Voor zoeken met alle includes meegeleverd voor depencency injection in detail
        public IReadOnlyList<Bestuurder> FilterOpBestuurdersNaam(string achterNaamEnVoornaam)
        {
            string query = "SELECT * FROM Bestuurder AS b " +
                   "LEFT JOIN adres AS a ON b.adresId = a.adresId " +
                   "LEFT JOIN Voertuig v ON b.voertuigId = v.voertuigId " +
                   "LEFT JOIN AutoModel au ON v.autoModelId = au.autoModelId " +
                   "LEFT JOIN Brandstoftype br ON v.brandstoftypeid = br.brandstoftypeid " +
                   "LEFT JOIN Tankkaart t ON b.bestuurderId = t.bestuurderId " +
                   "WHERE concat(b.achternaam, ' ', b.voornaam) LIKE @achterNaamEnVoornaam + '%' " +
                   "ORDER BY b.achternaam ASC, b.voornaam ASC " +
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

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("voertuigid"))
                                        && !dataReader.IsDBNull(dataReader.GetOrdinal("automodelid"))
                                        && !dataReader.IsDBNull(dataReader.GetOrdinal("brandstoftypeid")))
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

                                    bestuurderDB.VoegVoertuigToe(voertuigDB);
                                }

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("tankkaartnummer")))
                                {
                                    TankKaart tankKaartDB = new(
                                        (string)dataReader["tankkaartnummer"],
                                        (bool)dataReader["actief"],
                                        dataReader.GetDateTime(dataReader.GetOrdinal("geldigheidsdatum"))
                                    );

                                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("uitgeefdatum")))
                                    {
                                        tankKaartDB.UitgeefDatum = dataReader.GetDateTime(dataReader.GetOrdinal("uitgeefdatum"));
                                    }

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
