using FleetManagement.Test.Interfaces;
using FleetManagement.Test.Respositories;
using System;
using Xunit;

namespace FleetManagement.Test
{
    public class EnumTest
    {
        //Alle enums opvragen vanuit dezelfde locatie
        private readonly IEnumRepo _repoEnums = new EnumRepository();

        [Fact]
        public void IsLijstKleurenOpgevuld()
        {
            Array kleuren = _repoEnums.GeefKleuren();
            Assert.True(kleuren.Length > 0, $"Lijst {nameof(kleuren)} mag niet leeg zijn");
        }

        [Fact]
        public void IsLijstAutoTypeOpgevuld()
        {
            Array autoTypes = _repoEnums.GeefAutoTypes();
            Assert.True(autoTypes.Length > 0, $"Lijst {nameof(autoTypes)} mag niet leeg zijn");
        }

        [Fact]
        public void IsLijstAantalDeurenOpgevuld()
        {
            Array aantalDeuren = _repoEnums.GeefAantalDeuren();
            Assert.True(aantalDeuren.Length > 0, $"Lijst {nameof(aantalDeuren)} mag niet leeg zijn");
        }
    }
}
