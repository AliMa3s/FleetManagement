using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;

namespace FleetManagement.ADO.Repositories
{
    public class BestuurderManager
    {
        private readonly ConnectionDB _connectieDB;

        public BestuurderManager()
        {
            _connectieDB = new();

            //example
        }

        public Bestuurder AddBestuurder(Bestuurder bestuurder)
        {
            using SqlConnection conn = new(_connectieDB.ConnectionString);
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
