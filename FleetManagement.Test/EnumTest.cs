using FleetManagement.Test.Interfaces;
using FleetManagement.Test.Respositories;
using System;
using Xunit;

namespace FleetManagement.Test
{
    public class EnumTest
    {
        //Alle enums opvragen uit dezelfde locatie
        private readonly IEnumRepo _repoEnums = new EnumRepository();


        //Lijst met kleuren moeten gevuld zijn
        [Fact]
        public void KleurenLijstGevuld()
        {
            Array kleuren = _repoEnums.GeefKleuren();
            Assert.True(kleuren.Length > 0, $"Lijst {nameof(kleuren)} mag niet leeg zijn");
        }

        //Kleuren die zeker in de lijst moeten voorkomen; mag niet verwijderd worden
        [Theory]
        [InlineData("Wit")]
        [InlineData("Zwart")]
        [InlineData("Grijs")]
        [InlineData("Zilver")]
        [InlineData("Blauw")]
        [InlineData("Rood")]
        [InlineData("BruinBeige")]
        [InlineData("GoudGeel")]
        public void KleurAanwezig(string kleur)
        {
            bool isAutoTypeAanwezig = _repoEnums.ControleerKleur(kleur);
            Assert.True(isAutoTypeAanwezig, $"De Kleur {kleur} moet in de lijst voorkomen");
        }

         //Kleuren of tekst dat NIET in de lijst thuishoort
        [Theory]
        [InlineData("GeenKleur")]
        public void KleurNietAanwezig(string kleur)
        {
            bool isKleurNietAanwezig = _repoEnums.ControleerKleur(kleur);
            Assert.False(isKleurNietAanwezig, $"{kleur} mag niet in de lijst voorkomen");
        }


        //Lijst met AutoTypes die aanwezig moeten zijn
        [Fact]
        public void AutoTypeLijstGevuld()
        {
            Array autoTypes = _repoEnums.GeefAutoTypes();
            Assert.True(autoTypes.Length > 0, $"Lijst {nameof(autoTypes)} mag niet leeg zijn");
        }

        //AutoTypes dat in de lijst moet voorkomen; en niet mag verwijderd worden
        [Theory]
        [InlineData("Cabriolet")]
        [InlineData("Coupé")]
        [InlineData("GT")]
        [InlineData("Tererreinwagen")]
        [InlineData("Sedan")]
        [InlineData("Stationwagen")]
        [InlineData("SUV")]
        public void AutoTypeIsAanwezig(string autoType)
        {
            bool isAutoTypeAanwezig = _repoEnums.ControleerAutoType(autoType);
            Assert.True(isAutoTypeAanwezig, $"AutoType ({nameof(autoType)}) moeten in de lijst voorkomen");
        }

        //Aantal AutoTypes dat NIET in de lijst thuishoort (test ook dat zoeken de juiste resulaten oplevert)
        [Theory]
        [InlineData("Fiets")]
        public void AutoTypeIsNietAanwezig(string AutoType)
        {
            bool isAutoTypeAanwezig = _repoEnums.ControleerDeuren(AutoType);
            Assert.False(isAutoTypeAanwezig, $"({AutoType}) mag niet in de lijst voorkomen");
        }


        //Lijst met aantal deuren moeten gevuld zijn
        [Fact]
        public void AantalDeurenLijstGevuld()
        {
            Array deuren = _repoEnums.GeefAantalDeuren();
            Assert.True(deuren.Length > 0, $"Lijst {nameof(deuren)} mag niet leeg zijn");
        }

        //Het aantal deuren dat in de lijst moet voorkomen; en niet mag verwijderd worden
        [Theory]
        [InlineData("Twee")]
        [InlineData("Drie")]
        [InlineData("Vier")]
        [InlineData("Vijf")]
        [InlineData("Zes")]
        public void AantalDeurenAanwezig(string deuren) 
        {
            bool isAutoTypeAanwezig = _repoEnums.ControleerDeuren(deuren);
            Assert.True(isAutoTypeAanwezig, $"Het aantal deuren ({nameof(deuren)}) moeten in de lijst voorkomen");
        }

        //Aantal deuren dat NIET in de lijst thuishoort (test dat zoeken de juiste resulaten oplevert)
        [Theory]
        [InlineData("Een")]
        public void AantalDeurenNietAanwezig(string deuren)
        {
            bool isAutoTypeAanwezig = _repoEnums.ControleerDeuren(deuren);
            Assert.False(isAutoTypeAanwezig, $"Het aantal deuren ({deuren}) mag niet in de lijst voorkomen");
        }
    }
}
