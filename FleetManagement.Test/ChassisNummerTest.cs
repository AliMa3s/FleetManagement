using FleetManagement.CheckFormats;
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
        public void ChassieNummerIsGeldig(string ChassisNummer)
        {
            bool check = CheckFormat.IsChassisNummerGeldig(ChassisNummer);
            Assert.True(check);
        }
    }
}