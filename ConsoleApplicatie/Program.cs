using FleetManagement.ADO.Repositories;
using FleetManagement.Interfaces;
using FleetManagement.Manager;
using FleetManagement.Model;
using System;
using System.Text;

namespace ConsoleApplicatie
{
    class Program
    {
        private static string _connectionstring = "Data Source=.\\SQLEXPRESS;Initial Catalog=fleetManagement;Integrated Security=True; MultipleActiveResultSets=True";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            BestuurderRepoTest();
        }

        public static void BestuurderRepoTest()
        {
            //Maak repo bestuurder
            BestuurderRepositoryADO repo = new(_connectionstring);

            /* 
             * Maak een Bestuurder aan zonder ID want dit wordt automatisch toegekend in DB
             */
            string rijksregisterNr = "42012363747";
            Bestuurder nieuwBestuurder = new("Eddy", "Wally", "1942-01-23", "B, C", rijksregisterNr);  
            Adres bestuurderAdres = new("Markt Kramerlaan","200","5032","Voicelare");

            //Geef bestuurder een adres: transactie doet de post indien adres aanwezig is
            nieuwBestuurder.Adres = bestuurderAdres;

            //Na de post ontvang je bestuurder terug met het ID nummer bij
            Bestuurder bestuurderDB = repo.VoegBestuurderToe(nieuwBestuurder);

            Console.WriteLine("Bestuurder toegevoegd en kreeg ID nummer: " + bestuurderDB.BestuurderId);

            //Haal het object op via rijksregister om te controleren dat Bestuurder is gepost in DB
            Bestuurder b = repo.ZoekBestuurder(rijksregisterNr);

            if(b != null)
            {
                //Geef alle velden weer die in database staan

                StringBuilder str = new();
                str.AppendLine(b.Voornaam + " " + b.Achternaam);
                str.AppendLine(b.GeboorteDatum);
                str.AppendLine(b.TypeRijbewijs);
                str.AppendLine(b.RijksRegisterNummer);
                str.AppendLine(b.Adres.Straat + " " + b.Adres.Nr + " - " + b.Adres.Postcode + " " + b.Adres.Gemeente);

                Console.WriteLine(str.ToString());

                //Update het Bestuurder en maak een nieuw object aan zoals deze binnenkomt via API/WPF
                //Belangrijk is de ID nummer mee te geven anders wordt deze niet herkent

                Bestuurder bestuurderUpdaten = new(
                    b.BestuurderId,
                    "Joske",
                    "Vermeulen",
                    "2005/05/12",
                    "D",
                    "05051299971"
                );

                bestuurderUpdaten.Adres = new("Bloemenstraat", "16", "4445", "Vosselare");

                repo.UpdateBestuurder(bestuurderUpdaten);

                Console.WriteLine("Bestuurder succesvol geüpdatet ");

                //Haal nieuwe gegevens op in database
                b = repo.ZoekBestuurder("05051299971");

                //Geef alle velden weer van de update

                str = new();
                str.AppendLine(b.Voornaam + " " + b.Achternaam);
                str.AppendLine(b.GeboorteDatum);
                str.AppendLine(b.TypeRijbewijs);
                str.AppendLine(b.RijksRegisterNummer);
                str.AppendLine(b.Adres.Straat + " " + b.Adres.Nr + " - " + b.Adres.Postcode + " " + b.Adres.Gemeente);

                Console.WriteLine(str.ToString());

                //verwijder Bestuurder
                repo.VerwijderBestuurder(b);

                //Zoek op de twee gebruikte rijksregisternummers, die moeten verwijderd zijn
                Bestuurder metEersteRijksnr = repo.ZoekBestuurder(rijksregisterNr);

                if(metEersteRijksnr == null)
                {
                    Console.WriteLine("Bestuurder succesvol overschreven");
                }
                else
                {
                    Console.WriteLine($"Oeps... rijksregisternummer {rijksregisterNr} is niet verwijderd");
                }

                Bestuurder metTweedeRijksnr = repo.ZoekBestuurder("05051299971");

                if (metTweedeRijksnr == null)
                {
                    Console.WriteLine("Bestuurder succesvol verwijderd");
                }
                else
                {
                    Console.WriteLine($"Oeps... rijksregisternummer 05051299971 is niet verwijderd");
                }
            }
            else
            {
                Console.WriteLine("Bestuurder op rijksregisternummer werd niet gevonden");
            }

            repo = null;
        }
    }
}
