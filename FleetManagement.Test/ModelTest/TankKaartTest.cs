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
        public void Verplichte_Velden_Valid()
        {
            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);

            TankKaart tankKaart = new("1234567890123456789", geldigheidsDatum);
            
            Assert.Null(tankKaart.Pincode);
            Assert.False(tankKaart.HeeftTankKaartBestuurder);
            Assert.Null(tankKaart.Bestuurder);
            Assert.Empty(tankKaart.Brandstoffen);
            
            Assert.True(tankKaart.Actief);

            Assert.False(tankKaart.IsGeldigheidsDatumVervallen);
            Assert.Equal(geldigheidsDatum, tankKaart.GeldigheidsDatum);
        }

        [Fact]
        public void Met_Pincode_Geldig()
        { 
            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);

            TankKaart tankKaart = new("1234567890123456789", true, geldigheidsDatum, "5310");
            Assert.Equal("1234567890123456789", tankKaart.TankKaartNummer);
            Assert.Equal("5310", tankKaart.Pincode);
            Assert.True(tankKaart.Actief);
            Assert.Equal(geldigheidsDatum, tankKaart.GeldigheidsDatum);
        }
        [Fact]
        public void True_MetVervaldatum_Ongeldig()
        {
            DateTime geldigheidsDatum = DateTime.Now.AddDays(-365);

            TankKaart tankKaart = new("1234567890123456789", true, geldigheidsDatum, "52374");
            Assert.Equal("1234567890123456789", tankKaart.TankKaartNummer);
            Assert.Equal("52374", tankKaart.Pincode);

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

            var e = Assert.Throws<TankKaartException>(() => {
               new TankKaart("", false, geldigheidsDatum, "52374"); 
            });

            Assert.Equal($"TankKaartNummer Kan niet null of leeg zijn", e.Message);
        }

        [Fact]
        public void BankKaart_Null_Leeg_Ongeldig()
        {
            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);
            var e = Assert.Throws<TankKaartException>(() => {
                new TankKaart(null, false, geldigheidsDatum, "52374");
            });

            Assert.Equal($"TankKaartNummer Kan niet null of leeg zijn", e.Message);
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
        }

        [Fact] 
        public void GeldigheidsDatum_IsNietVervallen() {
            DateTime vervalDatum = DateTime.Now.AddDays(512);
            TankKaart t = new("1234567890123456789", vervalDatum);
            Assert.True(t.Actief);
            Assert.False(t.IsGeldigheidsDatumVervallen);

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

        //veranderen:
        [Fact]
        public void Pincode_Ongeldig()
        {
            //GeldegheidsDatum in de toekomst
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);

            TankKaart t = new("1234567890123456789", GeldigheidsDatum);

            var e = Assert.Throws<TankKaartException>(() => {
                t.VoegPincodeToe(null);
            });

            Assert.Equal("Ingegeven Pincode mag niet null zijn", e.Message);
            
        }

        [Fact]
        public void UpdatePincode_Null_ongeldig()
        {
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);
            TankKaart t = new("1234567890123456789", GeldigheidsDatum);

            t.VoegPincodeToe("1111");
            var e = Assert.Throws<TankKaartException>(() => {
                t.UpdatePincode(null);
            });

            Assert.Equal("Ingegeven Pincode mag niet null zijn", e.Message);

        }
        

        [Fact]
        public void VoegPincodeToe_Leeg_Null_Ongeldig()
        {
            //GeldegheidsDatum in de toekomst
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);

            //Pincode niet meegegeven
            TankKaart t = new("1234567890123456789", GeldigheidsDatum);

            var e = Assert.Throws<TankKaartException>(() => {
                t.VoegPincodeToe(null);
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

            
            t.VoegBrandstofToe(bs);
            Assert.True(t.IsBrandstofAanwezig(bs));

            
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
        public void IsBrandstofAanwezig_Invalid() {
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

            DateTime vandaag = DateTime.Today;
            TankKaart tanKaart1 = new("1234567890123456789", vandaag);
            Assert.False(tanKaart1.IsGeldigheidsDatumVervallen);

            DateTime vandaag1SecondeTerug = DateTime.Today.AddSeconds(-1);
            TankKaart tanKaart2 = new("1234567890123456789", vandaag1SecondeTerug);
            Assert.True(tanKaart2.IsGeldigheidsDatumVervallen);
        }

        [Fact]
        public void VoegTankKaart_Valid()
        {
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);
            bool actief = true;
            TankKaart tankKaart = new TankKaart("1234567890123456789",actief,GeldigheidsDatum);
            Assert.Equal("1234567890123456789",tankKaart.TankKaartNummer);
            Assert.True(actief);
            Assert.Equal(GeldigheidsDatum, tankKaart.GeldigheidsDatum);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("")]
        public void VoegTankKaart_KaartNummer_Invalid(string kaartNummer)
        {
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);
            bool actief = true;
            var ex = Assert.Throws<TankKaartException>(() => new TankKaart(kaartNummer, actief, GeldigheidsDatum));
            Assert.Equal($"TankKaartNummer Kan niet null of leeg zijn", ex.Message);
        }
        [Fact]
        public void verwijderTankKaart_Van_Bestuurder_Valid()
        {
           
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);
            bool actief = true;
            TankKaart tankKaart = new TankKaart("1234567890123456789", actief, GeldigheidsDatum);
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "A,B", "76033101986");

            tankKaart.VoegBestuurderToe(bestuurder);
            bestuurder.VerwijderTankKaart(tankKaart);

            Assert.False(tankKaart.HeeftTankKaartBestuurder);
            Assert.False(bestuurder.HeeftBestuurderTankKaart);
        }
        [Fact]
        public void verwijderTankKaart_Van_Bestuurder_InValid()
        {
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);
            bool actief = true;
            TankKaart tankKaart = new TankKaart("1234567890123456789", actief, GeldigheidsDatum);
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "A,B", "76033101986");
            var ex = Assert.Throws<TankKaartException>(() => tankKaart.VoegBestuurderToe(null));
            
            Assert.Equal($"{nameof(Bestuurder)} mag niet null zijn", ex.Message);
        }
        



        [Fact]
        public void VerwijderBestuurder_Invalid()
        {
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "A,B", "76033101986");
            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);

            var ex =    Assert.Throws<TankKaartException>(() => new TankKaart("1234567890123456789", geldigheidsDatum)
            .VerwijderBestuurder(null));
            Assert.Equal($"Ingegeven {nameof(Bestuurder)} mag niet null zijn", ex.Message);

        }

        
    }
}
