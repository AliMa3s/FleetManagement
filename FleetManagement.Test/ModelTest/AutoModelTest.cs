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
            var autotype = new AutoType("Cabriolet");
            AutoModel automodel = new AutoModel("mercedes", "klasse-c", autotype);
            Assert.Equal("mercedes", automodel.Merk);
            Assert.Equal("klasse-c", automodel.AutoModelNaam);
            Assert.Equal(autotype, automodel.AutoType);
        }
        [Fact]
        public void AutoModel_Ctor_Valid_WithId()
        {
            var autotype = new AutoType("Cabriolet");
            AutoModel automodel = new AutoModel(1, "mercedes", "klasse-c", autotype);
            Assert.Equal(1, automodel.AutoModelId);
            Assert.Equal("mercedes", automodel.Merk);
            Assert.Equal("klasse-c", automodel.AutoModelNaam);
            Assert.Equal(autotype, automodel.AutoType);
        }

        [Fact]
        public void BrandstofType_Invalid_Id()
        {
            var e = Assert.Throws<AutoModelException>(() => new AutoModel(-100, "mercedes", "klasse-c", new AutoType("Cabriolet")));
            Assert.Equal("AutoModelId moet meer zijn dan 0", e.Message);
        }
        [Fact]
        public void Nieuwe_BrandstofVoertuig_Valid()
        {
            BrandstofVoertuig benzine = new("benzine", false);
            Assert.Equal("benzine", benzine.BrandstofNaam);
        }
        
        
    }
}
