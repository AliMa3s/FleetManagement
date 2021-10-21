using FleetManagement.Exceptions;
using FleetManagement.Model;
using FleetManagement.Test.Interfaces;
using FleetManagement.Test.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test.ModelTest {
    public class TankKaartTest {

        //Cadeau van Bestuurder voor TankKaart:
        private readonly IBestuurderNepRepo _bestuurderRepo = new BestuurderNepManager();
        private readonly IVoertuigNepRepo _voertuigRepo = new VoertuigNepManager();

        [Fact]
        public void VoorbeeldVoorAli() //toont gebruik van NepManager, 
        {
            Assert.True(_bestuurderRepo.IsBestuurderAanwezig("76033101986"), "Bestuurder moet aanwezig zijn");

            //Vraag eerst correcte instantie van Bestuurder aan in Repo:
            Bestuurder bestuurderZonderIetsTeDoen = _bestuurderRepo.GeefBestuurder("76033101986");

            //Maak een vervaldatum aan in de toekomst van 365 dagen (vervaldatum zal dus nooit vervallen en test zal altijd slagen)
            DateTime vervalDatum = DateTime.Now.AddDays(365);

            //Maak TankKaart aan, pincode moet kunnen leeg zijn
            TankKaart tankKaart = new("1234567890123456789", vervalDatum, "");

            Assert.True(tankKaart.Pincode == string.Empty, "Pincode moet leeg kunnen zijn bij nieuwe instantie");
            Assert.False(tankKaart.HeeftTankKaartBestuurder, "Bestuurder moet leeg kunnen zijn bij nieuwe instantie");

            //Voeg nu bestuurder toe aan TankKaart
            tankKaart.VoegBestuurderToe(bestuurderZonderIetsTeDoen);

            Assert.True(tankKaart.HeeftTankKaartBestuurder);

            //Probeer nog eens bestuurder toe te voegen alvorens eerst te controleren:

            Assert.Throws<TankKaartException>(() => {
                tankKaart.VoegBestuurderToe(bestuurderZonderIetsTeDoen);
                //relatie is één op één, je moet eerst huidige bestuurder verwijderen
            });

            //Vergeet niet dat jouw implementatie verkeerd was voor Bestuurder toe te voegen
            //Ik heb deze gecorrigeerd anders faalt deze test hier! 
        }

        [Fact]
        public void NewTankkaart() {
            List<BrandstofType> l1 = new List<BrandstofType>();
            l1.Add(new BrandstofType("gas"));
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2000, 01, 02), "1234", l1);
            Assert.False(t.Actief);
            Assert.Equal("1234567890123456789", t.KaartNummer);
            Assert.Equal(new DateTime(2000, 01, 02), t.VervalDatum);
            Assert.Equal("1234", t.Pincode);
            Assert.Equal(l1, t.BrandstofType);
        }

        //Ingevoegd Filip: Test nieuwe instantie TankKaart met Actief (true)
        [Fact]
        public void InstantieTankKaartActief() {
            //Maak een vervaldatum aan in de toekomst van 365 dagen
            DateTime vervalDatum = DateTime.Now.AddDays(365);

            //Test Instantie, verplichte velden, datum in de toekomst
            TankKaart tankKaart = new TankKaart("1234567890123456789", vervalDatum, "");
            Assert.Equal("1234567890123456789", tankKaart.KaartNummer);
            Assert.True(tankKaart.Actief);
            Assert.Empty(tankKaart.Pincode);
            Assert.Equal(vervalDatum, tankKaart.VervalDatum);
            Assert.Null(tankKaart.Bestuurder);
        }

        //Ingevoegd Filip: Test nieuwe instantie TankKaart met Inactief (false)
        [Fact]
        public void InstantieTankKaartNietActief() {
            //Maak een vervaldatum aan in het verleden van -365 dagen
            DateTime vervalDatum = DateTime.Now.AddDays(-365);

            //Test Instantie, verplichte velden, datum in het verleden
            TankKaart tankKaart = new TankKaart("1234567890123456789", vervalDatum);
            Assert.Equal("1234567890123456789", tankKaart.KaartNummer);
            Assert.False(tankKaart.Actief);
            Assert.Empty(tankKaart.Pincode);
            Assert.Equal(vervalDatum, tankKaart.VervalDatum);
            Assert.Null(tankKaart.Bestuurder);
        }

        //KaartNummer wijzigen is niet OK. Het mag nooit kunnen
        //En het zal ook niet kunnen! Onze database moet een constraint hebben hiervoor (diamant op ERD)

        //[Fact]
        //public void VoegTankKaart_Valid() {
        //    DateTime vervalDatum = DateTime.Now.AddDays(365);
        //    Bestuurder b = _bestuurderRepo.GeefBestuurder("76033101986");

        //    b.TankKaart.VoegKaartNummerToe("1234567890123456789");
        //    Assert.Equal("1234567890123456789", b.TankKaart.KaartNummer);
        //}

        ////Opgelet: lege TankKaartNummer geeft ook een exception!
        //[Fact]
        //public void VoegTankKaart_Invalid() {
        //    TankKaart t = new TankKaart("", new DateTime(2000, 01, 02));
        //    var ex = Assert.Throws<TankKaartException>(() => t.VoegKaartNummerToe(""));
        //    Assert.Equal("Kaart nummer kan niet leeg zijn", ex.Message);
        //}

        //BlokkeertTankKaart heeft verkeerde implementatie: kaart was NIET geblokkeerd! Dus geeft compilefout

        //[Fact]
        //public void BlokeerTankKaart_Valid() {
        //    DateTime vervalDatum = DateTime.Now.AddDays(365);
        //    TankKaart t = new TankKaart("1234567890123456789", vervalDatum, "1234");
        //    Assert.True(t.BlokkeerTankKaart("1234567890123456789"));
        //    Assert.True(t.Actief);
        //}

        //BlokkeertTankKaart heeft verkeerde implementatie: kaart was NIET geblokkeerd! Dus geeft compilefout

        //[Fact]
        //public void BlokeerTankKaart_Invalid() {
        //    DateTime vervalDatum = DateTime.Now.AddDays(-365);
        //    TankKaart t = new TankKaart("1234567890123456789", vervalDatum);

        //    Assert.True(t.BlokkeerTankKaart("1234567890123456789"));
        //    Assert.False(t.Actief);
        //}

        [Fact]
        public void IsTankKaartVervallen_Valid() {
            DateTime vervalDatum = DateTime.Now.AddDays(-365);
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2000, 01, 02));
            Assert.True(t.IsTankKaartVervallen());
        }

        [Fact]
        public void IsTankKaartVervallen_InValid() {
            DateTime vervalDatum = DateTime.Now.AddDays(-365);
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2025, 01, 02));
            Assert.False(t.IsTankKaartVervallen());
        }

        [Fact]
        public void VoegPincodeToe_Valid() {
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2000, 01, 02));
            t.VoegPincodeToe("1234");
            Assert.Equal("1234", t.Pincode);
        }

        [Fact]
        public void UpdatePintcode_InValid() {
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2025, 01, 02));
            t.VoegPincodeToe("1456");
            t.UpdatePincode("1234");
            Assert.Equal("1234", t.Pincode);
        }

        [Fact]
        public void VulTankKaartMetBrandstof() {
            //Selecteer bestuurder in de NepRepo
            Bestuurder bestuurder = _bestuurderRepo.GeefBestuurder("76033101986");
            Assert.NotNull(bestuurder);

            //Selecteer voor test een voertuig
            Voertuig voertuig = _voertuigRepo.GeefVoertuig("ABCDEFGHJKLMN1234");
            Assert.NotNull(voertuig);

            //Maak vervaldatum in de toekomst van 365 dagen
            DateTime vervalDatum = DateTime.Now.AddDays(365);

            //Maak een TankKaart aan
            TankKaart tankKaart = new("1234567890123456789", vervalDatum);

            //Controleer of Tankkaart een bestuurder heeft
            Assert.False(tankKaart.HeeftTankKaartBestuurder);

            //Voeg bestuurder toe aan tankkaart
            tankKaart.VoegBestuurderToe(bestuurder);

            //Controleer of bestuurder (via tankkaart) Voertuig heeft
            Assert.True(tankKaart.HeeftTankKaartBestuurder);

            //Controleer of bestuurder Voertuig heeft
            Assert.False(tankKaart.Bestuurder.HeeftBestuurderVoertuig);

            //Geef bestuurder een voertuig (via tankkaart)
            tankKaart.Bestuurder.VoegVoertuigToe(voertuig);

            //Controleer of bestuurder een voertuig heeft
            Assert.True(tankKaart.Bestuurder.HeeftBestuurderVoertuig);

            //Haal de brandstof van voertuig
            BrandstofType brandstofType = tankKaart.Bestuurder.Voertuig.Brandstof;

            //Controleer de brandstof in de lijst brandstoffen van Tankkaart
            Assert.False(tankKaart.IsBrandstofAanwezig(brandstofType));

            //Voeg brandstof toe aan de TankKaart
            tankKaart.VoegBrandstofTypeToe(brandstofType);

            //Controleer dat brandstof aanwezig is
            Assert.True(tankKaart.IsBrandstofAanwezig(brandstofType));
        }

        [Fact]
        public void VoegBrandsotfType() {
            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2025, 01, 02));
            t.VoegBrandstofTypeToe(bs);
            Assert.Equal("Gas", bs.BrandstofNaam);
        }

        [Fact]
        public void VerwijderBrandsotfType_Valid() {
            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2025, 01, 02));
            t.VoegBrandstofTypeToe(bs);
            t.VerwijderBrandstofType(bs);
        }

        [Fact]
        public void VerwijderBrandsotfType_Invalid() {
            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2025, 01, 02));
            var ex = Assert.Throws<TankKaartException>(() => t.VerwijderBrandstofType(bs));
            Assert.Equal("Brandstof bestaat niet", ex.Message);
        }
        [Fact]
        public void IsBrandstofAanwezig() {
            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2025, 01, 02));
            t.VoegBrandstofTypeToe(bs);
            t.IsBrandstofAanwezig(bs);
        }
        [Fact]
        public void IsBrandstofAanwezigInvalid() {
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2025, 01, 02));
            var ex = Assert.Throws<TankKaartException>(() => t.IsBrandstofAanwezig(null));
            Assert.Equal("Brandstof mag niet null zijn", ex.Message);
        }

    }
}
