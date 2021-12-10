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

            string query = "SELECT count(*) FROM Adres WHERE adresid=@adresid";
            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@adresid", SqlDbType.NVarChar));
                    command.Parameters["@adresid"].Value = adres.AdresId;

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

        public void UpdateAdres(Adres adres) {

            string query = "UPDATE Adres " +
                           "SET straat=@straat, nummer=@nummer, postcode=@postcode, gemeente=@gemeente " +
                           "WHERE adresid=@adresid";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@straat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@nummer", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@postcode", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@gemeente", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@adresid", SqlDbType.Int));

                    command.Parameters["@straat"].Value = adres.Straat;
                    command.Parameters["@nummer"].Value = adres.Nr;
                    command.Parameters["@postcode"].Value = adres.Postcode;
                    command.Parameters["@gemeente"].Value = adres.Gemeente;
                    command.Parameters["@adresid"].Value = adres.AdresId;

                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new AdresRepositoryADOException("UpdateAdres - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }
    }
}
