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
            List<BrandstofType> brandstoffenInDB = new List<BrandstofType>();

            Connection.Open();
            using (SqlTransaction transaction = Connection.BeginTransaction())
            {
                try
                {
                    VoegTankKaartToe(tankkaart, Connection, transaction);
                    TransactionBrandstoffen(brandstoffenInDB, tankkaart, Connection, transaction);
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

            string query = "INSERT INTO Tankkaart (tankkaartnummer, geldigheidsdatum, pincode, actief, uitgeefdatum) " +
                           "VALUES (@tankkaartnummer, @geldigheidsdatum, @pincode, @actief, @uitgeefdatum)";

            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    if (transaction != null) command.Transaction = transaction;
                    if (Connection.State != ConnectionState.Open) Connection.Open();

                    command.Parameters.Add(new SqlParameter("@tankkaartnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geldigheidsdatum", SqlDbType.Date));
                    command.Parameters.Add(new SqlParameter("@pincode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@actief", SqlDbType.Bit));
                    command.Parameters.Add(new SqlParameter("@uitgeefdatum", SqlDbType.Date));

                    command.Parameters["@tankkaartnummer"].Value = tankkaart.TankKaartNummer;
                    command.Parameters["@geldigheidsdatum"].Value = tankkaart.GeldigheidsDatum;
                    command.Parameters["@actief"].Value = tankkaart.Actief;
                    command.Parameters["@uitgeefdatum"].Value = tankkaart.UitgeefDatum;

                    if (tankkaart.Pincode == null)
                    {
                        command.Parameters["@pincode"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@pincode"].Value = tankkaart.Pincode;
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

        //tansaction van brandstoffen toevoegen en/of verwijderen
        private void TransactionBrandstoffen(IReadOnlyList<BrandstofType> brandstofTypes, TankKaart tankKaart,
            SqlConnection sqlConnection = null, SqlTransaction transaction = null)
        {
            List<BrandstofType> verwijderBrandstofDB = new List<BrandstofType>();

            if (brandstofTypes.Count > 0)
            {
                //Calculeer het verschil van brandstof update vs brandstof in DB
                brandstofTypes.ToList().ForEach(brandstof => {
                    if (tankKaart.IsBrandstofAanwezig(brandstof))
                    {
                        //Zit in DB dus moet niet toegevoegd worden
                        tankKaart.VerwijderBrandstof(brandstof);
                    }
                    else
                    {
                        //Staat overbodig in DB
                        verwijderBrandstofDB.Add(brandstof);
                    }
                });
            }

            //Verwijder alle overbodige brandstoffen in DB
            int removes = VerwijderBrandstoffen(verwijderBrandstofDB, tankKaart.TankKaartNummer, sqlConnection, transaction);

            //voeg nieuwe brandstof toe dat nog niet in DB staat
            VoegBrandstoffenToe(tankKaart.Brandstoffen, tankKaart.TankKaartNummer, sqlConnection, transaction);
        }

        private void VoegBrandstoffenToe(List<BrandstofType> brandstoffen, string tankkaartNummer, SqlConnection sqlConnection = null, SqlTransaction transaction = null)
        {
            if (brandstoffen.Count > 0)
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
                        brandstoffen.ForEach(brandstof => {

                            query = "INSERT INTO Tankkaart_Brandstoftype (tankkaartnummer, brandstoftypeid) " +
                            $"VALUES (@tankkaartnummer{i}, @brandstoftypeid{i}); ";

                            command.Parameters.Add(new SqlParameter($"@tankkaartnummer{i}", SqlDbType.NVarChar));
                            command.Parameters.Add(new SqlParameter($"@brandstoftypeid{i}", SqlDbType.Int));
                            command.Parameters[$"@tankkaartnummer{i}"].Value = tankkaartNummer;
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

        private int VerwijderBrandstoffen(List<BrandstofType> brandstofTypes, string tankkaartNummer,
            SqlConnection sqlConnection = null, SqlTransaction transaction = null)
        {
            if (brandstofTypes.Count > 0)
            {
                if (sqlConnection != null)
                {
                    Connection = sqlConnection;
                }

                string query = "";

                using (SqlCommand command = Connection.CreateCommand())
                {
                    if (transaction != null) command.Transaction = transaction;
                    if (Connection.State != ConnectionState.Open) Connection.Open();

                    try
                    {
                        if (transaction != null) command.Transaction = transaction;
                        if (Connection.State != ConnectionState.Open) Connection.Open();

                        int i = 1;
                        brandstofTypes.ForEach(brandstof => {

                            query = "DELETE FROM Tankkaart_Brandstoftype " +
                                $"WHERE tankkaartnummer=@tankkaartnummer{i} AND brandstoftypeid=@brandstoftypeid{i}; ";

                            command.Parameters.Add(new SqlParameter($"@tankkaartnummer{i}", SqlDbType.NVarChar));
                            command.Parameters.Add(new SqlParameter($"@brandstoftypeid{i}", SqlDbType.Int));
                            command.Parameters[$"@tankkaartnummer{i}"].Value = tankkaartNummer;
                            command.Parameters[$"@brandstoftypeid{i}"].Value = brandstof.BrandstofTypeId;

                            command.CommandText += query;

                            i++;
                        });

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new TankkaartRepositoryADOException("Verwijder brandstof - gefaald", ex);
                    }
                    finally
                    {
                        if (sqlConnection is null) Connection.Close();
                    }
                } 
            }
           
            return 0;
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
                                         (string)dataReader["rijbewijsnummer"],
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


#warning add bij interfaces
        public IReadOnlyList<BrandstofType> BrandstoffenTankaart(TankKaart tankkaart)
        {
            //quey ophalen
            return null;
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

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("bestuurderid")))
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
                           " geldigheidsdatum=@geldigheidsdatum, pincode=@pincode, actief=@actief " +
                           " WHERE kaartnummer=@kaartnummer";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@kaartnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geldigheidsdatum", SqlDbType.DateTime));
                    command.Parameters.Add(new SqlParameter("@pincode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@actief", SqlDbType.Bit));
                    //command.Parameters.Add(new SqlParameter("@uitgeefdatum", SqlDbType.Timestamp));

                    command.Parameters["@kaartnummer"].Value = tankkaart.TankKaartNummer;
                    command.Parameters["@geldigheidsdatum"].Value = tankkaart.GeldigheidsDatum;
                    command.Parameters["@pincode"].Value = tankkaart.Pincode;
                    command.Parameters["@actief"].Value = tankkaart.Actief;
                    //command.Parameters["@uitgeefdatum"].Value = tankkaart.UitgeefDatum;

                    command.CommandText = query;
                    command.ExecuteNonQuery();

                } catch (Exception ex) {
                    throw new TankkaartRepositoryADOException("UpdateTankkaart - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

#warning tankkaart mag nooit verwijderd worden
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

        public IReadOnlyList<TankKaart> TankaartenZonderBestuurder() {
            string query = "SELECT * FROM Tankkaart t " +
                "LEFT JOIN Bestuurder b ON t.bestuurderid = b.bestuurderid " +
                "WHERE t.actief == true AND t.bestuurderid IS NULL" +
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
                                         (string)dataReader["rijbewijsnummer"],
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
                                         (string)dataReader["rijbewijsnummer"],
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
    }
}
