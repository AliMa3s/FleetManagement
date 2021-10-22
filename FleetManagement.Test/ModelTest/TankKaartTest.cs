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
        public void NieuwInstantieZonderPincode_Actief()
        {
            //Maak een vervaldatum aan in de toekomst van 365 dagen
            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);

            //Maak TankKaart aan met drie argumenten
            TankKaart tankKaart = new("1234567890123456789", geldigheidsDatum, "");
            
            /* Controleer dat nieuwe instantie: 
             * 1). een lege pincode mag zijn
             * 2). Geen Bestuurder nodig heeft
             * 3). De brandstoflijst leeg mag zijn */
            Assert.Equal("", tankKaart.Pincode);
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
        public void NieuwInstantieMetPincode_Actief()
        { 
            //Maak een vervaldatum aan in de toekomst van 365 dagen
            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);

            //Maak TankKaart met meegegeven pincode & status
            TankKaart tankKaart = new("1234567890123456789", true, geldigheidsDatum, "5310");
            Assert.Equal("1234567890123456789", tankKaart.TankKaartNummer);
            Assert.Equal("5310", tankKaart.Pincode);
            Assert.True(tankKaart.Actief);
            Assert.Equal(geldigheidsDatum, tankKaart.GeldigheidsDatum);
        }
        [Fact]
        public void NieuwInstantieOnTrue_InActief()
        {
            //Maak een vervaldatum aan in het verleden van 365 dagen
            DateTime geldigheidsDatum = DateTime.Now.AddDays(-365);

            //Maak tankKaart aan die op true staat, maar geef datum die vervallen is
            TankKaart tankKaart = new("1234567890123456789", true, geldigheidsDatum, "52374");
            Assert.Equal("1234567890123456789", tankKaart.TankKaartNummer);
            Assert.Equal("52374", tankKaart.Pincode);

            //Constructor is slim genoeg om te herkennen dat TankKaart vervallen is ondanks true werd meegegeven
            Assert.False(tankKaart.Actief);
            Assert.True(tankKaart.IsGeldigheidsDatumVervallen);
            Assert.Equal(geldigheidsDatum, tankKaart.GeldigheidsDatum);
        }

        [Fact]
        public void NieuwInstantieTrueNietMeegegeven_InActief()
        {
            //Maak een vervaldatum aan in het verleden van 365 dagen
            DateTime geldigheidsDatum = DateTime.Now.AddDays(-365);

            //Maak tankKaart aan zonder True voor Status mee te geven, en geef datum die vervallen is
            TankKaart tankKaart = new("1234567890123456789", geldigheidsDatum, "52374");
            Assert.Equal("1234567890123456789", tankKaart.TankKaartNummer);
            Assert.Equal("52374", tankKaart.Pincode);

            //Constructor is slim genoeg om te herkennen dat TankKaart vervallen is, ondanks geen true of false is meegegeven
            Assert.False(tankKaart.Actief);
            Assert.True(tankKaart.IsGeldigheidsDatumVervallen); 
            Assert.Equal(geldigheidsDatum, tankKaart.GeldigheidsDatum);
        }

        [Fact]
        public void NieuwInstantieTFalseMeegegeven_Blijft_InActief()
        {
            //Maak een vervaldatum aan in de toemkomst van 365 dagen
            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);

            //Maak tankKaart aan en geef false mee (geblokkeerd) met een datum dat nog geldig is
            TankKaart tankKaart = new("1234567890123456789", false, geldigheidsDatum, "52374");
            Assert.Equal("1234567890123456789", tankKaart.TankKaartNummer);
            Assert.Equal("52374", tankKaart.Pincode);

            //Constructor is slim genoeg om de Kaart niet naar true te zetten want tankkaart is geblokkeerd
            Assert.False(tankKaart.Actief);
            Assert.False(tankKaart.IsGeldigheidsDatumVervallen); //tankKaart staat op Inactief maar Datum is niet vervallen
            Assert.Equal(geldigheidsDatum, tankKaart.GeldigheidsDatum);
        }

        //Check dag van GeldigheidsDatum => mag op de dag zelf niet vervallen zijn



        //Je kan een lijst met dubbels ingeven als we in de constructor dat toelaten
        //[Fact]
        //public void NewTankkaart() {
        //    List<BrandstofType> l1 = new List<BrandstofType>();
        //    l1.Add(new BrandstofType("gas"));
        //    TankKaart t = new TankKaart("1234567890123456789", new DateTime(2000, 01, 02), "1234", l1);
        //    Assert.False(t.Actief);
        //    Assert.Equal("1234567890123456789", t.KaartNummer);
        //    Assert.Equal(new DateTime(2000, 01, 02), t.GeldigheidsDatum);
        //    Assert.Equal("1234", t.Pincode);
        //    Assert.Equal(l1, t.Brandstoffen);
        //}

        //Ingevoegd Filip: Test nieuwe instantie TankKaart met Actief (true)
        [Fact]

        public void InstantieTankKaartActief() {
            //Maak een vervaldatum aan in de toekomst van 365 dagen
            DateTime vervalDatum = DateTime.Now.AddDays(365);

            //Test Instantie, verplichte velden, datum in de toekomst
            TankKaart tankKaart = new TankKaart("1234567890123456789", vervalDatum, "");
            Assert.Equal("1234567890123456789", tankKaart.TankKaartNummer);
            Assert.True(tankKaart.Actief);
            Assert.Empty(tankKaart.Pincode);
            Assert.Equal(vervalDatum, tankKaart.GeldigheidsDatum);
            Assert.Null(tankKaart.Bestuurder);
        }

        //Ingevoegd Filip: Test nieuwe instantie TankKaart met Inactief (false)
        [Fact]
        public void InstantieTankKaartNietActief() {
            //Maak een vervaldatum aan in het verleden van -365 dagen
            DateTime vervalDatum = DateTime.Now.AddDays(-365);

            //Test Instantie, verplichte velden, datum in het verleden
            TankKaart tankKaart = new TankKaart("1234567890123456789", vervalDatum);
            Assert.Equal("1234567890123456789", tankKaart.TankKaartNummer);
            Assert.False(tankKaart.Actief);
            Assert.Empty(tankKaart.Pincode);
            Assert.Equal(vervalDatum, tankKaart.GeldigheidsDatum);
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
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2000, 01, 02));
            Assert.True(t.IsGeldigheidsDatumVervallen);
        }

        [Fact]
        public void IsTankKaartVervallen_InValid() {
            DateTime vervalDatum = DateTime.Now.AddDays(512);
            TankKaart t = new TankKaart("1234567890123456789", vervalDatum);
            Assert.False(t.IsGeldigheidsDatumVervallen);
        }

        //Test wordt afgekeurd omdat de vervaldatum alang is gepasseerd (jaar 2000) 
        //Pincode toevoegen bij vervallen of geblokkeerde TankKaart is niet mogelijk
        [Fact]
        public void VoegPincodeToe_VervallenDatum_InValid() {
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2000, 01, 02));

           var e = Assert.Throws<TankKaartException>(() => { 
                t.VoegPincodeToe("1234");
            });

            Assert.Equal("Kan Pincode niet toevoegen want de TankKaart is niet (meer) actief", e.Message);
        }

        [Fact]
        public void UpdatePincode_VervallenDatum_InValid()
        {
            //Bestaande Pincode moet in de constructor worden meegegeven,
            //Als TankKaart is vervallen gaat dat niet na de instantie (zie test VoegPincodeToe_VervallenDatum_InValid)
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2000, 01, 02), "1234");

            var e = Assert.Throws<TankKaartException>(() => {
                t.UpdatePincode("4567");
            });

            Assert.Equal("Kan Pincode niet updaten want de TankKaart is niet (meer) actief", e.Message);
        }

        [Fact]
        public void UpdatePincode_leeg_InValid()
        {
            //GeldegheidsDatum in de toekomst
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);

            //Pincode dat leeg is hoeft niet meegestuurd te worden als argument; krijgt default lege string
            TankKaart t = new TankKaart("1234567890123456789", GeldigheidsDatum);

            t.VoegPincodeToe("4567");
            Assert.Equal("4567", t.Pincode);

            var e = Assert.Throws<TankKaartException>(() => {
                t.UpdatePincode("");
            });

            Assert.Equal("Ingegeven Pincode mag niet null zijn", e.Message);
        }

        [Fact]
        public void VoegPincodeToe_leeg_InValid()
        {
            //GeldegheidsDatum in de toekomst
            DateTime GeldigheidsDatum = DateTime.Now.AddDays(512);

            //Pincode niet meegegeven
            TankKaart t = new TankKaart("1234567890123456789", GeldigheidsDatum);

            var e = Assert.Throws<TankKaartException>(() => {
                t.VoegPincodeToe("");
            });

            Assert.Equal("Ingegeven Pincode mag niet null zijn", e.Message);
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
            tankKaart.VoegBrandstofToe(brandstofType);

            //Controleer dat brandstof aanwezig is
            Assert.True(tankKaart.IsBrandstofAanwezig(brandstofType));
        }

        [Fact]
        public void VoegBrandsotfType() {
            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2025, 01, 02));
            t.VoegBrandstofToe(bs);
            Assert.Equal("Gas", bs.BrandstofNaam);
        }

        [Fact]
        public void VerwijderBrandsotfType_Valid() {
            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2025, 01, 02));
            t.VoegBrandstofToe(bs);
            t.VerwijderBrandstof(bs);
        }

        [Fact]
        public void VerwijderBrandsotfType_Invalid() {
            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2025, 01, 02));
            var ex = Assert.Throws<TankKaartException>(() => t.VerwijderBrandstof(bs));
            Assert.Equal("Brandstof bestaat niet", ex.Message);
        }
        [Fact]
        public void IsBrandstofAanwezig() {
            BrandstofType bs = new BrandstofType("Gas");
            TankKaart t = new TankKaart("1234567890123456789", new DateTime(2025, 01, 02));
            t.VoegBrandstofToe(bs);
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
