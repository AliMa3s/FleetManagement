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
            
            AutoModel autoModel = new("mercedes", "c-klasse", new AutoType("GT"));

            Voertuig voertuig = new(autoModel, "WAUZZZ8V5KA106598", "1AYB020", brandstof);
            Assert.True(voertuig.Brandstof.Hybride);
            Assert.Equal("Diesel", voertuig.Brandstof.BrandstofNaam);
        }
        [Fact]
        public void Voertuig_NoId_Valid()
        {
            BrandstofVoertuig brandstof = new("Diesel", false);
            AutoModel autoModel = new("mercedes", "c-klasse", new AutoType("GT"));
            Voertuig voertuig = new(autoModel, "WAUZZZ8V5KA106598", "1AYB020", brandstof);
            Assert.Equal("mercedes", voertuig.AutoModel.Merk);
            Assert.Equal("c-klasse", voertuig.AutoModel.AutoModelNaam);
            Assert.Equal(new AutoType("GT"), voertuig.AutoModel.AutoType);
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.ChassisNummer);
            Assert.Equal("1AYB020", voertuig.NummerPlaat);
            Assert.Equal("Diesel", voertuig.Brandstof.BrandstofNaam);

            Assert.Null(voertuig.VoertuigKleur);
            Assert.Null(voertuig.AantalDeuren);

            Assert.Equal(0, voertuig.VoertuigId);
        }

        [Fact]
        public void Voertuig_VerplichteVelden_Valid()
        {
            BrandstofVoertuig brandstof = new("Diesel", false);
            
            AutoModel autoModel = new("mercedes", "c-klasse", new AutoType("GT"));
            Voertuig voertuig = new(autoModel, "WAUZZZ8V5KA106598", "1AYB020", brandstof);

            Assert.Equal("mercedes", voertuig.AutoModel.Merk);
            Assert.Equal("c-klasse", voertuig.AutoModel.AutoModelNaam);
            Assert.Equal(new AutoType("GT"), voertuig.AutoModel.AutoType);
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.ChassisNummer);
            Assert.Equal("1AYB020", voertuig.NummerPlaat);
            Assert.Equal("Diesel", voertuig.Brandstof.BrandstofNaam);

            Assert.Null(voertuig.VoertuigKleur);
            Assert.Null(voertuig.AantalDeuren);
            Assert.Equal(0, voertuig.VoertuigId);
        }

        [Fact]
        public void Voertuig_Verkeerd_Id()
        {
            BrandstofVoertuig brandstof = new("diesel", false);

            AutoModel autoModel = new("mercedes", "c-klasse", new AutoType("GT"));

            var ex = Assert.Throws<VoertuigException>(() => {
                Voertuig voertuig = new(-10, autoModel, "WAUZZZ8V5KA106598", "1AYB020", brandstof);
            });

            Assert.Equal("VoertuigId moet meer zijn dan 0", ex.Message);
        }

        [Theory]
        [InlineData("Cabriolet")]        
        public void Valid_Voertuig_AutoType(string autoType)
        {
            Voertuig voertuig = new(new AutoModel("ferrari", "ferrari enzo", new AutoType(autoType)), "WAUZZZ8V5KA106598","1ABC599", new("benzine", false));    
            Assert.Equal(new AutoType(autoType), voertuig.AutoModel.AutoType);
        }
        [Fact]
        public void Valid_ChassisNummer()
        {
            Voertuig voertuig = new(
                new AutoModel("ferrari", "ferrari enzo", new AutoType("Coupé")), "WAUZZZ8V5KA106598", "1ABC599", new("benzine", false)
            );
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.ChassisNummer);
        }

        [Fact]
        public void Invalid_Chassisnummer()
        {
            Voertuig voertuig;
            var ex = Assert.Throws<ChassisNummerException>(() => voertuig = new(
                new AutoModel("ferrari", "ferrari enzo", new AutoType("Coupé")), "WAUZZZ8V5KA10659-5", "1ABC599", new("benzine", false))
            );
            Assert.Equal($"Chassisnummer moet string zijn van 17 cijfers/letters maar letter I/i, O/o en Q/q " +
                $"mag er niet in voorkomen",ex.Message);
        }

        [Fact]
        public void Verander_AutoKleur_Valid()
        {
            Voertuig voertuig = new(
                new AutoModel("ferrari", "ferrari enzo", new AutoType("Coupé")), "WAUZZZ8V5KA106598", "1ABC599", new("benzine", false)
            );
            voertuig.VoertuigKleur = new("Blauw");
            Assert.Equal(new Kleur("Blauw"), voertuig.VoertuigKleur);
        }
        
        [Fact]
        public void Verander_AantalDeuren()
        {
            Voertuig voertuig = new(
            new AutoModel("ferrari", "ferrari enzo", new AutoType("Coupé")),"WAUZZZ8V5KA106598", "1ABC599",new("benzine", false));
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
            AutoModel automodel = new("ferrari", "ferrari enzo", new AutoType("GT"));
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);
            voertuig.VoegBestuurderToe(bestuurder);
            
        }

        [Fact]
        public void VoegBestuurder_Invalid()
        {
            Bestuurder bestuurder = _bestuurderRepo.GeefBestuurder("");
            BrandstofVoertuig bezine = new("benzine", true);
            AutoModel automodel = new("ferrari", "ferrari enzo", new AutoType("GT"));
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);

            var ex = Assert.Throws<VoertuigException>(() => new Voertuig(automodel,"WAUZZZ8V5KA106598", "1ABC495", bezine).VoegBestuurderToe(bestuurder));
            Assert.Equal("Ingegeven Bestuurder mag niet null zijn",ex.Message);
        }

        [Fact]
        public void HeeftVoertuigBestuurder_Valid()
        {
            Bestuurder bestuurder = _bestuurderRepo.GeefBestuurder("76033101986");
            BrandstofVoertuig bezine = new("diesel", false);
            AutoModel automodel = new("ferrari", "ferrari enzo", new AutoType("GT"));
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);
            voertuig.VoegBestuurderToe(bestuurder);

            Assert.True(voertuig.HeeftVoertuigBestuurder);
        }
        [Fact]
        public void HeefVoertuigBestuurder_Invalid()
        {
            Bestuurder bestuurder = _bestuurderRepo.GeefBestuurder("76033101986");
            BrandstofVoertuig bezine = new("diesel", false);
            AutoModel automodel = new("ferrari", "ferrari enzo", new AutoType("GT"));
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);
            
            var ex = Assert.Throws<VoertuigException>(() =>
            {
                voertuig.VoegBestuurderToe(null);
            });
            Assert.Equal($"Ingegeven Bestuurder mag niet null zijn",ex.Message);
        }
        [Fact]
        public void VoegIdToe_Valid()
        {
            Bestuurder bestuurder = _bestuurderRepo.GeefBestuurder("");
            BrandstofVoertuig bezine = new("benzine", true);
            AutoModel automodel = new("ferrari", "ferrari enzo", new AutoType("GT"));
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);
            voertuig.VoegIdToe(1);
            Assert.Equal(1, voertuig.VoertuigId);
        }
        [Fact]
        public void VoegIdToe_Invalid_BestaandeId()
        {
            
            BrandstofVoertuig bezine = new("benzine", true);
            AutoModel automodel = new("ferrari", "ferrari enzo", new AutoType("GT"));
            Voertuig voertuig = new(1,automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);
            var ex = Assert.Throws<BestuurderException>(() =>
            {
                voertuig.VoegIdToe(1);
            });
            Assert.Equal($"VoertuigId is al aanwezig en kan niet gewijzigd worden", ex.Message);
        }
        [Fact]
        public void VoegIdToe_Invalid_Kleinerdan1()
        {
            BrandstofVoertuig bezine = new("benzine", true);
            AutoModel automodel = new("ferrari", "ferrari enzo", new AutoType("GT"));
            Voertuig voertuig = new( automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);
            var ex = Assert.Throws<BestuurderException>(() =>
            {
                voertuig.VoegIdToe(0);
            });
            Assert.Equal($"VoertuigId moet meer zijn dan 0", ex.Message);
        }
        [Fact]
        public void VoegVoertuig_Invalid_Null()
        {
            Bestuurder bestuurder1 = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            BrandstofVoertuig bezine = new("benzine", true);
            AutoModel automodel = new("ferrari", "ferrari enzo", new AutoType("GT"));
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);
            var ex = Assert.Throws<BestuurderException>(() =>
            {
                bestuurder1.VoegVoertuigToe(null);
            });
            Assert.Equal($"Ingegeven Voertuig mag niet null zijn", ex.Message);
        }
        [Fact]
        public void Nummerplaat_Valid()
        {
            BrandstofVoertuig bezine = new("benzine", true);
            AutoModel automodel = new("ferrari", "ferrari enzo", new AutoType("GT"));
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);

            Assert.Equal("1ABC495", voertuig.NummerPlaat);
        }
        [Fact]
        public void Nummerplaat_Invalid()
        {
            Bestuurder bestuurder1 = new Bestuurder("Filip", "Rigoir", "1976/03/31", "B,E+1", "76033101986");
            BrandstofVoertuig bezine = new("benzine", true);
            AutoModel automodel = new("ferrari", "ferrari enzo", new AutoType("GT"));
            
            var ex = Assert.Throws<NummerPlaatException>(() =>
            {
                Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1AB", bezine);
            });
            Assert.Equal($"Nummerplaat moet format [1-9AZ][a-z][0-9] zijn", ex.Message);
        }
    }
}
