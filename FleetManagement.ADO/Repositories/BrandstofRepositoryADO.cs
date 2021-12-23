using FleetManagement.ADO.RepositoryExceptions;
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
    public class BrandstofRepositoryADO : RepoConnection, IBrandstofRepository
    {
        public BrandstofRepositoryADO(string connectionstring) : base(connectionstring) { }

        public IReadOnlyList<BrandstofType> GeeAlleBrandstoffen()
        {
            string query = "SELECT * FROM Brandstoftype ORDER BY brandstofnaam ASC";

            List<BrandstofType> brandstoffen = new();

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    Connection.Open();

                    using SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            brandstoffen.Add(
                                new BrandstofType(
                                    (int)dataReader["brandstoftypeid"],
                                    (string)dataReader["brandstofnaam"]
                                )
                            );
                        }
                    }

                    return brandstoffen;
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
