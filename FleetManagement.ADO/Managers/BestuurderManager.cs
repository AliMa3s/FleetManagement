using FleetManagement.ADO.Interfaces;
using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;

namespace FleetManagement.ADO.Mangagers
{
    public class BestuurderManager : ConnectionDB, IBestuurderManager
    {
        public BestuurderManager()
        {
            //example
        }

        public Bestuurder RetrieveBestuurder(string rijksRegisterNummer, string query)
        {
            using (SqlConnection conn = new(ConnectionString))
            {
                //ToDo
            }

            return null;
        }
    }
}
