using FleetManagement.Exceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test.ModelTest {
    public class BrandstofTypeTest {
        [Fact]
        public void BrandstofType_Valid() {
            BrandstofType bstype = new BrandstofType("Gasoline");
            Assert.Equal("Gasoline", bstype.BrandstofNaam);
        }

        [Fact]
        public void BrandstofType_InValid() {
            var e = Assert.Throws<BrandstofTypeException>(() => new BrandstofType(""));
            Assert.Equal("Brandstof kan niet leeg zijn", e.Message);
        }

        [Fact]
        public void BrandstofType_Invalid_Id()
        {
            var e = Assert.Throws<BrandstofTypeException>(() => new BrandstofType(-15, "Hybride met diesel"));
            Assert.Equal("BrandstofTypeId moet meer zijn dan 0", e.Message);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void BrandstofType_Valid_Id(int id)
        {
            BrandstofType bstype = new BrandstofType(id,"Gasoline");
            Assert.Equal(id, bstype.BrandstofTypeId);
        }
    }
}
