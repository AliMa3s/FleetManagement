using FleetManagement.Interfaces;
using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.Repositories
{
    public class AutoModelRepositoryADO : RepoConnection, IAutoModelRepository {
        public AutoModelRepositoryADO(string connectionstring) : base(connectionstring) { }

        public bool BestaatAutoModelNaam(AutoModel autoModel) {

            string query = "Select count(*) from Automodel " +
                "WHERE merknaam=@merknaam " +
                "AND automodelnaam=@automodelnaam " + 
                "AND autotype=@autotype";

            using SqlCommand command = Connection.CreateCommand();
            try
            {
                Connection.Open();
                command.Parameters.AddWithValue("@merknaam", autoModel.Merk);
                command.Parameters.AddWithValue("@automodelnaam", autoModel.AutoModelNaam);
                command.Parameters.AddWithValue("@autotype", autoModel.AutoType.AutoTypeNaam);

                command.CommandText = query;
                int n = (int)command.ExecuteScalar();
                if (n > 0) return true; else return false;
            }
            catch (Exception ex)
            {
                throw new AutoModelRepositoryADOException("BestaatAutoModel - gefaald!", ex);
            }
            finally
            {
                Connection.Close();
            }
        }

        public bool BestaatAutoModel(int automodelId) {
            string query = "SELECT count(*) FROM Automodel where automodelid=@automodelid";

            using (SqlCommand command = new(query, Connection)) {
                try {

                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@automodelid", SqlDbType.Int));
                    command.Parameters["@automodelid"].Value = automodelId;
                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;

                } catch (Exception ex) {

                    throw new AutoModelRepositoryADOException("BestaatAutoModel(id) - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public IReadOnlyList<AutoModel> FilterOpAutoModelNaam(string autonaam) {
            string query = "SELECT * FROM AutoModel " +
                "WHERE concat(merknaam, ' ', automodelnaam)  LIKE @autonaam + '%' " +
                "ORDER BY merknaam ASC, automodelnaam ASC " +
                "OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            List<AutoModel> autoModellen = new();

            using (SqlCommand command = new(query, Connection)) {
                try {
                    command.Parameters.AddWithValue("@autonaam", autonaam);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader()) {
                        if (dataReader.HasRows) {
                            while (dataReader.Read()) {
                                autoModellen.Add(
                                    new(
                                        (int)dataReader["automodelid"],
                                        (string)dataReader["merknaam"],
                                        (string)dataReader["automodelnaam"],
                                        new AutoType((string)dataReader["autotype"])
                                    )
                                );
                            }
                        }

                        return autoModellen;
                    }
                } catch (Exception ex) {

                    throw new AutoModelRepositoryADOException("AutoModellen - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public IReadOnlyList<AutoModel> ZoekOpAutoType(AutoType autoType)
        {
            string query = "SELECT * FROM AutoModel " +
                "WHERE autotype = @autotype " +
                "ORDER BY merknaam ASC, automodelnaam ASC " +
                "OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            List<AutoModel> autoModellen = new();

            using (SqlCommand command = new(query, Connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@autotype", autoType.AutoTypeNaam);
                    Connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                autoModellen.Add(
                                    new(
                                        (int)dataReader["automodelid"],
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

                    throw new AutoModelRepositoryADOException("AutoModellen - gefaald", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public void UpdateAutoModel(AutoModel autoModel) {
            string query = "UPDATE Automodel SET merknaam=@merknaam,automodelnaam=@automodelnaam,autotype=@autotype " +
                    " where automodelid = @automodelid";

            using (SqlCommand command = new(query, Connection)) {
                try {

                    Connection.Open();
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@automodelid", autoModel.AutoModelId);
                    command.Parameters.AddWithValue("@merknaam", autoModel.Merk);
                    command.Parameters.AddWithValue("@automodelnaam", autoModel.AutoModelNaam);
                    command.Parameters.AddWithValue("@autotype", autoModel.AutoType.AutoTypeNaam);
                    command.ExecuteNonQuery();

                } catch (Exception ex) {

                    throw new AutoModelRepositoryADOException("UpdateAutoModel - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public void VerwijderAutoModel(AutoModel autoModel) {
            string query = "DELETE FROM Automodel WHERE automodelid=@automodelid";

            using (SqlCommand command = Connection.CreateCommand()) {
                try {
                    Connection.Open();
                    command.Parameters.Add(new SqlParameter("@automodelid", SqlDbType.Int));
                    command.Parameters["@automodelid"].Value = autoModel.AutoModelId;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new AutoModelRepositoryADOException("VerwijderAutoModel - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public void VoegAutoModelToe(AutoModel autoModel) {
            string query = "INSERT INTO Automodel (merknaam,automodelnaam,autotype) VALUES (@merknaam, @automodelnaam,@autotype)";

            using (SqlCommand command = new(query, Connection)) {
                try {

                    Connection.Open();
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@merknaam", autoModel.Merk);
                    command.Parameters.AddWithValue("@automodelnaam", autoModel.AutoModelNaam);
                    command.Parameters.AddWithValue("@autotype", autoModel.AutoType.AutoTypeNaam);
                    command.ExecuteNonQuery();


                } catch (Exception ex) {

                    throw new AutoModelRepositoryADOException("VoegAutoModelToe - gefaald", ex);
                } finally {
                    Connection.Close();
                }
            }
        }

        public bool IsAutoModelInGebruik(AutoModel autoModel)
        {
            string query = "Select count(*) from Automodel a " +
                "JOIN Voertuig v ON a.automodelid = v.automodelid " +
                "WHERE a.automodelid = @automodelid";

            using (SqlCommand command = Connection.CreateCommand())
            {
                try
                {
                    Connection.Open();
                    command.Parameters.AddWithValue("@automodelid", autoModel.AutoModelId);

                    command.CommandText = query;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new AutoModelRepositoryADOException("Is AutoModel In Gebruik - gefaald!", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }
    }
}
