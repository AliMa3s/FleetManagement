using FleetManagement.Exceptions;
using FleetManagement.Model;
using FleetManagement.Test.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test.ModelTest {
    public class VoertuigTest {

       private readonly BestuurderNepRepo _bestuurderRepo = new();

        [Fact]
        public void IsHybride_Valid()
        {
            bool ishybride = true;
            BrandstofVoertuig brandstof = new("Diesel", ishybride);
            
            AutoModel autoModel = new("mercedes", "c-klasse", AutoType.GT);

            Voertuig voertuig = new(autoModel, "WAUZZZ8V5KA106598", "1AYB020", brandstof);
            Assert.True(voertuig.Brandstof.Hybride);
            Assert.Equal("Hybride Diesel", voertuig.Brandstof.aandrijving);
            Assert.Equal("Diesel", voertuig.Brandstof.BrandstofNaam);
        }

        [Fact]
        public void Voertuig_VerplichteVelden_Valid()
        {
            BrandstofVoertuig brandstof = new("Diesel", false);
            
            AutoModel autoModel = new("mercedes", "c-klasse", AutoType.GT);
            Voertuig voertuig = new(autoModel, "WAUZZZ8V5KA106598", "1AYB020", brandstof);

            Assert.Equal("mercedes", voertuig.AutoModel.Merk);
            Assert.Equal("c-klasse", voertuig.AutoModel.AutoModelNaam);
            Assert.Equal(AutoType.GT, voertuig.AutoModel.AutoType);
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.ChassisNummer);
            Assert.Equal("1AYB020", voertuig.NummerPlaat);
            Assert.Equal("Diesel", voertuig.Brandstof.aandrijving);

            Assert.Null(voertuig.VoertuigKleur);
            Assert.Null(voertuig.AantalDeuren);
            Assert.Equal(0, voertuig.VoertuigId);
        }

        [Fact]
        public void Voertuig_Verkeerd_Id()
        {
            BrandstofVoertuig brandstof = new("diesel", false);

            AutoModel autoModel = new("mercedes", "c-klasse", AutoType.GT);

            var ex = Assert.Throws<VoertuigException>(() => {
                Voertuig voertuig = new(-10, autoModel, "WAUZZZ8V5KA106598", "1AYB020", brandstof);
            });

            Assert.Equal("VoertuigId moet meer zijn dan 0", ex.Message);
        }

        [Theory]
        [InlineData(AutoType.Cabriolet)]        
        public void Valid_Voertuig_AutoType(AutoType autoType)
        {
            Voertuig voertuig = new(new AutoModel("ferrari", "ferrari enzo",autoType), "WAUZZZ8V5KA106598","1ABC599", new("benzine", false));    
            Assert.Equal(AutoType.Cabriolet, voertuig.AutoModel.AutoType);
        }
        [Fact]
        public void Valid_ChassisNummer()
        {
            Voertuig voertuig = new(
                new AutoModel("ferrari", "ferrari enzo", AutoType.Coupé), "WAUZZZ8V5KA106598", "1ABC599", new("benzine", false)
            );
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.ChassisNummer);
        }

        [Fact]
        public void Invalid_Chassisnummer()
        {
            Voertuig voertuig;
            var ex = Assert.Throws<ChassisNummerException>(() => voertuig = new(
                new AutoModel("ferrari", "ferrari enzo", AutoType.Coupé), "WAUZZZ8V5KA10659-5", "1ABC599", new("benzine", false))
            );
            Assert.Equal($"Chassisnummer moet string zijn van 17 cijfers/letters maar letter I/i, O/o en Q/q " +
                $"mag niet voorkomen",ex.Message);
        }

        [Fact]
        public void Verander_AutoKleur_Valid()
        {
            Voertuig voertuig = new(
                new AutoModel("ferrari", "ferrari enzo", AutoType.Coupé), "WAUZZZ8V5KA106598", "1ABC599", new("benzine", false)
            );
            voertuig.VoertuigKleur = Kleur.Blauw;
            Assert.Equal(Kleur.Blauw, voertuig.VoertuigKleur);
        }
        
        [Fact]
        public void Verander_AantalDeuren()
        {
            Voertuig voertuig = new(
            new AutoModel("ferrari", "ferrari enzo", AutoType.Coupé),"WAUZZZ8V5KA106598", "1ABC599",new("benzine", false));
            voertuig.AantalDeuren = AantalDeuren.Zes;
            Assert.Equal(AantalDeuren.Zes, voertuig.AantalDeuren);

            voertuig.AantalDeuren = null;
            Assert.Null(voertuig.AantalDeuren);
        }

        [Fact]
        public void VoegBestuurder_Valid()
        {
            Bestuurder bestuurder = _bestuurderRepo.GeefBestuurder("76033101986");
            BrandstofVoertuig bezine = new("benzine", false);
            AutoModel automodel = new("ferrari", "ferrari enzo", AutoType.GT);
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);
            voertuig.VoegBestuurderToe(bestuurder);
            
        }

        [Fact]
        public void VoegBestuurder_Invalid()
        {
            Bestuurder bestuurder = _bestuurderRepo.GeefBestuurder("");
            BrandstofVoertuig bezine = new("benzine", true);
            AutoModel automodel = new("ferrari", "ferrari enzo", AutoType.GT);
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);

            var ex = Assert.Throws<VoertuigException>(() => new Voertuig(automodel,"WAUZZZ8V5KA106598", "1ABC495", bezine).VoegBestuurderToe(bestuurder));

        }

        [Fact]
        public void HeeftVoertuigBestuurder_Valid()
        {
            Bestuurder bestuurder = _bestuurderRepo.GeefBestuurder("76033101986");
            BrandstofVoertuig bezine = new("benzine", true);
            AutoModel automodel = new("ferrari", "ferrari enzo", AutoType.GT);
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);
            voertuig.VoegBestuurderToe(bestuurder);

            Assert.True(voertuig.HeeftVoertuigBestuurder);
        }
    }
}
