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
        public void AutoTypenaam_Valid()
        {
            AutoType autoType = new("porche");
            Assert.Equal("porche", autoType.AutoTypeNaam);
        }
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("\n")]
        public void AutoTypenaam_Invalid(string autoTypeNaam)
        {
            var mijnException = Assert.Throws<AutoTypeException>(() =>
            {
                new AutoType(autoTypeNaam);
            });
            Assert.Equal("AutoType moet ingevuld zijn", mijnException.Message);
           
        }
        
    }
}
