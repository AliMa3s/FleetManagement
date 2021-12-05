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
    public class PincodeTest
    {
        [Theory]
        [InlineData("0000")]
        [InlineData("0023")]
        [InlineData("1539")]
        [InlineData("9999")]
        [InlineData("98752")]
        [InlineData("00000")]
        [InlineData("63914")]
        [InlineData("99999")]
        public void PincodeFormatCorrect(string pincode)
        {
            bool check = CheckFormat.IsPincodeGeldig(pincode);
            Assert.True(check);
        }

        [Theory]
        [InlineData("000000")]
        [InlineData("M001")]
        [InlineData("KloP")]
        [InlineData("-1231")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("     ")]
        [InlineData("0")]
        [InlineData("000")]
        public void PincodeFormatNietCorrect(string pincode)
        {
            Assert.Throws<PincodeException>(() => {
                CheckFormat.IsPincodeGeldig(pincode);
            });
        }
    }
}