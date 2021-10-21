using FleetManagement.Exceptions;
using FleetManagement.Model;
using FleetManagement.Test.Interfaces;
using FleetManagement.Test.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test.ModelTest {
    public class VoertuigTest {

        
       private readonly IBestuurderNepRepo _bestuurderRepo = new BestuurderNepManager();

        [Fact]
        public void Test_VoegVoertuigToeMetKleur_Valid()
        {
            BrandstofType brandstof = new("diesel");

            AutoModel autoModel = new("mercedes", "c-klasse", AutoType.GT);
            Voertuig voertuig = new Voertuig(autoModel, "WAUZZZ8V5KA106598", "1AYB020", brandstof);

            Assert.Equal("mercedes", voertuig.AutoModel.Merk);
            Assert.Equal("c-klasse", voertuig.AutoModel.AutoModelNaam);
            Assert.Equal(AutoType.GT, AutoType.GT);
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.ChassisNummer);
            Assert.Equal("1AYB020", voertuig.NummerPlaat);
            Assert.Equal("diesel", voertuig.Brandstof.BrandstofNaam);

        }
        [Fact]
        public void VoegBestuurderToe_Valid()
        {
            Bestuurder bestuurder = new Bestuurder("ahmet", "yilmaz", "1976-03-31", "B","1234567891" , "76033101986");
            Voertuig voertuig = new Voertuig(new AutoModel("ferrari", "ferrari enzo", AutoType.GT), "WAUZZZ8V5KA106598", "1ABC495", new("benzine"));
            voertuig.VoegBestuurderToe(bestuurder);
        }
        
        [Fact]
        public void Test_CtorZonderKleur_VoertuigValid()
        {
            //er is iets mis met nummerplaat. 1abc495 => exception("1-abc-495") maar werkt niet.
            Voertuig voertuig = new Voertuig(new AutoModel("ferrari", "ferrari enzo", AutoType.GT), "WAUZZZ8V5KA106598","1ABC495",new("benzine"));
            Assert.Equal("ferrari", voertuig.AutoModel.Merk);
            Assert.Equal("ferrari enzo", voertuig.AutoModel.AutoModelNaam);
            Assert.Equal(AutoType.GT,AutoType.GT);
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.ChassisNummer);
            Assert.Equal("1ABC495", voertuig.NummerPlaat);
            Assert.Equal("benzine", voertuig.Brandstof.BrandstofNaam);
        }
        //zou een exception moeten geven.
        [Theory]
        [InlineData("1-AB-C495")]
        [InlineData("1aBC-495")]
        [InlineData("1AbC49-551")]
        [InlineData("ABC495-54")]
        [InlineData("1-AB-158")]
        public void Test_Ctor_InvalidNummerplaat(string nummerPlaat)
        {
            Voertuig voertuig;
            var ex=  Assert.Throws<NummerPlaatException>(() => voertuig = new(new AutoModel("ferrari","ferrari enzo",AutoType.GT),"WAUZZZ8V5KA106598",nummerPlaat,new("benzine")));
            Assert.Equal($"{nameof(nummerPlaat)} moet format [1-9][a-z][0-9] zijn", ex.Message);
        }

        [Theory]
        [InlineData("1ABC123")]
        [InlineData("1ABC085")]
        [InlineData("1ABC014")]
        public void Test_Ctor_ValidNummerPlaat(string nummerplaat)
        {
            Voertuig voertuig = new(new AutoModel("ferrari", "ferrari enzo", AutoType.GT), "WAUZZZ8V5KA106598", nummerplaat, new("benzine"));
            Assert.Equal(nummerplaat, voertuig.NummerPlaat);

        }
        [Theory]
        [InlineData(AutoType.Cabriolet)]        
        public void Test_valid_Voertuig_AutoType(AutoType autoType)
        {
            Voertuig voertuig = new(new AutoModel("ferrari", "ferrari enzo",autoType), "WAUZZZ8V5KA106598","1ABC599", new("benzine"));
            
            Assert.Equal(AutoType.Cabriolet, voertuig.AutoModel.AutoType);
            
          
        }
        [Fact]
        public void Test_Valid_ChassisNummer()
        {
            Voertuig voertuig = new(new AutoModel("ferrari", "ferrari enzo",AutoType.Coupé), "WAUZZZ8V5KA106598", "1ABC599", new("benzine"));
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.ChassisNummer);
        }
        [Theory]
        [InlineData("WAUZZZ8V5KA10659-5")]
        public void Test_Invalid_Chassisnummer(string chassisnummer)
        {
            Voertuig voertuig;
            var ex = Assert.Throws<ChassisNummerException>(() => voertuig = new(new AutoModel("ferrari", "ferrari enzo", AutoType.Coupé), chassisnummer, "1ABC599", new("benzine")));
            Assert.Equal($"{nameof(chassisnummer)} moet string zijn van 17 cijfers/letters maar letter I/i, O/o en Q/q mag niet voorkomen",ex.Message);
        }

        [Fact]
        public void Test_SetAutoKleur_Valid()
        {
            Voertuig voertuig = new(new AutoModel("ferrari", "ferrari enzo", AutoType.Coupé), "WAUZZZ8V5KA106598", "1ABC599", new("benzine"),StatusKleur.Blauw);
            Assert.Equal(StatusKleur.Blauw, voertuig.Kleur);
        }
        [Fact]
        public void Test_SetBrandStofType()
        {
            BrandstofType brandstof = new BrandstofType("diesel");
            Voertuig voertuig = new(new AutoModel("ferrari", "ferrari enzo", AutoType.Coupé), "WAUZZZ8V5KA106598", "1ABC599", new("benzine"));
            voertuig.SetBrandstof(brandstof);
            Assert.Equal(brandstof, voertuig.Brandstof);
        }
        [Fact]
        public void Test_SetAantalDeuren()
        {
            
            Voertuig voertuig = new(new AutoModel("ferrari", "ferrari enzo", AutoType.Coupé), "WAUZZZ8V5KA106598", "1ABC599", new("benzine"));
            voertuig.SetAantalDeuren(AantalDeuren.Zes);
            Assert.Equal(AantalDeuren.Zes, voertuig.AantalDeuren);

        }
        



        







    }
}
