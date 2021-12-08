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
    public class TankkaartRepositoryADO : RepoConnection, ITankkaartRepository {

        public TankkaartRepositoryADO(string connectionstring) : base(connectionstring) { }

        //Toevoegen public met transactie
        public void VoegTankKaartToe(TankKaart tankkaart)
        {
            Connection.Open();
            using (SqlTransaction transaction = Connection.BeginTransaction())
            {
                try
                {
                    VoegTankKaartToe(tankkaart, Connection, transaction);
                    VoegBrandstoffenToe(tankkaart, Connection, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new TankkaartRepositoryADOException("VoegTankkaarttoe - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        //toevoegen private
        private void VoegTankKaartToe(TankKaart tankkaart, SqlConnection sqlConnection = null, SqlTransaction transaction = null)
        {

            if (sqlConnection != null)
            {
                Connection = sqlConnection;
            }

            string query = "INSERT INTO Tankkaart (tankkaartnummer, bestuurderid, geldigheidsdatum, pincode, actief, uitgeefdatum) " +
                           "VALUES (@tankkaartnummer, @bestuurderid, @geldigheidsdatum, @pincode, @actief, @uitgeefdatum)";

            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    if (transaction != null) command.Transaction = transaction;
                    if (Connection.State != ConnectionState.Open) Connection.Open();

                    command.Parameters.Add(new SqlParameter("@tankkaartnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@geldigheidsdatum", SqlDbType.Date));
                    command.Parameters.Add(new SqlParameter("@pincode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@actief", SqlDbType.Bit));
                    command.Parameters.Add(new SqlParameter("@uitgeefdatum", SqlDbType.Date));

                    command.Parameters["@tankkaartnummer"].Value = tankkaart.TankKaartNummer;
                    command.Parameters["@geldigheidsdatum"].Value = tankkaart.GeldigheidsDatum;
                    command.Parameters["@actief"].Value = tankkaart.Actief;

                    if (tankkaart.Pincode == null)
                    {
                        command.Parameters["@pincode"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@pincode"].Value = tankkaart.Pincode;
                    }

                    if(tankkaart.HeeftTankKaartBestuurder)
                    {
                        command.Parameters["@bestuurderid"].Value = tankkaart.Bestuurder.BestuurderId;
                    }
                    else
                    {
                        command.Parameters["@bestuurderid"].Value = DBNull.Value;
                    }

                    if (tankkaart.UitgeefDatum.HasValue)
                    {
                        command.Parameters["@uitgeefdatum"].Value = tankkaart.UitgeefDatum.Value;
                    }
                    else
                    {
                        command.Parameters["@uitgeefdatum"].Value = DBNull.Value;
                    }

                    command.CommandText = query;
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("VoegTankkaarttoe - gefaald", ex);
                }
                finally
                {
                    if (sqlConnection is null) Connection.Close();
                }
            }
        }

        private void VoegBrandstoffenToe(TankKaart tankkaart, SqlConnection sqlConnection = null, SqlTransaction transaction = null)
        {
            if (tankkaart.Brandstoffen.Count > 0)
            {
                if (sqlConnection != null)
                {
                    Connection = sqlConnection;
                }

                string query = "";

                using (SqlCommand command = Connection.CreateCommand())
                {
                    try
                    {
                        if (transaction != null) command.Transaction = transaction;
                        if (Connection.State != ConnectionState.Open) Connection.Open();

                        int i = 1;
                        tankkaart.Brandstoffen.ForEach(brandstof => {

                            query = "INSERT INTO Tankkaart_Brandstoftype (tankkaartnummer, brandstoftypeid) " +
                                $"VALUES (@tankkaartnummer{i}, @brandstoftypeid{i}); ";

                            command.Parameters.Add(new SqlParameter($"@tankkaartnummer{i}", SqlDbType.NVarChar));
                            command.Parameters.Add(new SqlParameter($"@brandstoftypeid{i}", SqlDbType.Int));
                            command.Parameters[$"@tankkaartnummer{i}"].Value = tankkaart.TankKaartNummer;
                            command.Parameters[$"@brandstoftypeid{i}"].Value = brandstof.BrandstofTypeId;

                            command.CommandText += query;

                            i++;
                        });

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new TankkaartRepositoryADOException("Voeg brandstof toe - gefaald", ex);
                    }
                    finally
                    {
                        if (sqlConnection is null) Connection.Close();
                    }
                }
            }
        }

        public bool BestaatTankkaartBrandstof(TankKaart tankkaart, BrandstofType brandstof)
        {
            string query = "SELECT count(*) FROM Tankkaart_Brandstoftype " +
                "WHERE tankkaartnummer=@tankkaartnummer AND brandstoftypeid = @brandstoftypeid";

            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@tankkaartnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@brandstoftypeid", SqlDbType.Int));

                    command.Parameters["@tankkaartnummer"].Value = tankkaart.TankKaartNummer;
                    command.Parameters["@brandstoftypeid"].Value = brandstof.BrandstofTypeId;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("BestaatTankkaart- gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public void VoegTankkaartBrandstofToe(TankKaart tankkaart, BrandstofType brandstof) 
        {
            string query = "INSERT INTO Tankkaart_Brandstoftype (tankkaartnummer, brandstoftypeid) " +
                "VALUES (@tankkaartnummer, @brandstoftypeid); ";

            Connection.Open();

            try
            {
                using (SqlCommand command = Connection.CreateCommand())
                {
                    try
                    {
                        Connection.Open();

                        command.Parameters.Add(new SqlParameter($"@tankkaartnummer", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter($"@brandstoftypeid", SqlDbType.Int));
                        command.Parameters[$"@tankkaartnummer"].Value = tankkaart.TankKaartNummer;
                        command.Parameters[$"@brandstoftypeid"].Value = brandstof.BrandstofTypeId;

                        command.CommandText = query;
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new TankkaartRepositoryADOException("BestaatTankkaart- gefaald", ex);
                    }
                    finally
                    {
                        Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TankkaartRepositoryADOException("Verwijder brandstof - gefaald", ex);
            }
            finally
            {
                Connection.Close();
            }

        }

        public void VerwijderBrandstoffen(TankKaart tankKaart)
        {
            using (SqlCommand command = Connection.CreateCommand())
            {
                string query = "DELETE FROM Tankkaart_Brandstoftype WHERE tankkaartnummer=@tankkaartnummer ";

                Connection.Open();

                try
                {
                    command.Parameters.Add(new SqlParameter($"@tankkaartnummer", SqlDbType.NVarChar));
                    command.Parameters[$"@tankkaartnummer"].Value = tankKaart.TankKaartNummer;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("Verwijder brandstof - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            } 
        }

        public bool BestaatTankKaart(TankKaart tankkaart) {

            string query = "SELECT count(*) FROM Tankkaart WHERE tankkaartnummer=@tankkaartnummer";
            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@tankkaartnummer", SqlDbType.NVarChar));

                    command.Parameters["@tankkaartnummer"].Value = tankkaart.TankKaartNummer;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new TankkaartRepositoryADOException("BestaatTankkaart- gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public IReadOnlyList<TankKaart> GeefAlleTankkaarten() {
            string query = "SELECT * FROM Tankkaart t " +
                "LEFT JOIN Bestuurder b ON t.bestuurderid = b.bestuurderid " +
                "ORDER BY tankkaartnummer ASC " +
                "OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            List<TankKaart> kaartLijst = new List<TankKaart>();

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                string pincode = null;

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("pincode"))) pincode = (string)dataReader["pincode"];

                                TankKaart tankKaartDB = new TankKaart(
                                    (string)dataReader["tankkaartnummer"],
                                    (bool)dataReader["actief"],
                                    dataReader.GetDateTime(dataReader.GetOrdinal("geldigheidsdatum")),
                                    pincode
                                );

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("uitgeefdatum")))
                                {
                                    tankKaartDB.UitgeefDatum = dataReader.GetDateTime(dataReader.GetOrdinal("uitgeefdatum"));
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

                                    tankKaartDB.VoegBestuurderToe(bestuurderDB);
                                }

                                kaartLijst.Add(tankKaartDB);
                            }
                        }

                        return kaartLijst;
                    }
                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("Tankkaarten - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
           
        }

        public IReadOnlyList<BrandstofType> BrandstoffenVoorTankaart(TankKaart tankkaart)
        {
            //ophalen van brandstoffen voor detailweergave
            string query = "SELECT * FROM Tankkaart_Brandstoftype t " +
                "LEFT JOIN Brandstoftype b ON t.brandstoftypeid = b.brandstoftypeid " +
                "WHERE t.tankkaartnummer=@tankkaartnummer";

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    List<BrandstofType> brandstofTypesDB = new();

                    command.Parameters.AddWithValue("@tankkaartnummer", tankkaart.TankKaartNummer);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            { 
                                BrandstofType brandstofType = new BrandstofType(
                                    (int)dataReader["brandstoftypeid"],
                                    (string)dataReader["brandstofnaam"]
                                );

                                brandstofTypesDB.Add(brandstofType);
                            }  
                        }
                        return brandstofTypesDB;
                    }
                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("Geef tankkaart - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public TankKaart ZoekTankKaart(string tankkaartNr) {
            string query = "SELECT * FROM Tankkaart t " +
                "LEFT JOIN Bestuurder b ON t.bestuurderid = b.bestuurderid " +
                "WHERE tankkaartnummer=@tankkaartnummer";

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@tankkaartnummer", tankkaartNr);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();

                            string pincode = null;
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("pincode"))) pincode = (string)dataReader["pincode"];

                            TankKaart tankkaart = new TankKaart(
                                (string)dataReader["tankkaartnummer"],
                                (bool)dataReader["actief"],
                                (DateTime)dataReader["geldigheidsdatum"],
                                pincode
                            );

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("uitgeefdatum")))
                            {
                                tankkaart.UitgeefDatum = dataReader.GetDateTime(dataReader.GetOrdinal("uitgeefdatum"));
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

                                tankkaart.VoegBestuurderToe(bestuurderDB);
                            }

                            return tankkaart;
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("Geef tankkaart - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public void UpdateTankKaart(TankKaart tankkaart) {

            string query = "UPDATE Tankkaart SET " +
                           " bestuurderid=@bestuurderid, geldigheidsdatum=@geldigheidsdatum, pincode=@pincode, " +
                           "actief=@actief, uitgeefdatum=@uitgeefdatum " +
                           "WHERE tankkaartnummer=@tankkaartnummer";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {

                    Connection.Open();

                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@tankkaartnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geldigheidsdatum", SqlDbType.Date));
                    command.Parameters.Add(new SqlParameter("@pincode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@actief", SqlDbType.Bit));
                    command.Parameters.Add(new SqlParameter("@uitgeefdatum", SqlDbType.Date));

                    command.Parameters["@tankkaartnummer"].Value = tankkaart.TankKaartNummer;
                    command.Parameters["@geldigheidsdatum"].Value = tankkaart.GeldigheidsDatum;
                    command.Parameters["@actief"].Value = tankkaart.Actief;

                    if (tankkaart.Pincode == null)
                        command.Parameters["@pincode"].Value = DBNull.Value;
                    else
                        command.Parameters["@pincode"].Value = tankkaart.Pincode;

                    if (tankkaart.HeeftTankKaartBestuurder)
                        command.Parameters["@bestuurderid"].Value = tankkaart.Bestuurder.BestuurderId;
                    else
                        command.Parameters["@bestuurderid"].Value = DBNull.Value;

                    if (tankkaart.UitgeefDatum.HasValue)
                        command.Parameters["@uitgeefdatum"].Value = tankkaart.UitgeefDatum.Value;
                    else
                        command.Parameters["@uitgeefdatum"].Value = DBNull.Value;

                    command.CommandText = query;
                    command.ExecuteNonQuery();

                } catch (Exception ex) {
                    throw new TankkaartRepositoryADOException("UpdateTankkaart - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public TankKaart UpdateTankKaart(TankKaart tankkaart, string AnderTankkaartNummer)
        {
            string query = "UPDATE Tankkaart SET " +
                "tankkaartnummer=@andertankkaartnummer, bestuurderid=@bestuurderid, geldigheidsdatum=@geldigheidsdatum, pincode=@pincode, " +
                "actief=@actief, uitgeefdatum=@uitgeefdatum " +
                "WHERE tankkaartnummer=@tankkaartnummer";

            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();

                    command.Parameters.Add(new SqlParameter("@bestuurderid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@tankkaartnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@andertankkaartnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geldigheidsdatum", SqlDbType.Date));
                    command.Parameters.Add(new SqlParameter("@pincode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@actief", SqlDbType.Bit));
                    command.Parameters.Add(new SqlParameter("@uitgeefdatum", SqlDbType.Date));

                    command.Parameters["@tankkaartnummer"].Value = tankkaart.TankKaartNummer;
                    command.Parameters["@andertankkaartnummer"].Value = AnderTankkaartNummer;
                    command.Parameters["@geldigheidsdatum"].Value = tankkaart.GeldigheidsDatum;
                    command.Parameters["@actief"].Value = tankkaart.Actief;

                    if (tankkaart.Pincode == null)
                        command.Parameters["@pincode"].Value = DBNull.Value;
                    else
                        command.Parameters["@pincode"].Value = tankkaart.Pincode;

                    if (tankkaart.HeeftTankKaartBestuurder)
                        command.Parameters["@bestuurderid"].Value = tankkaart.Bestuurder.BestuurderId;
                    else
                        command.Parameters["@bestuurderid"].Value = DBNull.Value;

                    if (tankkaart.UitgeefDatum.HasValue)
                        command.Parameters["@uitgeefdatum"].Value = tankkaart.UitgeefDatum.Value;
                    else
                        command.Parameters["@uitgeefdatum"].Value = DBNull.Value;

                    command.CommandText = query;
                    command.ExecuteNonQuery();

                    tankkaart.UpdateTankkaartNummer(AnderTankkaartNummer);
                    return tankkaart;
                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("UpdateTankkaart - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

#warning tankkaart mag nooit verwijderd worden! Vragen aan Tom wat te doen
        public void VerwijderTankKaart(TankKaart tankkaart) {

            string query = "DELETE FROM Tankkaart WHERE tankkaartnummer=@tankkaartnummer";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@tankkaartnummer", SqlDbType.NVarChar));
                    command.Parameters["@tankkaartnummer"].Value = tankkaart.TankKaartNummer;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new TankkaartRepositoryADOException("VerwijderTankkaart - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        //Geef alle tankkaarten die nog geen bestuurder hebben
        public IReadOnlyList<TankKaart> TankaartenZonderBestuurder() {

            string query = "SELECT * FROM Tankkaart t " +
                "LEFT JOIN Bestuurder b ON t.bestuurderid = b.bestuurderid " +
                "WHERE t.actief = 1 AND t.bestuurderid IS NULL " +
                "ORDER BY tankkaartnummer ASC " +
                "OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            List<TankKaart> kaartLijst = new List<TankKaart>();

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                string pincode = null;

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("pincode"))) pincode = (string)dataReader["pincode"];

                                TankKaart tankKaartDB = new TankKaart(
                                    (string)dataReader["tankkaartnummer"],
                                    (bool)dataReader["actief"],
                                    dataReader.GetDateTime(dataReader.GetOrdinal("geldigheidsdatum")),
                                    pincode
                                );

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("uitgeefdatum")))
                                {
                                    tankKaartDB.UitgeefDatum = dataReader.GetDateTime(dataReader.GetOrdinal("uitgeefdatum"));
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

                                    tankKaartDB.VoegBestuurderToe(bestuurderDB);
                                }

                                kaartLijst.Add(tankKaartDB);
                            }
                        }

                        return kaartLijst;
                    }
                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("Tankkaarten - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public IReadOnlyList<TankKaart> ZoekTankKaarten(bool isTankkaartGeldig)
        {
            string query = "SELECT * FROM Tankkaart t " +
                "LEFT JOIN Bestuurder b ON t.bestuurderid = b.bestuurderid " +
                "WHERE actief = @isGeldig " + 
                "ORDER BY tankkaartnummer ASC " +
                "OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            List<TankKaart> tankaartenDB = new List<TankKaart>();

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@isGeldig", isTankkaartGeldig);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                string pincode = null;

                                if (!dataReader.IsDBNull(dataReader.GetOrdinal("pincode"))) pincode = (string)dataReader["pincode"];

                                TankKaart tankKaartDB = new TankKaart(
                                    (string)dataReader["tankkaartnummer"],
                                    (bool)dataReader["actief"],
                                    dataReader.GetDateTime(dataReader.GetOrdinal("geldigheidsdatum")),
                                    pincode
                                )
                                {
                                    UitgeefDatum = dataReader.GetDateTime(dataReader.GetOrdinal("uitgeefdatum"))
                                };

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

                                    tankKaartDB.VoegBestuurderToe(bestuurderDB);
                                }

                                tankaartenDB.Add(tankKaartDB);
                            }
                        }

                        return tankaartenDB;
                    }
                }
                catch (Exception ex)
                {
                    throw new TankkaartRepositoryADOException("Tankkaarten - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public bool BestaatTankkaart(string tankkaartNummer)
        {
            string query = "select count(*) from Tankkaart where tankkaartnummer=@tankkaartnummer";
            using(SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();
                    command.Parameters.AddWithValue("@tankkaartnummer", tankkaartNummer);
                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n <= 0) return false;
                    return true;
                    
                }
                catch(Exception ex)
                {
                    throw new TankkaartRepositoryADOException("BestaatTankkaart - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }
    }
}
