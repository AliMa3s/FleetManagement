using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;

namespace FleetManagement.ADO.Repositories
{
    public class BestuurderManager : ConnectionDB
    {
        public BestuurderManager()
        {
            //example
        }

        public Bestuurder AddBestuurder(Bestuurder bestuurder)
        {
            using SqlConnection conn = new(ConnectionString);
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
