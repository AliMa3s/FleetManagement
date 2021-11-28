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
            string query = "SELECT * FROM Tankkaart " +
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

        public TankKaart GetTankKaart(string tankkaartNr) {
            string query = "SELECT * FROM Tankkaart WHERE tankkaartnummer=@tankkaartnummer";

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

        public void VoegTankKaartToe(TankKaart tankkaart) {

            string query = "INSERT INTO Tankkaart (tankkaartnummer, geldigheidsdatum, pincode, actief, uitgeefdatum) " +
                           "VALUES (@tankkaartnummer, @geldigheidsdatum, @pincode, @actief, @uitgeefdatum)";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@tankkaartnummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@geldigheidsdatum", SqlDbType.Date));
                    command.Parameters.Add(new SqlParameter("@pincode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@actief", SqlDbType.Bit));
                    command.Parameters.Add(new SqlParameter("@uitgeefdatum", SqlDbType.Date));

                    command.Parameters["@tankkaartnummer"].Value = tankkaart.TankKaartNummer;
                    command.Parameters["@geldigheidsdatum"].Value = tankkaart.GeldigheidsDatum;
                    command.Parameters["@actief"].Value = tankkaart.Actief;
                    command.Parameters["@uitgeefdatum"].Value = tankkaart.UitgeefDatum;

                    if(tankkaart.Pincode == null)
                    {
                        command.Parameters["@pincode"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@pincode"].Value = tankkaart.Pincode;
                    }

                    command.CommandText = query;
                    command.ExecuteNonQuery();

                } catch (Exception ex) {
                    throw new TankkaartRepositoryADOException("VoegTankkaarttoe - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public TankKaart ZoekTankKaart(string tankkaartNr, BrandstofType branstof) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TankKaart> ZoekTankKaarten(bool isGeldig)
        {
            string query = "SELECT * FROM Tankkaart " +
                "WHERE actief = @isGeldig " +
                "ORDER BY tankkaartnummer ASC " +
                "OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            List<TankKaart> tankaartenDB = new List<TankKaart>();

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@isGeldig", isGeldig);
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

                                tankaartenDB.Add(tankKaartDB);
                            }
                        }

                        return tankaartenDB;
                    }
                }
                catch (Exception ex)
                {
                    throw new BrandstofRepositoryADOException("Brandstoffen - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }
    }
}
