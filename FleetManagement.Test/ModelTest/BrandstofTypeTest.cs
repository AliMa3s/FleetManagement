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
        public void Test_BrandstofType_Valid() {
            BrandstofType bstype = new BrandstofType("Gasoline");
            bstype.VoegBrandstofToe("Diesel");
            Assert.Equal("Diesel", bstype.BrandstofNaam);
        }

        [Fact]
        public void Test_BrandstofType_InValid() {
            BrandstofType bstype = new BrandstofType("Gasoline");
            Assert.Throws<BrandstofTypeException>(() => bstype.VoegBrandstofToe(""));
        }

    }
}
