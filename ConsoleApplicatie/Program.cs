using FleetManagement.ADO.Repositories;
using FleetManagement.Interfaces;
using FleetManagement.Manager;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplicatie
{
    class Program
    {
        private static string _connectionstring = "Data Source=.\\SQLEXPRESS;Initial Catalog=fleetManagement;Integrated Security=True; MultipleActiveResultSets=True";

        static void Main(string[] args)
        {
            Console.WriteLine("Test Bestuurder Repo: ");
            BestuurderRepoTest();

            Console.WriteLine("Test Tankkaart Repo: ");
            TankkaartRepoTest();

            Console.WriteLine("Test Voertuig Repo: ");
            VoertuigRepoTest();
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
            Bestuurder b = repo.VoegBestuurderToe(nieuwBestuurder);

            Console.WriteLine("Bestuurder toegevoegd en kreeg ID nummer: " + b.BestuurderId);

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

                //Update Bestuurder en maak een nieuw object aan zoals deze binnenkomt via API/WPF
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

                //Zoek op verwijderd object in DB
                bool metTweedeRijksnr = repo.BestaatRijksRegisterNummer("05051299971");

                if (!metTweedeRijksnr)
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

            Console.WriteLine("");
            repo = null;
        }

        public static void TankkaartRepoTest()
        {
            //Maak repo bestuurder
            TankkaartRepositoryADO repo = new(_connectionstring);


        }

        public static void VoertuigRepoTest()
        {
            //Maak repo bestuurder
            VoertuigRepositoryADO voertuigRepo = new(_connectionstring);
            AutoModelRepositoryADO autoModelrepo = new(_connectionstring);
            BrandstofRepositoryADO brandstofRepo = new(_connectionstring);

            List<AutoModel> models = (List<AutoModel>)autoModelrepo.FilterOpAutoModelNaam("");
            List<BrandstofType> brandstoffen = (List<BrandstofType>)brandstofRepo.GeeAlleBrandstoffen();

            if(models.Count > 1 && brandstoffen.Count > 1)
            {
                BrandstofVoertuig brandstof = new(
                    brandstoffen[0].BrandstofTypeId,
                    brandstoffen[0].BrandstofNaam,
                    false
                );

                //Maak aan en voeg toe
                Voertuig nieuwVoertuig = new(models[0], "AAAAAAAAAA1AA1111", "1AAA111", brandstof);
                Voertuig voertuigDB = voertuigRepo.VoegVoertuigToe(nieuwVoertuig);

                Console.WriteLine("Voertuig succesvol toegevoegd en kreeg ID " + voertuigDB.VoertuigId);

                //Haal het voertuig op in db via chassisnummer
                Voertuig v = voertuigRepo.ZoekOpNummerplaatOfChassisNummer("AAAAAAAAAA1AA1111");

                StringBuilder str = new();
                str.AppendLine(v.VoertuigNaam);
                str.AppendLine(v.NummerPlaat);
                str.AppendLine(v.ChassisNummer);
                str.AppendLine(v.Brandstof.BrandstofNaam);
                str.AppendLine(v.Brandstof.Hybride.ToString());
                Console.WriteLine(str.ToString());

                //Update Voertuig: maak een nieuw instantie en injecteer ID nummer erin
                BrandstofVoertuig brandstof2 = new(
                    brandstoffen[1].BrandstofTypeId,
                    brandstoffen[1].BrandstofNaam, 
                    true
                );

                //Maak aan en voeg toe
                Voertuig UpdateVoertuig = new(v.VoertuigId, models[1], "ZZZZZZZZZZ9ZZ9999", "2ZZZ999", brandstof2);
                voertuigRepo.UpdateVoertuig(UpdateVoertuig, "ZZZZZZZZZZ9ZZ9999", "2ZZZ999");

                Console.WriteLine("Voertuig succesvol geüpdatet ");

                //Zoek updated ID opnieuw met de nieuwe nummerplaat
                v = voertuigRepo.ZoekOpNummerplaatOfChassisNummer("2ZZZ999");

                str = new();
                str.AppendLine(v.VoertuigNaam);
                str.AppendLine(v.NummerPlaat);
                str.AppendLine(v.ChassisNummer);
                str.AppendLine(v.Brandstof.BrandstofNaam);

                str.AppendLine(v.Brandstof.Hybride.ToString());
                Console.WriteLine(str.ToString());

                //Verwijder het Voertuig en controleer nog eens de velden om te zien of ze weg geschreven zijn 
                voertuigRepo.VerwijderVoertuig(v);

                if (!voertuigRepo.BestaatChassisnummer("ZZZZZZZZZZ9ZZ9999") || !voertuigRepo.BestaatNummerplaat("2ZZZ999"))
                {
                    Console.WriteLine("Voertuig succesvol verwijderd");
                }
                else
                {
                    Console.WriteLine("Oep... voertuig mey chassisnummer ZZZZZZZZZZ9ZZ9999 en/of numerplaat 2ZZZ999 niet verwijderd");
                }               
            }
            else
            {
                Console.WriteLine("Zorg er eerst voor dat je minstens 2 resutaten hebt om automodel & brandstof te selecteren");
            }

            Console.WriteLine("");
            voertuigRepo = null;
            autoModelrepo = null;
            brandstofRepo = null;
        }
    }
}
