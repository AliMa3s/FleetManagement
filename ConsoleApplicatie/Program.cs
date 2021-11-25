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

#warning Gelieve eerst toe te voegen zonder ID. Daarna ophalen via rijksregisternummer en dan dit object verwijderen. Dan verwijder je altijd de juiste ID 
            string connectionstring = @"Data Source=.\SQLEXPRESS;Initial Catalog=fleetManagement;Integrated Security=True";
            BestuurderRepositoryADO bsd = new BestuurderRepositoryADO(connectionstring);
            //Bestuurder b = new Bestuurder(4, "Ahmeti", "Yilmaiz", "1976-03-10", "B", "1514081390", "76031010956");//teammeeting bespreken
            //bsd.VoegBestuurderToe(b);
            Console.WriteLine("Bestuurder toegevoegd!");//done 
            Bestuurder bu = new Bestuurder(3, "Filip", "Updated", "1976-03-31", "B", "1514081390", "76033101986");
            //bsd.UpdateBestuurder(bu);
            Console.WriteLine("Bestuurder Geüpdatet!");//done 
            //bsd.VerwijderBestuurder(b); //done
            if (bsd.BestaatBestuurder(3)){
                Console.WriteLine("Bestuurder bestaat!");
            } else {
                Console.WriteLine("Bestuurder bestaat niet!");
            }//checked

            bsd.Dispose();

            AdresRepositoryADO ado = new AdresRepositoryADO(connectionstring);
            Adres ad = new Adres("stratenstraat", "2", "5000", "Hasselt");
            ado.VoegAdresToe(ad);
            Console.WriteLine("Adres toegvoegd!");//done
            Adres upAd = new Adres(1,"stationstraat", "5", "3500", "Hasselt");
            ado.UpdateAdres(upAd);
            Console.WriteLine("Adres Geüpdatet!");//done
            ado.VerwijderAdres(upAd);
            Console.WriteLine("Adres Geüpdatet!");//done
            if (ado.BestaatAdres(upAd)) {
                Console.WriteLine("Adres bestaat!");
            } else {
                Console.WriteLine("Adres bestaat niet!");
            }//checked

            ado.Dispose();
        }
    }
}
