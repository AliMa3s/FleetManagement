using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test
{
    public class RijksRegisterNummerTest
    {
        [Theory]
        [InlineData("76033101986", "1976-03-31")]
        [InlineData("76033101986", "1976/03/31")]
        [InlineData("76030001946", "1976-03-00")]
        [InlineData("76003101965", "1976-00-31")]
        [InlineData("76000001925", "1976-00-00")]
        [InlineData("76000001925", "1976")]
        [InlineData("00033100090", "2000-03-31")]
        [InlineData("18120553401", "2018-12-05")]
        public void RijksRegisterNummerIsGeldig(string rijksRegisterNummer, string geboorteDatum) 
        {
            Assert.True(CheckFormats.CheckFormat.IsRijksRegisterGeldig(rijksRegisterNummer, geboorteDatum));
        }
    }
}
