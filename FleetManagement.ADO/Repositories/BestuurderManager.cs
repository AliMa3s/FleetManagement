using FleetManagement.ADO.Repositories.Connections;
using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;

namespace FleetManagement.ADO.Repositories
{
    public class BestuurderManager
    {
        private readonly BestuurderConfig _config;

        public BestuurderManager()
        {
            _config = new();
        }

        public Bestuurder AddBestuurder(Bestuurder bestuurder)
        {
            using SqlConnection conn = new(_config.ConnectionString);
            try
            {
                //ToDo
            }
            catch
            {

            }
            finally
            {

            }

            return null; //is momenteel Null voor geen compilefout te hebben
        }
    }
}
