using FleetManagement.Exceptions;
using FleetManagement.Model;
using FleetManagement.Test.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test.ModelTest {
    public class TankKaartTest {

        //Selecteer een Bestuurder en Voertuig uit de repo:
        private readonly BestuurderNepRepo _bestuurderRepo = new();
        private readonly VoertuigNepRepo _voertuigRepo = new();

        [Fact]
        public void Verplichte_Velden_Geldig()
        {
            //Maak een vervaldatum aan in de toekomst van 365 dagen
            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);

            //Maak TankKaart aan met drie argumenten
            TankKaart tankKaart = new("1234567890123456789", geldigheidsDatum);
            
            /* Controleer of nieuwe instantie: 
             * 1). En lege pincode mag zijn
             * 2). Geen Bestuurder nodig heeft
             * 3). De brandstofLijst leeg mag zijn */
            Assert.Null(tankKaart.Pincode);
            Assert.False(tankKaart.HeeftTankKaartBestuurder);
            Assert.Null(tankKaart.Bestuurder);
            Assert.Empty(tankKaart.Brandstoffen);

            //TankKaart staat op Actief
            Assert.True(tankKaart.Actief);

            //TankKaart is niet vervallen
            Assert.False(tankKaart.IsGeldigheidsDatumVervallen);
            Assert.Equal(geldigheidsDatum, tankKaart.GeldigheidsDatum);
        }

        [Fact]
        public void Met_Pincode_Geldig()
        { 
            //Maak een vervaldatum aan in de toekomst van 365 dagen
            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);

            //Maak TankKaart met meegeleverde pincode & status
            TankKaart tankKaart = new("1234567890123456789", true, geldigheidsDatum, "5310");
            Assert.Equal("1234567890123456789", tankKaart.TankKaartNummer);
            Assert.Equal("5310", tankKaart.Pincode);
            Assert.True(tankKaart.Actief);
            Assert.Equal(geldigheidsDatum, tankKaart.GeldigheidsDatum);
        }
        [Fact]
        public void True_MetVervaldatum_Ongeldig()
        {
            //Maak een vervaldatum aan in het verleden van 365 dagen
            DateTime geldigheidsDatum = DateTime.Now.AddDays(-365);

            //Maak tankKaart aan die op true staat,en geef datum op die vervallen is
            TankKaart tankKaart = new("1234567890123456789", true, geldigheidsDatum, "52374");
            Assert.Equal("1234567890123456789", tankKaart.TankKaartNummer);
            Assert.Equal("52374", tankKaart.Pincode);

            //Constructor is slim genoeg om te herkennen dat TankKaart vervallen is
            Assert.False(tankKaart.Actief);
            Assert.True(tankKaart.IsGeldigheidsDatumVervallen);
            Assert.Equal(geldigheidsDatum, tankKaart.GeldigheidsDatum);
        }

        [Fact]
        public void Ongeldig_Blijft_Ongeldig()
        {
            //Maak een vervaldatum aan in de toemkomst van 365 dagen
            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);

            //Maak tankKaart aan en geef false mee (geblokkeerd) maar met een niet vervallen datum
            TankKaart tankKaart = new("1234567890123456789", false, geldigheidsDatum, null);
            Assert.Equal("1234567890123456789", tankKaart.TankKaartNummer);
            Assert.Null(tankKaart.Pincode);

            //Constructor is slim genoeg om de Kaart niet naar true te zetten want tankkaart is geblokkeerd
            Assert.False(tankKaart.Actief);
            Assert.False(tankKaart.IsGeldigheidsDatumVervallen); //tankKaart staat op Inactief maar Datum is niet vervallen
            Assert.Equal(geldigheidsDatum, tankKaart.GeldigheidsDatum);
        }

        [Fact]
        public void TankKaartNummer_Null_leeg_Ongeldig()
        {
            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);

            //BankKaartNummer Leeg
            var e = Assert.Throws<TankKaartException>(() => {
               new TankKaart("", false, geldigheidsDatum, "52374"); 
            });

            Assert.Equal($"{nameof(TankKaart)} Kan niet null of leeg zijn", e.Message);

            //BankKaartNummer Null
            e = Assert.Throws<TankKaartException>(() => {
                new TankKaart(null, false, geldigheidsDatum, "52374");
            });

            Assert.Equal($"{nameof(TankKaart)} Kan niet null of leeg zijn", e.Message);
        }

        [Fact]
        public void VoegBestuurderToe_En_Verwijder()
        {
            //Selecteer een bestuurder uit de lijst
            Bestuurder bestuurder = _bestuurderRepo.GeefBestuurder("76033101986");

            //Maak een TankKaart aan
            var GeldigheidsDatum = DateTime.Now.AddDays(312);
            TankKaart tanKaart = new("5632637890123456789", GeldigheidsDatum);

            //Controleer dat TankKaart nog geen Bestuurder heeft
            Assert.False(tanKaart.HeeftTankKaartBestuurder);

            //Voeg de bestuurder toe
            tanKaart.VoegBestuurderToe(bestuurder);

            //controleer nu dat bestuurder aanwezig is
            Assert.True(tanKaart.HeeftTankKaartBestuurder);

            //Controleer de relatie: Bestuurder moet nu ook de TankKaart kennen
            Assert.True(tanKaart.Bestuurder.HeeftBestuurderTankKaart);

            //Controleer dat alle TankKaartNummers gelijk zijn
            Assert.Equal(tanKaart.TankKaartNummer, tanKaart.Bestuurder.TankKaart.TankKaartNummer);
            Assert.Equal("5632637890123456789", tanKaart.TankKaartNummer);
            Assert.Equal("5632637890123456789", tanKaart.Bestuurder.TankKaart.TankKaartNummer);

            //Controleer de GeldigheidsDatums
            Assert.Equal(tanKaart.GeldigheidsDatum, tanKaart.Bestuurder.TankKaart.GeldigheidsDatum);
            Assert.Equal(GeldigheidsDatum, tanKaart.GeldigheidsDatum);
            Assert.Equal(GeldigheidsDatum, tanKaart.Bestuurder.TankKaart.GeldigheidsDatum);

            //Voeg een andere Bestuurder toe via TankKaart(selecteer ander Bestuurder in repo)
            Bestuurder anderBestuurder = _bestuurderRepo.GeefBestuurder("76003101965");

            var ex = Assert.Throws<TankKaartException>(() =>
            {
                tanKaart.VoegBestuurderToe(anderBestuurder);
            });

            Assert.Equal($"Er is al een {nameof(Bestuurder)} aan de TankKaart toegevoegd", ex.Message);

            //Voeg nu een ander bestuurder toe via de relatie
            ex = Assert.Throws<TankKaartException>(() =>
            {
                tanKaart.Bestuurder.TankKaart.VoegBestuurderToe(anderBestuurder);
            });

            Assert.Equal($"Er is al een {nameof(Bestuurder)} aan de TankKaart toegevoegd", ex.Message);

            //Voeg een TankKaart toe via de relatie
            var ex2 = Assert.Throws<BestuurderException>(() =>
            {
                tanKaart.Bestuurder.VoegTankKaartToe(
                        new TankKaart("5632394280123456789", DateTime.Now.AddDays(512))
                    );
            });

            Assert.Equal($"{nameof(Bestuurder)} heeft al een {nameof(TankKaart)}", ex2.Message);

            //Controleer de eerste Bestuurder uit repo die we hebben toegevoegd
            //Via Rreference Type moet dat gekoppeld zijn aan de TankKaart
            Assert.True(bestuurder.HeeftBestuurderTankKaart);

            //Probeer eerst anderBestuurder mee te geven om te verwijderen      
            var ex3 = Assert.Throws<TankKaartException>(() =>
            {
                tanKaart.VerwijderBestuurder(anderBestuurder);
            });

            Assert.Equal($"{nameof(Bestuurder)} kan niet worden verwijderd", ex3.Message);

            //Probeer nog eens null mee te geven om te verwijderen  
            var ex4 = Assert.Throws<TankKaartException>(() =>
            {
                tanKaart.VerwijderBestuurder(null);
            });

            Assert.Equal($"Ingegeven {nameof(Bestuurder)} mag niet null zijn", ex4.Message);

            //Verwijder nu de juiste Bestuurder met nieuwe instantie
            Bestuurder zelfdeBestuurder = new(1, "Filip", "Rigoir", "1976-03-31", "B", "1514081390", "76033101986");
            tanKaart.VerwijderBestuurder(zelfdeBestuurder);

            //Controleer tankkaart & bestuurder, beide moeten losgekoppeld zijn
            Assert.False(tanKaart.HeeftTankKaartBestuurder);
            Assert.False(bestuurder.HeeftBestuurderTankKaart); //Reference Type is ook null
        }

        [Fact]
        public void BlokkeerTankKaart_Inactief()
        {
            DateTime vervalDatum = DateTime.Now.AddDays(365);
            TankKaart t = new("1234567890123456789", true, vervalDatum, "");

            //Kaart is Actief
            Assert.True(t.Actief);

            //Blokkeer TankKaart
            t.BlokkeerTankKaart();

            //Kaart is onmiddelijk Inactief
            Assert.False(t.Actief);
        }

        [Fact]
        public void BlokkeerTankKaart_ReedsGeblokkeerd()
        {
            //Datum in toekomst
            DateTime vervalDatum = DateTime.Now.AddDays(365);
            TankKaart t = new("1234567890123456789", false, vervalDatum, "1234");

            //TankKaart is al goblokkeerd
            var e = Assert.Throws<TankKaartException>(() => {
                t.BlokkeerTankKaart();
            });

            Assert.Equal($"{nameof(TankKaart)} is al geblokkeerd", e.Message);

            //TankKaart is nog niet vervallen maar wel geblokkeerd
            Assert.False(t.IsGeldigheidsDatumVervallen);
        }

        [Fact]
        public void BlokkeerTankKaart_ReedsVervallen()
        {
            //Datum verleden
            DateTime vervalDatum = DateTime.Now.AddDays(-365);
            TankKaart t = new("1234567890123456789", vervalDatum, "1234");

            var e = Assert.Throws<TankKaartException>(() => {
                t.BlokkeerTankKaart();
            });

            Assert.Equal($"{nameof(TankKaart)} is reeds vervallen", e.Message);

            //TankKaart is vervallen
            Assert.True(t.IsGeldigheidsDatumVervallen);
        }

        [Fact]
        public void GeldigheidsDatum_IsVervallen() {
            TankKaart t = new("1234567890123456789", new DateTime(2000, 01, 02));
            Assert.False(t.Actief);
            Assert.True(t.IsGeldigheidsDatumVervallen);

            //Todo: check Date.now + 1 dag (moet vervallen op: 00.00.00 am)
        }

        [Fact] 
        public void GeldigheidsDatum_IsNietVervallen() {
            DateTime vervalDatum = DateTime.Now.AddDays(512);
            TankKaart t = new("1234567890123456789", vervalDatum);
            Assert.True(t.Actief);
            Assert.False(t.IsGeldigheidsDatumVervallen);

            //Todo: check dag van invoeging (IsGeldigheidsDatumVervallen moet op dag zelf van aanmaken false zijn)
            //met GedligheidsDatum van vandaag ingeven dus

            //ook Todo: Check einde van GeldigheidsDatum => mag niet vervallen zijn
        }

        [Fact]
        public void VoegPincode_En_VerstrekenDatum() {
            TankKaart t = new("1234567890123456789", new DateTime(2000, 01, 02));

           var e = Assert.Throws<TankKaartException>(() => { 
                t.VoegPincodeToe("1234");
            });

            Assert.Equal("Kan Pincode niet toevoegen want de TankKaart is niet (meer) actief", e.Message);
        }

        [Fact]
        public void UpdatePincode_En_VerstrekenDatum()
        {
            TankKaart t = new("1234567890123456789", new DateTime(2000, 01, 02), "1234");

            var e = Assert.Throws<TankKaartException>(() => {
                t.UpdatePincode("4567");
            });

            Assert.Equal("Kan Pincode niet updaten want de TankKaart is niet (meer) actief", e.Message);
        }

        [Fact]
        public void UpdatePincode_Leeg_Geldig()
        {
            //GeldegheidsDatum in de toekomst
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);

            //Pincode dat leeg is hoeft niet meegestuurd te worden als argument; krijgt default lege string
            TankKaart t = new("1234567890123456789", GeldigheidsDatum);
        }

        [Fact]
        public void IngegevenPincodes_null_Ongeldig()
        {
            //GeldegheidsDatum in de toekomst
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);

            TankKaart t = new("1234567890123456789", GeldigheidsDatum);

            var e = Assert.Throws<TankKaartException>(() => {
                t.VoegPincodeToe(null);
            });

            Assert.Equal("Ingegeven Pincode mag niet null zijn", e.Message);

            e = Assert.Throws<TankKaartException>(() => {
                t.UpdatePincode(null);
            });

            Assert.Equal("Ingegeven Pincode mag niet null zijn", e.Message);
        }

        //wanneer constructor voor pin leeg is, gooi e want dat is plaatsen

        [Fact]
        public void VoegPincodeToe_Leeg_Null_Ongeldig()
        {
            //GeldegheidsDatum in de toekomst
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);

            //Pincode niet meegegeven
            TankKaart t = new("1234567890123456789", GeldigheidsDatum);

            var e = Assert.Throws<TankKaartException>(() => {
                t.VoegPincodeToe("");
            });

            Assert.Equal("Ingegeven Pincode mag niet null zijn", e.Message);
        }

        [Fact]
        public void VoegPincodeToe_Dan_UpdatePincode() {
            DateTime vervalDatum = DateTime.Now.AddDays(512);

            TankKaart t = new("1234567890123456789", vervalDatum);
            t.VoegPincodeToe("1456");
            Assert.Equal("1456", t.Pincode);
            t.UpdatePincode("1234");
            Assert.Equal("1234", t.Pincode);
            t.UpdatePincode("");
            Assert.Equal("", t.Pincode);
            t.UpdatePincode("78953");
            Assert.Equal("78953", t.Pincode);
        }

        [Fact]
        public void VulTankKaartMetBrandstof() {

            //Selecteer een voertuig uit de repo
            Voertuig voertuig = _voertuigRepo.GeefVoertuig("ABCDEFGHJKLMN1234");

            //Maak vervaldatum in de toekomst van 365 dagen
            DateTime vervalDatum = DateTime.Now.AddDays(365);

            //Maak een TankKaart aan
            TankKaart tankKaart = new("1234567890123456789", vervalDatum);

            //Controleer in "Tankkaart brandstoffen" de brandstof van het voertuig 
            Assert.False(tankKaart.IsBrandstofAanwezig(voertuig.Brandstof));
            Assert.Equal("Hybride met Benzine", voertuig.Brandstof.BrandstofNaam);

            //Vul de TankKaart met de brandstof van het voertuig
            tankKaart.VoegBrandstofToe(voertuig.Brandstof);

            //Controleer dat de brandstof is gevuld
            Assert.True(tankKaart.IsBrandstofAanwezig(voertuig.Brandstof));
            Assert.Equal("Hybride met Benzine", tankKaart.Brandstoffen[0].BrandstofNaam);
        }

        [Fact]
        public void VoegBrandstofTypeToe_Valid() {
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);
            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new("1234567890123456789", GeldigheidsDatum);

            Assert.False(t.IsBrandstofAanwezig(bs));

            t.VoegBrandstofToe(bs);
            Assert.Equal("Gas", bs.BrandstofNaam);
        }
        [Fact]
        public void VoegBrandstofTypeToe_Invalid()
        {
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);
            TankKaart t = new("1234567890123456789", GeldigheidsDatum);

            var ex = Assert.Throws<TankKaartException>(() => t.VoegBrandstofToe(null));
            Assert.Equal("Brandstof mag niet null zijn", ex.Message);
        }
        [Fact]
        public void VerwijderBrandsotfType_Valid() {
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);
            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new("1234567890123456789", GeldigheidsDatum);

            //Toevoegen
            t.VoegBrandstofToe(bs);
            Assert.True(t.IsBrandstofAanwezig(bs));

            //Verwijderd
            t.VerwijderBrandstof(bs);
            Assert.False(t.IsBrandstofAanwezig(bs));
        }
        
        [Fact]
        public void VerwijderBrandsotfType_Invalid() {
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);

            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new("1234567890123456789", GeldigheidsDatum);
            var ex = Assert.Throws<TankKaartException>(() => t.VerwijderBrandstof(bs));
            Assert.Equal("Brandstof bestaat niet", ex.Message);
        }
        [Fact]
        public void IsBrandstofAanwezig_valid() {
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);
            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new("1234567890123456789", GeldigheidsDatum);
            t.VoegBrandstofToe(bs);
            Assert.True(t.IsBrandstofAanwezig(bs));
        }
        [Fact]
        public void IsBrandstofAanwezigInvalid() {
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);
            TankKaart t = new("1234567890123456789", GeldigheidsDatum);
            var ex = Assert.Throws<TankKaartException>(() => t.IsBrandstofAanwezig(null));
            Assert.Equal("Brandstof mag niet null zijn", ex.Message);
        }

        [Fact]
        public void InstantieVergelijking()
        {
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);

            //Zelfde instantie Gegevens
            TankKaart tanKaart1 = new("1234567890123456789", GeldigheidsDatum);
            TankKaart tanKaart2 = new("1234567890123456789", GeldigheidsDatum);
            Assert.True(tanKaart1.Equals(tanKaart2));

            //Verkeerde GeldigheidsDatum
            var GeldigheidsDatum2 = DateTime.Now.AddDays(312);
            TankKaart tanKaart3 = new("1234567890123456789", GeldigheidsDatum2);
            Assert.False(tanKaart1.Equals(tanKaart3));

            //Verkeerde TankKaartNummer
            GeldigheidsDatum = DateTime.Now.AddDays(312);
            TankKaart tanKaart4 = new("5632637890123456789", GeldigheidsDatum);
            Assert.False(tanKaart1.Equals(tanKaart4));

            //Verkeerde GeldigheidsDatum & TankKaartNummer
            TankKaart tanKaart5 = new("5632789012345674489", GeldigheidsDatum2);
            Assert.False(tanKaart1.Equals(tanKaart5));
        }

        [Fact]
        public void UitersteGeldigheidsDatum()
        {
            //Om te vergelijken worden de uren van datums op 00:00:00 gezet

            //De geldigheidsDatum op vandaag moet steeds geldig zijn.
            DateTime vandaag = DateTime.Today;
            TankKaart tanKaart1 = new("1234567890123456789", vandaag);
            Assert.False(tanKaart1.IsGeldigheidsDatumVervallen);

            //De geldigheidsDatum op 1 seconde vroeger dan vandaag is vervallen.
            DateTime vandaag1SecondeTerug = DateTime.Today.AddSeconds(-1);
            TankKaart tanKaart2 = new("1234567890123456789", vandaag1SecondeTerug);
            Assert.True(tanKaart2.IsGeldigheidsDatumVervallen);
        }
    }
}
