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
    public class KleurRepositoryADO : RepoConnection, IVoertuigKleurRepository
    {
        public KleurRepositoryADO(string connectionstring) : base(connectionstring) { }

        public IReadOnlyList<Kleur> GeefAlleVoertuigKleuren()
        {
            string query = "SELECT * FROM Kleur ORDER BY kleurnaam ASC";

            List<Kleur> kleuren = new();

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
                                kleuren.Add(
                                    new Kleur(
                                        (int)dataReader["kleurid"],
                                        (string)dataReader["kleurnaam"]
                                    )
                                );
                            }
                        }

                        return kleuren;
                    }
                }
                catch (Exception ex)
                {
                    throw new KleurRepositoryADOException("Kleuren - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }
    }
}
