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
        public void Nummerplaat_Valid(string nummerplaat)
        {
            bool check = CheckFormat.IsNummerplaatGeldig(nummerplaat);
            Assert.True(check);
        }

        [Theory]
        [InlineData("1-AB-C495")]
        [InlineData("1aBC-495")]
        [InlineData("1AbC49-551")]
        [InlineData("ABC495-54")]
        [InlineData("1-AB-158")]
        public void NummerplaatInvalid(string nummerplaat)
        {
            Assert.Throws<NummerPlaatException>(() => {
                CheckFormat.IsNummerplaatGeldig(nummerplaat);
            });
        }
    }
}