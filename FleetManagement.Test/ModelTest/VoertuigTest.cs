﻿using FleetManagement.Exceptions;
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
        public void Test_VoegVoertuigToe_Valid()
        {
            //Vraag eerst correcte instantie van Bestuurder aan in Repo: (zoals je selecteert in de lijst)
            Bestuurder bestuurderZonderIetsTeDoen = _bestuurderRepo.GeefBestuurder("76033101986");

            //Geeft zekerheid dat repo een instantie van bestuurder afleverd
            Assert.True(_bestuurderRepo.IsBestuurderAanwezig("76033101986"), "Bestuurder moet aanwezig zijn");

            //Maak uw Voertuig aan
            BrandstofType brandstofType = new BrandstofType("benzine");

            AutoModel autoModel = new AutoModel("ferrari", "ferrari enzo", AutoType.GT);
            Voertuig voertuig = new Voertuig(autoModel, "WAUZZZ8V5KA106598", "1ABC234", brandstofType);

            //Voeg nu bestuurder toe aan uw Voertuig
            Bestuurder bestuurder = new Bestuurder("ahmet", "yilmaz", "07/12/1998", "B", "flip flap flop", "12345678912");
            bestuurder.VoegVoertuigToe(voertuig);
            



            //Zo, hoef je je niet bezig te houden met een geldige bestuurder aan te maken
            //De bedoeling is dat je ook bij VoertuigNepManager geldige instantie aanmaakt van Voertuig,
            //meer hoef je niet te doen, zo ben ik ook in staat een geldig voertuig op te vragen
        }
        [Fact]
        public void Test_Ctor_VoertuigValid()
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
        public void Test_Ctor_InvalidNummerplaat(string nummerplaat)
        {
            Voertuig voertuig;
            var ex=  Assert.Throws<NummerPlaatException>(() => voertuig = new(new AutoModel("ferrari","ferrari enzo",AutoType.GT),"WAUZZZ8V5KA106598",nummerplaat,new("benzine")));
            Assert.Equal($"{nameof(nummerplaat)} moet format [1-9][a-z][0-9] zijn", ex.Message);
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
        //[InlineData(AutoType.Coupé)]
        //[InlineData(AutoType.GT)]
        //[InlineData(AutoType.Sedan)]
        //[InlineData(AutoType.Stationwagen)]
        //[InlineData(AutoType.SUV)]
        //[InlineData(AutoType.Tererreinwagen)]
        public void Test_valid_Voertuig_AutoType(AutoType autoType)
        {
            Voertuig voertuig = new(new AutoModel("ferrari", "ferrari enzo",autoType), "WAUZZZ8V5KA106598","1ABC599", new("benzine"));
            //Voertuig voertuig2 = new(new AutoModel("ferrari","ferrari enzo",autoType), "WAUZZZ8V5KA106598","1ABC599", new("benzine"));
            //Voertuig voertuig3 = new(new AutoModel("ferrari","ferrari enzo",autoType), "WAUZZZ8V5KA106598","1ABC599", new("benzine"));
            //Voertuig voertuig4 = new(new AutoModel("ferrari","ferrari enzo",autoType), "WAUZZZ8V5KA106598","1ABC599", new("benzine"));
            //Voertuig voertuig5 = new(new AutoModel("ferrari","ferrari enzo",autoType), "WAUZZZ8V5KA106598","1ABC599", new("benzine"));
            //Voertuig voertuig6 = new(new AutoModel("ferrari","ferrari enzo",autoType), "WAUZZZ8V5KA106598","1ABC599", new("benzine"));
            //Voertuig voertuig7= new(new AutoModel("ferrari","ferrari enzo",autoType), "WAUZZZ8V5KA106598","1ABC599", new("benzine"));
            //Voertuig voertuig8 = new(new AutoModel("ferrari","ferrari enzo",autoType), "WAUZZZ8V5KA106598","1ABC599", new("benzine"));
            Assert.Equal(AutoType.Cabriolet, voertuig.AutoModel.AutoType);
            //Assert.Equal(AutoType.Coupé, voertuig2.AutoModel.AutoType);
            //Assert.Equal(AutoType.GT, voertuig3.AutoModel.AutoType);
            //Assert.Equal(AutoType.Sedan, voertuig4.AutoModel.AutoType);
            //Assert.Equal(AutoType.Stationwagen, voertuig5.AutoModel.AutoType);
            //Assert.Equal(AutoType.SUV, voertuig6.AutoModel.AutoType);
            //Assert.Equal(AutoType.Tererreinwagen, voertuig7.AutoModel.AutoType);
            //Assert.Equal(AutoType.Tererreinwagen, voertuig8.AutoModel.AutoType);

        }
        [Fact]
        public void Test_Valid_ChassisNummer()
        {
            Voertuig voertuig = new(new AutoModel("ferrari", "ferrari enzo",AutoType.Coupé), "WAUZZZ8V5KA106598", "1ABC599", new("benzine"));
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.ChassisNummer);
        }
        [Theory]
        [InlineData("WAUZZZ8V5KA106598")]
        [InlineData("wAUZZZ8V5KA106598")]
        [InlineData("WAUZZZ8V5KA10659-5")]
        public void Test_Invalid_Chassisnummer(string chassisnummer)
        {
            Voertuig voertuig;
            var ex = Assert.Throws<ChassisNummerException>(() => voertuig = new(new AutoModel("ferrari", "ferrari enzo", AutoType.Coupé), chassisnummer, "1ABC599", new("benzine")));
            Assert.Equal(chassisnummer, $"{nameof(chassisnummer)} moet string zijn van 17 cijfers/letters maar letter I/i, O/o en Q/q mag niet voorkomen");
        }

        [Fact]
        public void Test_SetAutoKleur_Valid()
        {
            //autokleur ontbreekt nog in constructor
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
        



        //public void SetAutoKleur(StatusKleur kleur)
        //{
        //    Kleur = kleur;
        //}







    }
}
