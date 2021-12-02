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
            Console.WriteLine("Bestuurder toegevoegd!");//done 
            Bestuurder bu = new Bestuurder(3, "Filip", "Updated", "1976-03-31", "B", "76033101986");
            //bsd.UpdateBestuurder(bu);
            Console.WriteLine("Bestuurder Geüpdatet!");//done 
            //bsd.VerwijderBestuurder(b); //done
            if (bsd.BestaatBestuurder(3)){
                Console.WriteLine("Bestuurder bestaat!");
            } else {
                Console.WriteLine("Bestuurder bestaat niet!");
            }//checked

             AdresRepositoryADO ado = new AdresRepositoryADO(connectionstring);
            Adres ad = new Adres("stratenstraat", "2", "5000", "Hasselt");
            //ado.VoegAdresToe(ad);
            Console.WriteLine("Adres toegvoegd!");//done
            Adres upAd = new Adres("stationstraat", "5", "3500", "Hasselt");
            upAd.VoegIdToe(1);
            //ado.UpdateAdres(upAd);
            Console.WriteLine("Adres Geüpdatet!");//done
            //ado.VerwijderAdres(upAd);
            Console.WriteLine("Adres Geüpdatet!");//done
            if (ado.BestaatAdres(upAd)) {
                Console.WriteLine("Adres bestaat!");
            } else {
                Console.WriteLine("Adres bestaat niet!");
            }//checked

            //Het probleem is hier, je checkt chassis & nummerplaat niet meer in de ctor door te de class te hebben aangepast
            //VoertuigRepositoryADO vrt = new VoertuigRepositoryADO(connectionstring);
            //AutoType autotype = new AutoType("Sedan");
            //bool ishybride = true;
            //BrandstofType brandstof = new BrandstofType(1,"Benzin");
            //AutoModel automodel = new AutoModel(1,"BMW", "X1", autotype);
            //Voertuig v1 = new Voertuig(automodel, "WAUZZZ8V5KA106598","1ALI007" ,AantalDeuren.Drie, brandstof);
            //vrt.VoegVoertuigToe(v1);
            //Console.WriteLine("Voertuig toegevoegd!");

            //Kijk eens hoe het moet Ali

            VoertuigRepositoryADO vrt = new VoertuigRepositoryADO(connectionstring);

            AutoType autotype = new AutoType("Sedan");
            AutoModel automodel = new AutoModel("BMW", "X1", autotype);

            //Dit ali is brandstof voor Voertuig. Er zijn één of twee brandstoffen mogelijk maar ook niet meer. Daarom deze oplossing
            bool ishybride = true;
            BrandstofVoertuig brandstof = new("Benzine", ishybride);

            //Ali: op het einde onze brandstof class
            string chassisnummer = "WAUZZZ8V5KA106598";
            Voertuig v1 = new Voertuig(automodel, "WAUZZZ8V5KA106598", "1ALI007", brandstof);

            //Altijd checken dat Voertuig bestaat, hetzelfde chassisnummer & nummerplaat gooit een exception in DB
            //Deze manager doet nog niet wat hij zou moeten doen: dat is toDo
            bool isVoertuigOK = vrt.BestaatVoertuig(v1);

            if (isVoertuigOK)
                vrt.VoegVoertuigToe(v1);

            //Helaas test je iets wat nog niet naar behoren werkt in Manager & DB
            //
            //Voertuig haalVoertuig = vrt.GetVoertuig(chassisnummer);
            //verwijderVoertuig = vrt.VerwijderVoertuig(haalVoertuig);  //nu ben je zeker dat je de juiste verwijdert
        }
    }
}
