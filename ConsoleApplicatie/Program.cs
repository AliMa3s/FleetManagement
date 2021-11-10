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
             
            IUnitOfRepository repos = new UnitOfRepository(connectionstring);  //Maak ADO met alle repos
            IUnitOfManager managers = new UnitOfManager(repos);  //Maak Managers (erft ook over van IUnitOfRepository)


            //Indien gelezen mag dat allemaal weg: is om te communiceren met Ali & Ahmet
            /************************************/
            //Idee van Ali was alles tesamen 
            //Idee van Filip was met Bouwers 
            //Is alle twee geworden onder één beheer (maar toch staat alles apart voor onderhoud)
            //Dus in managers zitten alle managers & bouwers (is toegevoegd aan de pattern om in regel te zijn)
            //** Voertuig & VoertuigBouwer is al gedaan, meer werk hieraan is er niet. Is klaar voor formulier te maken in WPF, te checken en te posten **
            //(behalve extra interfaces (in het rood) & testen zijn nog niet gedaan)

            /* Voorbeelden:
            * managers.Voertuigen.BestaatVoertuig();
            * managers.VoertuigBouwer; 
            * managers.Bestuurders...
            * Etc...
            * 
            * Een voorbeeld zal duidelijk worden hoe eenvoudig het wel is. 
            * Je moet maar een punt zetten en je kan de props & methods kiezen uit de lijst
            */
            /**************************************/

        }
    }
}
