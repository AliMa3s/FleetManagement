using FleetManagement.ADO.Repositories;
using FleetManagement.Interfaces;
using FleetManagement.Manager;
using System;

namespace ConsoleApplicatie
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string connectionstring = @"Data Source=.\SQLEXPRESS;Initial Catalog=fleetManagement;Integrated Security=True";
             
            //UnitOfManager & UnitOfRepository is idd voor WPF 
            //Zullen we later zien als WPF is aangemaakt
            //Hier komt idd VoertuigRepositoryADO, en de rest ADO 
            //Deze tekst mag dan weg

        }
    }
}
