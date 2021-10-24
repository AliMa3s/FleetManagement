using FleetManagement.ADO.Interfaces;
using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;

namespace FleetManagement.ADO.Managers
{
    public class BestuurderManager : ConnectionDB, IBestuurderManager
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
