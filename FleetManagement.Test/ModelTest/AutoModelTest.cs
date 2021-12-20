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
            AutoType autotype = new("Cabriolet");
            AutoModel automodel = new("mercedes", "klasse-c", autotype);
            Assert.Equal("mercedes", automodel.Merk);
            Assert.Equal("klasse-c", automodel.AutoModelNaam);
            Assert.Equal(autotype, automodel.AutoType);
        }
        [Fact]
        public void AutoModel_Ctor_Valid_WithId()
        {
            AutoType autotype = new("Cabriolet");
            AutoModel automodel = new (1, "mercedes", "klasse-c", autotype);
            Assert.Equal(1, automodel.AutoModelId);
            Assert.Equal("mercedes", automodel.Merk);
            Assert.Equal("klasse-c", automodel.AutoModelNaam);
            Assert.Equal(autotype, automodel.AutoType);
        }
        
        [Fact]
        public void AutoModel_Ctor_Invalid_WithId()
        {
            var e = Assert.Throws<AutoModelException>(() => new AutoModel(-100, "mercedes", "klasse-c", new("Cabriolet")));
            Assert.Equal("AutoModelId moet meer zijn dan 0", e.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public void Merk_Invalid(string argument)
        {
            var e = Assert.Throws<AutoModelException>(() => new AutoModel(argument, "klasse-c", new AutoType("Cabriolet")));
            Assert.Equal("Merk moet ingevuld zijn", e.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public void AutoModelNaam_Invalid_Invalid(string argument)
        {
            var e = Assert.Throws<AutoModelException>(() => new AutoModel("mercedes", argument, new AutoType("Cabriolet")));
            Assert.Equal("AutoModelnaam moet ingevuld zijn", e.Message);
        }

        [Fact]
        public void AutoType_Invalid_Id()
        {
            var e = Assert.Throws<AutoModelException>(() => new AutoModel("mercedes", "klasse-c", null));
            Assert.Equal("Autotype moet ingevuld zijn", e.Message);
        }
    }
}
