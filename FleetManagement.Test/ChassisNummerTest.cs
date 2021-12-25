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
    public class ChassisNummerTest
    {
        [Theory]
        [InlineData("ABCDEFGHJKLMN1234")]
        [InlineData("0234DkzpsGUKNt6Gb")]
        public void ChassisnummerValid(string ChassisNummer)
        {
            bool check = CheckFormat.IsChassisNummerGeldig(ChassisNummer);
            Assert.True(check);
        }
        [Theory]
        [InlineData("OABCDEFGHJKLMN1234")]
        [InlineData("I0234DkzpGUKNt6Gb")]
        [InlineData("0Q234DkzpGUKNt6Gb")]
        //[InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("0234DkzpIGUKNt6GbK")]
        public void ChassisnummerInvalid(string ChassisNummer)
        {
            var ex = Assert.Throws<ChassisNummerException>(() => {
                CheckFormat.IsChassisNummerGeldig(ChassisNummer);
            });

            Assert.Equal("Chassisnummer moet string zijn van 17 cijfers/letters maar " +
                "letter I/i, O/o en Q/q mag niet voorkomen", ex.Message);
        }

        [Fact]
        public void ChassisnummerNullInvalid()
        {
            var ex = Assert.Throws<ChassisNummerException>(() => {
                CheckFormat.IsChassisNummerGeldig(null);
            });

            Assert.Equal("Chassisnummer mag niet null zijn", ex.Message);
        }
    } 
}
