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

        [Fact]
        public void VoorbeeldVoorAli()
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
            tankKaart.VoegBestuurderAanTankKaart(bestuurderZonderIetsTeDoen);

            Assert.True(tankKaart.HeeftTankKaartBestuurder);

            //Probeer nog eens bestuurder toe te voegen alvorens eerst te controleren:

            Assert.Throws<TankKaartException>(() => {
                tankKaart.VoegBestuurderAanTankKaart(bestuurderZonderIetsTeDoen);
                //relatie is één op één, je moet eerst huidige bestuurder verwijderen
            });

            //Vergeet niet dat jouw implementatie verkeerd was voor Bestuurder toe te voegen
            //Ik heb deze gecorrigeerd anders faalt deze test hier! 
        }

        [Fact]
        public void Test_NewTankkaart() {
            List<BrandstofType> l1 = new List<BrandstofType>();
            l1.Add(new BrandstofType("gas"));
            TankKaart t = new TankKaart("123", new DateTime(2000, 01, 02), "1234", l1);
            Assert.Equal("123", t.KaartNummer);
            Assert.Equal(new DateTime(2000, 01, 02), t.VervalDatum);
            Assert.Equal("1234", t.Pincode);
            Assert.Equal(l1, t.BrandstofType);
        }

        [Fact]
        public void Test_VoegTankKaart_Valid() {
            TankKaart t = new TankKaart("", new DateTime(2000, 01, 02));
            t.VoegKaartNummerToe("abc");
            Assert.Equal("abc", t.KaartNummer);
        }
        [Fact]
        public void Test_VoegTankKaart_Invalid() {
            TankKaart t = new TankKaart("", new DateTime(2000, 01, 02));
            var ex = Assert.Throws<TankKaartException>(() => t.VoegKaartNummerToe(""));
            Assert.Equal("Kaart nummer kan niet leeg zijn", ex.Message);
        }

        [Fact]
        public void Test_BlokeerTankKaart_Valid() {
            TankKaart t = new TankKaart("abc", new DateTime(2000, 01, 02));
            Assert.True(t.BlokkeerTankKaart("abc"));
        }

        [Fact]
        public void Test_BlokeerTankKaart_Invalid() {
            TankKaart t = new TankKaart("abc", new DateTime(2000, 01, 02));

            Assert.False(t.BlokkeerTankKaart(""));
        }

        [Fact]
        public void Test_IsTankKaartVervallen_Valid() {
            TankKaart t = new TankKaart("abc", new DateTime(2000, 01, 02));
            Assert.True(t.IsTankKaartVervallen());
        }

        [Fact]
        public void Test_IsTankKaartVervallen_InValid() {
            TankKaart t = new TankKaart("abc", new DateTime(2025, 01, 02));
            Assert.False(t.IsTankKaartVervallen());
        }

        [Fact]
        public void Test_VoegPincodeToe_Valid() {
            TankKaart t = new TankKaart("abc", new DateTime(2000, 01, 02));
            t.VoegPincodeToe("1234");
            Assert.Equal("1234", t.Pincode);
        }

        [Fact]
        public void Test_UpdatePintcode_InValid() {
            TankKaart t = new TankKaart("abc", new DateTime(2025, 01, 02));
            t.VoegPincodeToe("1456");
            t.UpdatePincode("1234");
            Assert.Equal("1234", t.Pincode);
        }

        //Meeting moet besproken worden over de list of string of brandstoftype
        //[Fact]
        //public void Test_VoegBrandStofTypeToe_Valid() {
        //    TankKaart t = new TankKaart("abc", new DateTime(2000, 01, 02));
        //    BrandstofType bs = new BrandstofType("Gas");
        //    List<BrandstofType> l1 = new List<BrandstofType>();
        //    l1.Add(bs);
        //    t.VoegBrandstofType(bs);
        //    Assert.Equal("Gas", bs.BrandstofNaam);
        //}


        //rijksregister moet nog gemaakt worden in bestuurder klas
        //[Fact]
        //public void Test_VoegBestuurder_Valid() {
        //    TankKaart t = new TankKaart("abc", new DateTime(2000, 01, 02));
        //    Bestuurder b = new Bestuurder("a", "b", new DateTime(2000, 01, 02), "b", "123", "12-12-21-123-21");
        //    t.VoegBestuurderAanTankKaart(b);
        //}

    }
}
