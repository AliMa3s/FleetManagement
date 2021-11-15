using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test
{
    //moet nog worden geordend in meedere methods
    public class RijksRegisterNummerTest
    {
        [Theory]
        [InlineData("76033101986", "1976-03-31")]
        [InlineData("76033101986", "1976/03/31")]
        [InlineData("76030001946", "1976-03-00")]
        [InlineData("76030001946", "1976-03")]
        [InlineData("76003101965", "1976-00-31")]
        [InlineData("76000001925", "1976-00-00")]
        [InlineData("76000001925", "1976")]
        [InlineData("18120553401", "2018-12-05")]
        [InlineData("18250553492", "2018-25-05")]
        [InlineData("18450553438", "2018-45-05")]
        [InlineData("00000153447", "00000001")]
        public void RijksRegisterNummerIsGeldig(string rijksRegisterNummer, string geboorteDatum) 
        {
            Assert.True(CheckFormats.CheckFormat.IsRijksRegisterGeldig(rijksRegisterNummer, geboorteDatum));
        }

        [Theory]
        [InlineData("76033101905", "1976-03-31")]
        [InlineData("76033101998", "1976/03/31")]
        [InlineData("76033101998", "1976/03/316")]
        [InlineData("76033101p86", "1976/03/31")]
        [InlineData("76033101986", "31/03/1976")]
        [InlineData("76033201956", "31/03/1976")]
        [InlineData("76033109986", "31/03/1976")]
        [InlineData("76043101986", "31/03/1976")]
        [InlineData("76043101986", "3103-19/76")]
        [InlineData("00450553419", "0000")]
        public void RijksRegisterNummerOngeldig(string rijksRegisterNummer, string geboorteDatum)
        {
            Assert.Throws<RijksRegisterNummerException>(() => {
                CheckFormats.CheckFormat.IsRijksRegisterGeldig(rijksRegisterNummer, geboorteDatum);
            });
        }
    }
}

