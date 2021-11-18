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
    public class AdresRepositoryADO : RepoConnection, IAdresRepository {

        public AdresRepositoryADO(string connectionstring) : base(connectionstring) { }

        public bool BestaatAdres(Adres adres) {

            string query = "SELECT count(*) FROM Adres WHERE straat=@straat AND nummer=@nummer AND postcode=@postcode AND gemeente=@gemeente";
            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@straat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@postcode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@gemeente", SqlDbType.NVarChar));

                    command.Parameters["@straat"].Value = adres.Straat;
                    command.Parameters["@nummer"].Value = adres.Nr;
                    command.Parameters["@postcode"].Value = adres.Postcode;
                    command.Parameters["@gemeente"].Value = adres.Gemeente;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new AdresRepositoryADOException("BestaatAdres- gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public bool BestaatAdres(int adresId) {

            string query = "SELECT count(*) FROM Adres WHERE adresId=@adresId";
            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@adresId", SqlDbType.Int));

                    command.Parameters["@adresId"].Value = adresId;

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex) {
                    throw new AdresRepositoryADOException("BestaatAdres(int-id)- gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public void UpdateAdres(Adres adres) {

            string query = "UPDATE Adres " +
                           " SET straat=@straat, nummer=@nummer, postcode=@postcode, gemeente=@gemeente " +
                           " WHERE adresId=@adresId";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@straat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@postcode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@gemeente", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@adresId", SqlDbType.Int));

                    command.Parameters["@straat"].Value = adres.Straat;
                    command.Parameters["@nummer"].Value = adres.Nr;
                    command.Parameters["@postcode"].Value = adres.Postcode;
                    command.Parameters["@gemeente"].Value = adres.Gemeente;
                    command.Parameters["@adresId"].Value = adres.AdresId;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new AdresRepositoryADOException("UpdateAdres - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public void VerwijderAdres(Adres adres) {

            string query = "DELETE FROM Adres WHERE adresId=@adresId";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@adresId", SqlDbType.Int));
                    command.Parameters["@adresId"].Value = adres.AdresId;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new AdresRepositoryADOException("VerwijderAdres - gefaald", ex);
                } finally {
                    Connection.Close();                }
            }
        }

        public void VoegAdresToe(Adres adres) {

            string query = "INSERT INTO Adres (straat, nummer, postcode, gemeente)" +
                "VALUES (@straat, @nummer, @postcode, @gemeente)";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@straat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@postcode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@gemeente", SqlDbType.NVarChar));

                    command.Parameters["@straat"].Value = adres.Straat;
                    command.Parameters["@nummer"].Value = adres.Nr;
                    command.Parameters["@postcode"].Value = adres.Postcode;
                    command.Parameters["@gemeente"].Value = adres.Gemeente;

                    command.CommandText = query;
                    command.ExecuteNonQuery();

                } catch (Exception ex) {
                    throw new AdresRepositoryADOException("VoegAdresToe(adres)- gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }
    }
}
