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

        //ID nummer is correct ingegeven
        [Fact]
        public void Id_Nummer_Correct()
        {
            Bestuurder bestuurder = new Bestuurder(1,"Filip", "Rigoir", "1976/03/31", "A,B", "1514081390", "76033101986");
            Assert.Equal(1, bestuurder.BestuurderId);
        }

        //Probeer een nul & negatief getal mee te geven
        [Fact]
        public void Id_Nummer_Incorrect()
        {
            var e = Assert.Throws<BestuurderException>(() => {
                new Bestuurder(-35, "Filip", "Rigoir", "1976/03/31", "A,B", "1514081390", "76033101986");
            });

            Assert.Equal("BestuurderId moet meer zijn dan 0", e.Message);

            e = Assert.Throws<BestuurderException>(() => {
                new Bestuurder(0, "Filip", "Rigoir", "1976/03/31", "A,B", "1514081390", "76033101986");
            });

            Assert.Equal("BestuurderId moet meer zijn dan 0", e.Message);
        }

        //Nieuwe instantie: met verplichte velden (zonder ID)
        [Fact]
        public void Instantie_Verplichte_Velden()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "1514081390", "76033101986");

            //Verplichte velden zijn aanwezig
            Assert.Equal("Filip", bestuurder.Voornaam);
            Assert.Equal("Rigoir", bestuurder.Achternaam);
            Assert.Equal("1976/03/31", bestuurder.GeboorteDatum);
            Assert.Equal("B,E+1", bestuurder.TypeRijbewijs);
            Assert.Equal("1514081390", bestuurder.RijBewijsNummer);
            Assert.Equal("76033101986", bestuurder.RijksRegisterNummer);

            //Niet verplichte velden bij instantie hoeven niet aanwezig te zijn
            Assert.Equal(0, bestuurder.BestuurderId);
            Assert.False(bestuurder.HeeftBestuurderVoertuig);
            Assert.False(bestuurder.HeeftBestuurderTankKaart);
            Assert.Null(bestuurder.Adres);

            //Voeg Id nummer toe 
            bestuurder.VoegIdToe(1);
            Assert.Equal(1, bestuurder.BestuurderId);

            //Probeer nog eenzs toe te voegen
            var e = Assert.Throws<BestuurderException>(() => {
                bestuurder.VoegIdToe(1);
            });

            Assert.Equal("BestuurderId is al aanwezig en kan niet gewijzigd worden", e.Message);

        }

        [Fact]
        public void Instantie_Aangevulde_Velden()
        {
            Bestuurder bestuurder = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "1514081390", "76033101986");

            //Voeg niet-verplichte toe
            bestuurder.Adres = new Adres(1,"L.Schuermanstraat","20","9040","Gent");

           //Voeg eerst ID toe want bestuurder moet uit een lijst geslecteerd worden
            bestuurder.VoegIdToe(1);

            //Voeg Voertuig & TankKaart toe
            bestuurder.VoegVoertuigToe(_voertuigRepo.GeefVoertuig("ABCDEFGHJKLMN1234"));
            bestuurder.VoegTankKaartToe(_tankKaartRepo.GeefTankKaart("1234567890123456789"));

            //Controleer de aanwezigheid
            Assert.Equal(1, bestuurder.BestuurderId);
            Assert.True(bestuurder.HeeftBestuurderVoertuig);
            //Assert.True(bestuurder.HeeftBestuurderTankKaart);
            Assert.NotNull(bestuurder.Adres);
        }

        [Fact]
        public void InstantieVergelijking()
        {

            //Zelfde instantie Gegevens
            Bestuurder bestuurder1 = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "1514081390", "76033101986");
            Bestuurder bestuurder2 = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "1514081390", "76033101986");
            Assert.True(bestuurder1.Equals(bestuurder2));

            //Ander RijsksRegisterNummer
            Bestuurder bestuurder3 = new Bestuurder("Filip", "Rigoir", "2018-12-05", "B,E+1", "1514081390", "18120553401");
            Assert.False(bestuurder1.Equals(bestuurder3));
        }

        //This relatie testen (bestuurder & voertuig) 
        //ToDo

    }
}
