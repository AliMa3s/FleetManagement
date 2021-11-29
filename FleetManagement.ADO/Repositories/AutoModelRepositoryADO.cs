using FleetManagement.Interfaces;
using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.Repositories
{
    public class AutoModelRepositoryADO : RepoConnection, IAutoModelRepository
    {
        public AutoModelRepositoryADO(string connectionstring) : base(connectionstring) { }

        public IReadOnlyList<AutoModel> FilterOpAutoModelNaam(string autonaam)
        {
            string query = "SELECT * FROM AutoModel " +
                "WHERE concat(merknaam, ' ', automodelnaam)  LIKE @autonaam + '%' " +
                "ORDER BY merknaam ASC " +
                "OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            //string query = "SELECT * FROM AutoModel ORDER BY merknaam ASC";

            List<AutoModel> autoModellen = new();

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@autonaam", autonaam);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                autoModellen.Add(
                                    new(
                                        (string)dataReader["merknaam"],
                                        (string)dataReader["automodelnaam"],
                                        new AutoType((string)dataReader["autotype"])
                                    )
                                );
                            }
                        }
  
                        return autoModellen;
                    }
                }
                catch (Exception ex)
                {
                    return null; //exception nog aanmaken
                    //throw new AutoModelRepositoryADOException("AutoModellen - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }
    }
}
