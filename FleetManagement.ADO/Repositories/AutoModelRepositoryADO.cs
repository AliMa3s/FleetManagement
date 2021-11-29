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

        public IReadOnlyList<AutoModel> FilterOpAutoModelNaam(string autoModelNaam)
        {
            string query = "SELECT * FROM Brandstoftype ORDER BY brandstofnaam ASC";

            List<AutoModel> autoModellen = new();

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
