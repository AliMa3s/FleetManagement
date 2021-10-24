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
    public class RijBewijstNummerTest
    {
        [Theory]
        [InlineData("5265748126")]
        [InlineData("1000000000")]
        public void RijbewijsNummerFormatCorrect(string rijbewijsNummer)
        {
            bool check = CheckFormat.IsRijbewijsNummerGeldig(rijbewijsNummer);
            Assert.True(check);
        }

        [Theory]
        [InlineData("-1000000000")]
        [InlineData("-100000000")]
        [InlineData("0632015236")]
        [InlineData("100000")]
        [InlineData("M001")]
        [InlineData("231045698")]
        [InlineData("-1231")]
        [InlineData(" ")]
        [InlineData("     ")]
        [InlineData("0")]
        [InlineData("000")]
        public void RijbewijsNummerFormatNietCorrect(string pincode)
        {
            Assert.Throws<RijBewijsNummerException>(() => {
                CheckFormat.IsRijbewijsNummerGeldig(pincode);
            });
        }
    }
}
