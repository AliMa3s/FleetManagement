using FleetManagement.CheckFormats;
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
        [Fact]
        public void IsNummerPlaatGeldig()
        {
            bool check = CheckFormat.IsNummerplaatGeldig("1feg830");
            Assert.True(check);
        }
    }
}