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
        private static readonly string _connectionstring = "Data Source=.\\SQLEXPRESS;Initial Catalog=fleetManagement;Integrated Security=True; MultipleActiveResultSets=True";

        static void Main(string[] args)
        {
            Console.WriteLine("Test Bestuurder Repo: ");
            BestuurderRepoTest();

            /* 
             * Na elke debug dient t1 & t2 gewijzigd te worden omdat een tankkaart niet mag verwijderd worden 
             * Of was handmatig eerst brandstoffen in koppeltabel en dan tankaart
             */
            Console.WriteLine("Test Tankkaart Repo: ");
            string t1 = "99999999999999999828";
            string t2 = "11111111111111111676";
            TankkaartRepoTest(t1, t2);

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
                bestuurderUpdaten.Adres.VoegIdToe(b.Adres.AdresId);

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

                //Verwijder Adres
                repo.VerwijderBestuurderAdres(b);
                Console.WriteLine("Adres succesvol verwijderd");

                //Zoek op verwijderd object in DB
                bool metEersteRijksnr = repo.BestaatRijksRegisterNummer(rijksregisterNr);
                bool metTweedeRijksnr = repo.BestaatRijksRegisterNummer("05051299971");

                if (!metTweedeRijksnr && !metEersteRijksnr)
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

            Console.WriteLine("----------------------------------------------------------------------");
        }

        public static void TankkaartRepoTest(string tankkaartnummerStart, string tankkaartnummerUpdate)
        {            
            //Maak repo Tankkaart
            TankkaartRepositoryADO repo = new(_connectionstring);
            BrandstofRepositoryADO brandstofRepo = new(_connectionstring);

            List<BrandstofType> brandstoffen = (List<BrandstofType>)

            brandstofRepo.GeeAlleBrandstoffen();

            if (!repo.BestaatTankkaart(tankkaartnummerStart) && !repo.BestaatTankkaart(tankkaartnummerUpdate))
            {
                if (brandstoffen.Count > 2)
                {
                    DateTime geldigheidsDatum = DateTime.Now.AddDays(365);
                    TankKaart nieuwTankkaart = new(tankkaartnummerStart, geldigheidsDatum);  //kan tss 16 tot en met 20 digits zijn
                    nieuwTankkaart.VoegBrandstofToe(brandstoffen[0]);
                    nieuwTankkaart.VoegBrandstofToe(brandstoffen[1]);

                    //Tankkaart posten
                    repo.VoegTankKaartToe(nieuwTankkaart);
                    Console.WriteLine("Tankkaart succesvol ingevoegd");

                    //tankkaart ophalen in DB en weergeven
                    TankKaart t = repo.ZoekTankKaart(tankkaartnummerStart);

                    StringBuilder str = new();
                    str.AppendLine(t.TankKaartNummer);
                    str.AppendLine(t.GeldigheidsDatum.ToLongDateString());
                    str.AppendLine(t.Actief ? "Is actief" : "Is niet actief");

                    List<BrandstofType> brandstofVanTankkaart = (List<BrandstofType>)repo.BrandstoffenVoorTankaart(t);

                    brandstofVanTankkaart.ForEach(brandstof => {
                        str.AppendLine(brandstof.BrandstofNaam);
                    });

                    Console.WriteLine(str.ToString());

                    //Tankkaart en brandstof updaten 
                    geldigheidsDatum = t.GeldigheidsDatum.AddDays(100);
                    TankKaart UpdateTankkaart = new(t.TankKaartNummer, false, geldigheidsDatum, "9999");
                    UpdateTankkaart.VoegBrandstofToe(brandstoffen[2]);

                    //Verwijder eerst de brandstof met de oude kaart dat in DB zit want update zal de tankkaart automatisch via transactie invoegen
                    repo.VerwijderBrandstoffen(t);

                    //Controleer dat brandstoffen zijn verwijderd
                    if (repo.BrandstoffenVoorTankaart(t).Count < 1)
                    {
                        Console.WriteLine("Brandstoffen van tankkaart succesvol verwijderd");

                        //update tankkaart 
                        TankKaart tDB = repo.UpdateTankKaart(UpdateTankkaart, tankkaartnummerUpdate);

                        Console.WriteLine("Tankkaart succesvol geüpdatet");

                        //tankkaart ophalen in DB en weergeven
                        TankKaart zoekInDB = repo.ZoekTankKaart(tankkaartnummerUpdate);

                        str = new();
                        str.AppendLine(tDB.TankKaartNummer); //bewijs dat ID nummer ook terugkeert als dat wijzigt
                        str.AppendLine(zoekInDB.GeldigheidsDatum.ToLongDateString());
                        str.AppendLine(zoekInDB.Actief ? "Is actief" : "Is niet actief");
                        str.AppendLine(zoekInDB.Pincode);

                        brandstofVanTankkaart = (List<BrandstofType>)repo.BrandstoffenVoorTankaart(zoekInDB);

                        brandstofVanTankkaart.ForEach(brandstof => {
                            str.AppendLine(brandstof.BrandstofNaam);
                            zoekInDB.VoegBrandstofToe(brandstof);
                        });

                        Console.WriteLine(str.ToString());

                        //Verwijder tankkaart is gevraagd om niet te kunnen verwijderen
                        Console.WriteLine("Tankkaart kan niet verwijderd worden want moet steeds in DB blijven staan");

                        //verwijder brandstoffen
                        repo.VerwijderBrandstoffen(zoekInDB);
                    }
                    else
                    {
                        Console.WriteLine("Oeps... tankkaart heeft nog brandstof en is niet verwijderd");
                    }
                }
                else
                {
                    Console.WriteLine("Er moeten minstens 3 Brandstoffen in de database lijst zitten");
                }
            }
            else
            {
                Console.WriteLine("Wijzig eerst de twee tankkaartnummers want deze zijn al ingevoegd");
            }

            Console.WriteLine("----------------------------------------------------------------------");
            repo = null;
        }

        public static void VoertuigRepoTest()
        {
            //Maak repo Voertuig
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
                str.AppendLine(v.Brandstof.Hybride ? "Ja Hybride" : "Neen geen Hybride");
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

                str.AppendLine(v.Brandstof.Hybride ? "Ja Hybride" : "Neen geen Hybride");
                Console.WriteLine(str.ToString());

                //Verwijder het Voertuig en controleer nog eens de velden om te zien of ze weg geschreven zijn 
                voertuigRepo.VerwijderVoertuig(v);

                if (!voertuigRepo.BestaatChassisnummer("ZZZZZZZZZZ9ZZ9999") && !voertuigRepo.BestaatNummerplaat("2ZZZ999"))
                {
                    Console.WriteLine("Voertuig succesvol verwijderd");
                }
                else
                {
                    Console.WriteLine("Oep... voertuig met chassisnummer ZZZZZZZZZZ9ZZ9999 en/of numerplaat 2ZZZ999 niet verwijderd");
                }               
            }
            else
            {
                Console.WriteLine("Zorg er eerst voor dat je minstens 2 resutaten hebt om automodel & brandstof te selecteren");
            }

            Console.WriteLine("----------------------------------------------------------------------");
        }
    }
}
