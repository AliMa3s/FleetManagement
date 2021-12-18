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
    public class BestuurderTest {

        //Selecteer een Bestuurder en Voertuig uit de repo:
        private readonly TankKaartNepRepo _tankKaartNepRepo = new();
        private readonly VoertuigNepRepo _voertuigNepRepo = new();

        private readonly BestuurderNepRepo _bestuurderNepRepo = new();

        [Fact]
        public void VerplichteVelden_Valid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");

            Assert.Equal("Filip", bestuurder.Voornaam);
            Assert.Equal("Rigoir", bestuurder.Achternaam);
            Assert.Equal("1976/03/31", bestuurder.GeboorteDatum);
            Assert.Equal("B,E+1", bestuurder.TypeRijbewijs);
            Assert.Equal("76033101986", bestuurder.RijksRegisterNummer);

            Assert.Equal(0, bestuurder.BestuurderId);
            Assert.False(bestuurder.HeeftBestuurderVoertuig);
            Assert.False(bestuurder.HeeftBestuurderTankKaart);
            Assert.Null(bestuurder.Adres);
        }
        
        [Fact]
        public void Id_Nummer_Valid()
        {
            Bestuurder bestuurder = new(1, "Filip", "Rigoir", "1976/03/31", "A,B", "76033101986");
            Assert.Equal(1, bestuurder.BestuurderId);
        }

        [Theory]
        [InlineData(-35)]
        [InlineData(0)]
        public void Id_Nummer_InValid(int id)
        {
            var e = Assert.Throws<BestuurderException>(() => {
                new Bestuurder(id, "Filip", "Rigoir", "1976/03/31", "A,B", "76033101986");
            });

            Assert.Equal("BestuurderId moet meer zijn dan 0", e.Message);

        }

        [Theory]
        [InlineData("ahmet")]
        [InlineData("ali")]
        [InlineData("filip")]
        public void Bestuurder_voornaam_valid(string voornaam)
        {
            Bestuurder bestuurder = new Bestuurder(voornaam, "yilmaz", "1976/03/31", "B,E+1", "76033101986");
            Assert.Equal(voornaam, bestuurder.Voornaam);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("\n")]
        [InlineData(" ")]
        public void Bestuurder_voornaam_Invalid(string voornaam)
        {
            var ex = Assert.Throws<BestuurderException>(() =>
            {
                new Bestuurder(voornaam, "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            });

            Assert.Equal("Voornaam moet ingevuld zijn", ex.Message);
        }

        [Theory]
        [InlineData("yilmaz")]
        [InlineData("maes")]
        [InlineData("rigoir")]
        public void Bestuurder_achternaam_valid(string achternaam)
        {
            Bestuurder bestuurder = new Bestuurder("ahmet", achternaam, "1976/03/31", "B,E+1", "76033101986");
            Assert.Equal(achternaam, bestuurder.Achternaam);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("\n")]
        [InlineData(" ")]
        public void Bestuurder_achternaam_Invalid(string achternaam)
        {
            var ex = Assert.Throws<BestuurderException>(() =>
            {
                new Bestuurder("Ahmet", achternaam, "1976/03/31", "B,E+1", "76033101986");
            });

            Assert.Equal("Achternaam moet ingevuld zijn", ex.Message);
        }

        [Fact]
        public void Ctor_NoId_Valid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            Assert.Equal(0, bestuurder.BestuurderId);
            bestuurder.VoegIdToe(1);
            Assert.Equal(1, bestuurder.BestuurderId);
        }

        [Fact]
        public void VoegId_HeeftID_Invalid()
        {
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            var ex = Assert.Throws<BestuurderException>(() => bestuurder.VoegIdToe(1));
            Assert.Equal("BestuurderId is al aanwezig en kan niet gewijzigd worden", ex.Message);

        }

        [Fact]
        public void BestuurderAdres_NotNull_Ingevuld_Valid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            bestuurder.Adres = new Adres("L.Schuermanstraat", "20", "9040", "Gent");
            Assert.NotNull(bestuurder.Adres);
        }

        [Fact]
        public void BestuurderAdres_NotNull_NietIngevuld_Valid() 
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            bestuurder.Adres = new Adres("", "", "", "");
            Assert.NotNull(bestuurder.Adres);
        }

        [Fact]
        public void InstantieVergelijking_Valid()
        {
            Bestuurder bestuurder1 = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            Bestuurder bestuurder2 = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            Assert.True(bestuurder1.Equals(bestuurder2));
        }

        [Fact]
        public void InstantieVergelijking_InValid()
        {
            Bestuurder bestuurder1 = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            Bestuurder bestuurder2 = new Bestuurder("Filip", "Rigoir", "2018-12-05", "B,E+1", "18120553401");
            Assert.False(bestuurder1.Equals(bestuurder2));
        }

        [Fact]
        public void GeefBestuurder_Valid()
        {
            Bestuurder bestuurder = _bestuurderNepRepo.GeefBestuurder("76033101986");
            Assert.Equal("76033101986", bestuurder.RijksRegisterNummer);
        }

        [Fact]
        public void VolledigeNaamProperty()
        {
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            Assert.Equal("Rigoir Filip", bestuurder.Naam);
        }

        /* 
         * relaties testen Bestuurder krijgt Voertuig
         */

        //Plaats entiteit en controleer met bestuurder
        [Fact]
        public void GeefVoertuig_Valid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            Voertuig voertuig = _voertuigNepRepo.GeefVoertuig("ABCDEFGHJKLMN1234");

            bestuurder.VoegVoertuigToe(voertuig);
            Assert.True(bestuurder.HeeftBestuurderVoertuig);
        }

        //Plaats entiteit en controleer met voertuig
        [Fact]
        public void GeefVoertuig_Relatie_Valid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            Voertuig voertuig = _voertuigNepRepo.GeefVoertuig("ABCDEFGHJKLMN1234");

            bestuurder.VoegVoertuigToe(voertuig);
            Assert.True(bestuurder.Voertuig.HeeftVoertuigBestuurder);
            Assert.True(voertuig.HeeftVoertuigBestuurder);  //controleer ook reference type
        }

        //Plaats entiteit en probeer nog een ander toe te voegen
        [Fact]
        public void GeefVoertuig_InValid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            Voertuig voertuig = _voertuigNepRepo.GeefVoertuig("ABCDEFGHJKLMN1234");
            bestuurder.VoegVoertuigToe(voertuig);

            //Probeer een ander voertuig toe te voegen
            Voertuig anderVoertuig = _voertuigNepRepo.GeefVoertuig("ABCDEFG1234HJKLMN");

            var ex = Assert.Throws<BestuurderException>(() => bestuurder.VoegVoertuigToe(voertuig));
            Assert.Equal("Bestuurder heeft al een Voertuig", ex.Message);
        }

        //Plaats null
        [Fact]
        public void GeefVoertuig_null_InValid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            Voertuig voertuig = _voertuigNepRepo.GeefVoertuig("ABCDEFGHJKLMN1234");

            var ex = Assert.Throws<BestuurderException>(() => bestuurder.VoegVoertuigToe(null));
            Assert.Equal("Ingegeven Voertuig mag niet null zijn", ex.Message);
        }

        //Plaats entiteit en verwijder dezelfde entiteit
        [Fact]
        public void VerwijderVoertuig_Valid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            Voertuig voertuig = _voertuigNepRepo.GeefVoertuig("ABCDEFGHJKLMN1234");
            bestuurder.VoegVoertuigToe(voertuig);
            Assert.True(bestuurder.HeeftBestuurderVoertuig);

            bestuurder.VerwijderVoertuig(bestuurder.Voertuig);

            Assert.False(bestuurder.HeeftBestuurderVoertuig);
            Assert.False(voertuig.HeeftVoertuigBestuurder);  //reference type is nu ook losgekoppeld
        }

        //Plaats entiteit en probeer een ander object te verwijder
        [Fact]
        public void VerwijderVoertuig_InValid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            Voertuig voertuig = _voertuigNepRepo.GeefVoertuig("ABCDEFGHJKLMN1234");
            bestuurder.VoegVoertuigToe(voertuig);

            //Probeer met ander voertuig te verwijderen
            Voertuig anderVoertuig = _voertuigNepRepo.GeefVoertuig("ABCDEFG1234HJKLMN");

            var ex = Assert.Throws<BestuurderException>(() => bestuurder.VerwijderVoertuig(anderVoertuig));
            Assert.Equal("Voertuig kan niet verwijderd worden", ex.Message);
        }

        //Plaats null
        [Fact]
        public void VerwijderVoertuig_null_InValid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");

            var ex = Assert.Throws<BestuurderException>(() => bestuurder.VerwijderVoertuig(null));
            Assert.Equal("Voertuig mag niet null zijn", ex.Message);
        }

        /* 
         * relaties testen Bestuurder krijgt Tankkaart
         */

        //Plaats entiteit en controleer met bestuurder
        [Fact]
        public void GeefTankKaart_Valid()
        {
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            TankKaart tankkaart = _tankKaartNepRepo.GeefTankKaart("1234567890123456789");

            bestuurder.VoegTankKaartToe(tankkaart);
            Assert.True(bestuurder.HeeftBestuurderTankKaart);
            Assert.True(tankkaart.HeeftTankKaartBestuurder);  //via reference type bevestiging
        }

        //Plaats entiteit en controleer met tankkaart
        [Fact]
        public void GeefTankKaart_Relatie_Valid()
        {
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            TankKaart tankkaart = _tankKaartNepRepo.GeefTankKaart("1234567890123456789");

            bestuurder.VoegTankKaartToe(tankkaart);
            Assert.True(bestuurder.Tankkaart.HeeftTankKaartBestuurder);
        }

        //Plaats entiteit en probeer nog een ander toe te voegen
        [Fact]
        public void GeefTankKaart_Relatie_InValid()
        {
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            TankKaart tankkaart = _tankKaartNepRepo.GeefTankKaart("1234567890123456789");

            bestuurder.VoegTankKaartToe(tankkaart);

            TankKaart anderTankkaart = _tankKaartNepRepo.GeefTankKaart("56231545856320589751");

            var ex = Assert.Throws<BestuurderException>(() => bestuurder.VoegTankKaartToe(anderTankkaart));
            Assert.Equal("Bestuurder heeft al een Tankkaart", ex.Message);
        }

        //Plaats null
        [Fact]
        public void GeefTankKaart_Null_InValid()
        {
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            TankKaart tankkaart = _tankKaartNepRepo.GeefTankKaart("1234567890123456789");

            var ex = Assert.Throws<BestuurderException>(() => bestuurder.VoegTankKaartToe(null));
            Assert.Equal("Ingegeven Tankkaart mag niet null zijn", ex.Message);
        }

        //Plaats entiteit en verwijder dezelfde entiteit
        [Fact]
        public void VerwijderTankKaart_Valid()
        {
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            TankKaart tankkaart = _tankKaartNepRepo.GeefTankKaart("1234567890123456789");

            bestuurder.VoegTankKaartToe(tankkaart);
            Assert.True(bestuurder.HeeftBestuurderTankKaart);

            bestuurder.VerwijderTankKaart(bestuurder.Tankkaart);
            Assert.False(bestuurder.HeeftBestuurderTankKaart);
            Assert.False(tankkaart.HeeftTankKaartBestuurder); ;  //reference type is nu ook losgekoppeld
        }

        //Plaats entiteit en probeer te verwijderen met een ander object
        [Fact]
        public void VerwijderTankKaart_InValid()
        {
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            TankKaart tankkaart = _tankKaartNepRepo.GeefTankKaart("1234567890123456789");

            bestuurder.VoegTankKaartToe(tankkaart);
            Assert.True(bestuurder.HeeftBestuurderTankKaart);

            TankKaart anderTankkaart = _tankKaartNepRepo.GeefTankKaart("56231545856320589751");

            var ex = Assert.Throws<BestuurderException>(() => bestuurder.VerwijderTankKaart(anderTankkaart));
            Assert.Equal("Tankkaart kan niet verwijderd worden", ex.Message);
        }

        //Plaats null
        [Fact]
        public void VerwijderTankKaart_Null_InValid()
        {
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            TankKaart tankkaart = _tankKaartNepRepo.GeefTankKaart("1234567890123456789");

            var ex = Assert.Throws<BestuurderException>(() => bestuurder.VerwijderTankKaart(null));
            Assert.Equal("Tankkaart mag niet null zijn", ex.Message);
        }
    }
}
