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
    public class TankKaartNummerTest
    {
        [Theory]
        [InlineData("0003654120658978521")]
        [InlineData("1230564105987456325")]
        [InlineData("000365412065897852")]
        [InlineData("123056410598745632")]
        public void TankKaartNummerFormatCorrect(string tankKaartNummer)
        {
            bool check = CheckFormat.IsTankKaartNummerGeldig(tankKaartNummer);
            Assert.True(check);
        }

        [Theory]
        [InlineData("123056410598745632561")]
        [InlineData("123056410598745")]
        [InlineData("M001")]
        [InlineData("231045698")]
        [InlineData("-1231")]
        [InlineData(" ")]
        [InlineData("     ")]
        [InlineData("0")]
        [InlineData("000")]
        public void TankKaartFormatNietCorrect(string tankKaartNummer)
        {
            Assert.Throws<TankKaartException>(() => {
                CheckFormat.IsTankKaartNummerGeldig(tankKaartNummer);
            });
        }
    }
}