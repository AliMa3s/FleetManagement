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
    public class AutoModelTest
    {
        [Fact]
        public void AutoModel_Ctor_Valid_NoId()
        {
            AutoModel automodel = new AutoModel("mercedes", "klasse-c", AutoType.Cabriolet);
            Assert.Equal("mercedes", automodel.Merk);
            Assert.Equal("klasse-c", automodel.AutoModelNaam);
            Assert.Equal(AutoType.Cabriolet, AutoType.Cabriolet);
        }
        [Fact]
        public void AutoModel_Ctor_Valid_WithId()
        {
            AutoModel automodel = new AutoModel(1,"mercedes", "klasse-c", AutoType.Cabriolet);
            Assert.Equal(1, automodel.AutoModelId);
            Assert.Equal("mercedes", automodel.Merk);
            Assert.Equal("klasse-c", automodel.AutoModelNaam);
            Assert.Equal(AutoType.Cabriolet, AutoType.Cabriolet);
        }

        [Fact]
        public void BrandstofType_fout_Id()
        {
            var e = Assert.Throws<AutoModelException>(() => new AutoModel(-100, "mercedes", "klasse-c", AutoType.Cabriolet));
            Assert.Equal("AutoModelId moet meer zijn dan 0", e.Message);
        }
    }
}
