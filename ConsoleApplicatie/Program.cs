using FleetManagement.ADO.Repositories;
using FleetManagement.Interfaces;
using FleetManagement.Manager;
using FleetManagement.Model;
using System;

namespace ConsoleApplicatie
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string connectionstring = @"Data Source=.\SQLEXPRESS;Initial Catalog=fleetManagement;Integrated Security=True";
            BestuurderRepositoryADO bsd = new BestuurderRepositoryADO(connectionstring);
            Bestuurder b = new Bestuurder(1, "Filip", "Rigoir", "1976-03-31", "B", "1514081390", "76033101986");
            bsd.VoegBestuurderToe(b);
            Console.WriteLine("Bestuurder teogevoegd!");//done 


        }
    }
}
