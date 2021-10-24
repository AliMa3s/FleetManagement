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
        [InlineData("")]
        [InlineData("     ")]
        [InlineData("0234DkzpIGUKNt6GbK")]
        public void ChassisnummerInValid(string ChassisNummer)
        {
            Assert.Throws<ChassisNummerException>(() => {
                CheckFormat.IsChassisNummerGeldig(ChassisNummer);
            });
        }
    } 
}