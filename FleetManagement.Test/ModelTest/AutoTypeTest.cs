using FleetManagement.Exceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test.ModelTest
{
    public class AutoTypeTest
    {
        [Fact]
        public void autoTypenaam_Valid()
        {
            AutoType autoType = new AutoType("porche");
            Assert.Equal("porche", autoType.AutoTypeNaam);
        }
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("\n")]
        public void autoTypenaam_Invalid(string autoTypeNaam)
        {
            var mijnException = Assert.Throws<AutoTypeException>(() =>
            {
                new AutoType(autoTypeNaam);
            });
            Assert.Equal("autoType naam mag niet null zijn", mijnException.Message);
           
        }
        
    }
}
