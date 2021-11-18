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

            string query = "SELECT count(*) FROM Tankkaart WHERE kaartnummer=@kaatnummer";
            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@kaartnummer", SqlDbType.NVarChar));

                    command.Parameters["@kaartnummer"].Value = tankkaart.TankKaartNummer;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new TankkaarRepositoryADOException("BestaatTankkaart- gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public IReadOnlyList<TankKaart> GeefAlleTankkaart() {
            string query = "SELECT * FROM Tankkaart";
            List<TankKaart> kaartLijst = new List<TankKaart>();

            using (SqlCommand command = new SqlCommand(query, Connection)) {
                try {
                    Connection.Open();
                    IDataReader dataReader = command.ExecuteReader();
                    TankKaart k = null;
                    while (dataReader.Read()) {
                        if (k == null) k = new TankKaart((string)dataReader["kaartnummer"], (bool)dataReader["actief"], (DateTime)dataReader["geldigheidsdatum"],
                        (string)dataReader["pincode"]);
                        kaartLijst.Add(k);
                    }
                    dataReader.Close();
                } catch (Exception ex) {
                    throw new TankkaarRepositoryADOException("GetAlleTankkaart niet gelukt", ex);
                } finally {
                    Connection.Close();
                }
            }
            return kaartLijst.AsReadOnly();
        }

        public TankKaart GetTankKaart(string tankkaartNr) {
            string query = "SELECT * FROM Tankkart WHERE kaartnummer=@kaartnummer";

            using (SqlCommand command = new SqlCommand(query, Connection)) {
                try {
                    Connection.Open();
                    command.CommandText = query;
                    SqlParameter paramId = new SqlParameter();
                    paramId.ParameterName = "@kaartnummer";
                    paramId.SqlDbType = SqlDbType.NVarChar;
                    paramId.Value = tankkaartNr;
                    command.Parameters.Add(paramId);

                    SqlDataReader dataReader = command.ExecuteReader();
                    dataReader.Read();
                    string kaartnummer = (string)dataReader["kaartnummer"];
                    DateTime geldigheidsdatum = (DateTime)dataReader["geldigheidsdatum"];
                    string pin = (string)dataReader["pincode"];
                    bool isactief = (bool)dataReader["isactief"];
                    TankKaart tankkaart = new TankKaart(kaartnummer, isactief, geldigheidsdatum, pin);
                    dataReader.Close();
                    return tankkaart;
                } catch (Exception ex) {
                    throw new TankkaarRepositoryADOException("GetTankkaart - gefaald", ex);
                } finally {
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
                    throw new TankkaarRepositoryADOException("UpdateTankkaart - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public void VerwijderTankKaart(TankKaart tankkaart) {

            string query = "DELETE FROM Tankkaart WHERE kaartnummer=@kaartnummer";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@kaartnummer", SqlDbType.NVarChar));
                    command.Parameters["@kaartnummer"].Value = tankkaart.TankKaartNummer;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new TankkaarRepositoryADOException("VerwijderTankkaart - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public void VoegTankKaartToe(TankKaart tankkaart) {

            string query = "INSERT INTO Tankkaart (kaartnummer, geldigheidsdatum, pincode, actief, uitgeefdatum)" +
                           "VALUES (@kaartnummer, @geldigheidsdatum, @pincode, @actief, @uitgeefdatum)";

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
                    throw new TankkaarRepositoryADOException("VoegTankkaarttoe - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public TankKaart ZoekTankKaart(string tankkaartNr, BrandstofType branstof) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TankKaart> ZoekTankKaarten(string tankkaartNr, BrandstofType brandstof) {

            List<TankKaart> kaartLijst = new List<TankKaart>();

            string query = "SELECT Tankkaart.kaartnummer, Brandstoftype.brandstofnaam FROM Tankkaart" +
                " INNER JOIN Brandstoftype ON Tankkaart.kaartnummer=@Tankkaart.kaartnummer ";

            using (SqlCommand command = Connection.CreateCommand()) {
                Connection.Open();
                try {
                    command.Parameters.Add(new SqlParameter("@Tankkaart.kaartnummer ", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@Brandstoftype.brandstofnaam", SqlDbType.NVarChar));

                    command.Parameters["@Tankkaart.kaartnummer"].Value = tankkaartNr;
                    command.Parameters["@Brandstoftype.brandstofnaam"].Value = brandstof;

                    command.CommandText = query;
                    IDataReader dataReader = command.ExecuteReader();
                    TankKaart k = null;
                    while (dataReader.Read()) {
                        if (k == null) k = new TankKaart((string)dataReader["kaartnummer"], (bool)dataReader["actief"], (DateTime)dataReader["geldigheidsdatum"],
                        (string)dataReader["pincode"]);
                        kaartLijst.Add(k);
                    }
                } catch (Exception ex) {
                    throw new TankkaarRepositoryADOException("ZoekTankKaarten niet gelukt", ex);
                } finally {
                    Connection.Close();
                }
            }
            return kaartLijst.AsReadOnly();
        }
    }
}
