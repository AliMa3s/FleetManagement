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
    public class TankkaartRepositoryADO : ITankkaartRepository {


        private string connectionString = @"YourConnectionStringhere";
        public TankkaartRepositoryADO(string connectionstring) {
            this.connectionString = connectionstring;
        }

        private SqlConnection getConnection() {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public bool BestaatTankKaart(TankKaart tankkaart) {
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM Tankkaart WHERE kaartnummer=@kaatnummer";
            using (SqlCommand command = connection.CreateCommand()) {
                try {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("@kaartnummer", SqlDbType.NVarChar));

                    command.Parameters["@kaartnummer"].Value = tankkaart.TankKaartNummer;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new TankkaarRepositoryADOException("BestaatTankkaart- gefaald", ex);
                } finally {
                    connection.Close();
                }
            }
        }

        public IReadOnlyList<TankKaart> GeefAlleTankkaart() {
            string query = "SELECT * FROM Tankkaart";
            List<TankKaart> kaartLijst = new List<TankKaart>();
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                try {
                    connection.Open();
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
                    connection.Close();
                }
            }
            return kaartLijst.AsReadOnly();
        }

        public TankKaart GetTankKaart(string tankkaartNr) {
            string query = "SELECT * FROM Tankkart WHERE kaartnummer=@kaartnummer";
            SqlConnection connection = getConnection();
            using (SqlCommand command = new SqlCommand(query, connection)) {
                try {
                    connection.Open();
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
                    connection.Close();
                }
            }
        }
        public void UpdateTankKaart(TankKaart tankkaart) {
            throw new NotImplementedException();
        }

        public void VerwijderTankKaart(TankKaart tankkaart) {
            throw new NotImplementedException();
        }

        public void VoegTankKaartToe(TankKaart tankkaart) {
            throw new NotImplementedException();
        }

        public TankKaart ZoekTankKaart(string tankkaartNr, BrandstofType branstof) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TankKaart> ZoekTankKaarten(string tankkaartNr, BrandstofType brandstof) {
            throw new NotImplementedException();
        }
    }
}
