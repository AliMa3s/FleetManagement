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
            //Bestuurder b = new Bestuurder(4, "Ahmeti", "Yilmaiz", "1976-03-10", "B", "1514081390", "76031010956");//teammeeting bespreken
            //bsd.VoegBestuurderToe(b);
            Console.WriteLine("Bestuurder teogevoegd!");//done 
            Bestuurder bu = new Bestuurder(3, "Filip", "Updated", "1976-03-31", "B", "1514081390", "76033101986");
            //bsd.UpdateBestuurder(bu);
            Console.WriteLine("Bestuurder Updated!");//done 
            //bsd.VerwijderBestuurder(b); //done
            if (bsd.BestaatBestuurder(3)){
                Console.WriteLine("Bestuur bestaat!");
            } else {
                Console.WriteLine("Bestuurder bestaat niet!");
            }//checked
            AdresRepositoryADO ado = new AdresRepositoryADO(connectionstring);
            Adres ad = new Adres("stratenstraat", "2", "5000", "Hasselt");
            //ado.VoegAdresToe(ad);
            Console.WriteLine("Adres toegveogd!");//done
            Adres upAd = new Adres(1,"stationstraat", "5", "3500", "Hasselt");
            //ado.UpdateAdres(upAd);
            Console.WriteLine("Adres Updated!");//done
            //ado.VerwijderAdres(upAd);
            Console.WriteLine("Adres Updated!");//done
            if (ado.BestaatAdres(upAd)) {
                Console.WriteLine("Adres bestaat!");
            } else {
                Console.WriteLine("Adres bestaat niet!");
            }//checked
        }
    }
}
