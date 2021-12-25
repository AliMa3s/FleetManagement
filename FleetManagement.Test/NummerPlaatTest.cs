using FleetManagement.CheckFormats;
using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test
{
    public class NummerPlaatTest
    {
        [Theory]
        [InlineData("1FEG830")]
        [InlineData("2FEG830")]
        [InlineData("9FEG830")]
        [InlineData("1tgp520")]
        [InlineData("ztgp520")]
        [InlineData("eert520")]
        public void Nummerplaat_Valid(string nummerplaat)
        {
            Assert.True(CheckFormat.IsNummerplaatGeldig(nummerplaat));
        }

        [Theory]
        [InlineData("")]
        [InlineData("       ")]
        [InlineData("1-AB-C495")]
        [InlineData("1aBC-495")]
        [InlineData("1AbC49-551")]
        [InlineData("ABC495-54")]
        [InlineData("1-AB-158")]
        [InlineData("1tgp52")]
        [InlineData("tgp520")]
        public void NummerplaatInvalid(string nummerplaat)
        {
           var ex = Assert.Throws<NummerPlaatException>(() => {
                CheckFormat.IsNummerplaatGeldig(nummerplaat);
            });

            Assert.Equal("Nummerplaat moet beginnen met 1 cijfer/letter gevolgd door 3 letters en dan 3 cijfers", ex.Message);
        }

        [Fact]
        public void NummerplaatNullInvalid()
        {
            var ex = Assert.Throws<NummerPlaatException>(() => {
                CheckFormat.IsNummerplaatGeldig(null);
            });

            Assert.Equal("Nummerplaat mag niet null zijn", ex.Message);
        }
    }
}