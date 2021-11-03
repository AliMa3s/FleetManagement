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
        private readonly TankKaartNepRepo _tankKaartRepo = new();
        private readonly VoertuigNepRepo _voertuigRepo = new();

        
        [Fact]
        public void Id_Nummer_Correct()
        {
            Bestuurder bestuurder = new Bestuurder(1,"Filip", "Rigoir", "1976/03/31", "A,B", "1514081390", "76033101986");
            Assert.Equal(1, bestuurder.BestuurderId);
        }

        
        [Theory]
        [InlineData(-35)]
        [InlineData(0)]
        public void Id_Nummer_Incorrect(int id)
        {
            var e = Assert.Throws<BestuurderException>(() => {
                new Bestuurder(id, "Filip", "Rigoir", "1976/03/31", "A,B", "1514081390", "76033101986");
            });

            Assert.Equal("BestuurderId moet meer zijn dan 0", e.Message);

        }

       
        [Fact]
        public void Ctor_NoId_Valid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "1514081390", "76033101986");

            
            Assert.Equal("Filip", bestuurder.Voornaam);
            Assert.Equal("Rigoir", bestuurder.Achternaam);
            Assert.Equal("1976/03/31", bestuurder.GeboorteDatum);
            Assert.Equal("B,E+1", bestuurder.TypeRijbewijs);
            Assert.Equal("1514081390", bestuurder.RijBewijsNummer);
            Assert.Equal("76033101986", bestuurder.RijksRegisterNummer);

            Assert.Equal(0, bestuurder.BestuurderId);
            Assert.False(bestuurder.HeeftBestuurderVoertuig);
            Assert.False(bestuurder.HeeftBestuurderTankKaart);
            Assert.Null(bestuurder.Adres);

            bestuurder.VoegIdToe(1);
            Assert.Equal(1, bestuurder.BestuurderId);

        }

        [Fact]
        public void VoegId_Invalid()
        {
            Bestuurder bestuurder = new Bestuurder(1, "Filip", "Rigoir", "1976/03/31", "B,E+1", "1514081390", "76033101986");
            var ex = Assert.Throws<BestuurderException>(() => bestuurder.VoegIdToe(1));
            Assert.Equal("BestuurderId is al aanwezig en kan niet gewijzigd worden", ex.Message);
            
        }

        [Fact]
        public void Bestuurder_Adres_NotNull_Valid()
        {
            Bestuurder bestuurder = new Bestuurder(1,"Filip", "Rigoir", "1976/03/31", "B,E+1", "1514081390", "76033101986");

            bestuurder.Adres = new Adres(1,"L.Schuermanstraat","20","9040","Gent");

            Assert.NotNull(bestuurder.Adres);
        }
        
        [Fact]
        public void GeefVoertuig_Valid()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "1514081390", "76033101986");

            bestuurder.Adres = new Adres(1, "L.Schuermanstraat", "20", "9040", "Gent");

            bestuurder.VoegIdToe(1);

            bestuurder.VoegVoertuigToe(_voertuigRepo.GeefVoertuig("ABCDEFGHJKLMN1234"));

            Assert.True(bestuurder.HeeftBestuurderVoertuig);
        }
       
        [Fact]
        public void GeefTankKaart_Valid()
        {
            Bestuurder bestuurder = new Bestuurder(1,"Filip", "Rigoir", "1976/03/31", "B,E+1", "1514081390", "76033101986");

            bestuurder.Adres = new Adres(1, "L.Schuermanstraat", "20", "9040", "Gent");

            bestuurder.VoegTankKaartToe(_tankKaartRepo.GeefTankKaart("1234567890123456789"));
            Assert.True(bestuurder.HeeftBestuurderTankKaart);

        }


        [Fact]
        public void InstantieVergelijking()
        {

            
            Bestuurder bestuurder1 = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "1514081390", "76033101986");
            Bestuurder bestuurder2 = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "1514081390", "76033101986");
            Assert.True(bestuurder1.Equals(bestuurder2));

            
            Bestuurder bestuurder3 = new Bestuurder("Filip", "Rigoir", "2018-12-05", "B,E+1", "1514081390", "18120553401");
            Assert.False(bestuurder1.Equals(bestuurder3));
        }

        //This relatie testen (bestuurder & voertuig) 
        //ToDo

    }
}
