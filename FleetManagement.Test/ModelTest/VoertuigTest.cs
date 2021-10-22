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
            //Selecteer een bestuurder uit de lijst
            Bestuurder bestuurder = _bestuurderRepo.GeefBestuurder("76033101986");

            //Maak een autoType & Benzine
            BrandstofType bezine = new("benzine");
            AutoModel automodel = new ("ferrari", "ferrari enzo", AutoType.GT);

            //Maak een voertuig aan
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);

            //Controleer dat TankKaart nog geen Bestuurder heeft
            Assert.False(voertuig.HeeftVoertuigBestuurder);

            //Voeg de bestuurder toe
            voertuig.VoegBestuurderToe(bestuurder);

            //controleer nu dat bestuurder aanwezig is
            Assert.True(voertuig.HeeftVoertuigBestuurder);

            //Controleer de relatie: Bestuurder moet nu ook het voertuig hebben
            Assert.True(voertuig.Bestuurder.HeeftBestuurderVoertuig);

            //Controleer dat alle chassisNummers gelijk zijn
            Assert.Equal(voertuig.ChassisNummer, voertuig.Bestuurder.Voertuig.ChassisNummer);
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.ChassisNummer);
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.Bestuurder.Voertuig.ChassisNummer);

            //Controleer de Nummerplaten
            Assert.Equal(voertuig.NummerPlaat, voertuig.Bestuurder.Voertuig.NummerPlaat);
            Assert.Equal("1ABC495", voertuig.NummerPlaat);
            Assert.Equal("1ABC495", voertuig.Bestuurder.Voertuig.NummerPlaat);

            //Voeg een andere Bestuurder toe via Voertuig (selecteer ander Bestuurder in repo)
            Bestuurder anderBestuurder = _bestuurderRepo.GeefBestuurder("76003101965");

            var ex = Assert.Throws<VoertuigException>(() => { 
                 voertuig.VoegBestuurderToe(anderBestuurder);
            });

            Assert.Equal($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}", ex.Message);

            //Voeg nu een ander bestuurder toe via de relatie
            ex = Assert.Throws<VoertuigException>(() => {
                voertuig.Bestuurder.Voertuig.VoegBestuurderToe(anderBestuurder);
            });

            Assert.Equal($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}", ex.Message);

            //Voeg een Voertuig toe via de relatie
            var ex2 = Assert.Throws<BestuurderException>(() => {
                voertuig.Bestuurder.VoegVoertuigToe( 
                        new Voertuig(5, automodel, "GDTKBSD1256YFES56", "2BDO563", bezine)
                    );
            });

            Assert.Equal($"{nameof(Bestuurder)} heeft al een {nameof(Voertuig)}", ex2.Message);

            //Controleer de eerste Bestuurder uit repo die we hebben toegevoegd
            //Via Rreference Type moet dat gekoppeld zijn aan het Voertuig
            Assert.True(bestuurder.HeeftBestuurderVoertuig);


            //Probeer eerst anderBestuurder mee te geven om te verwijderen      
             var ex3 = Assert.Throws<VoertuigException>(() => {
                 voertuig.VerwijderBestuurder(anderBestuurder);
             });

            Assert.Equal($"{nameof(Bestuurder)} kan niet worden verwijderd", ex3.Message);

            //Probeer nog eens null mee te geven om te verwijderen  
            var ex4 = Assert.Throws<VoertuigException>(() => {
                voertuig.VerwijderBestuurder(null);
            });

            Assert.Equal($"Ingegeven {nameof(Bestuurder)} mag niet null zijn", ex4.Message);

            //Verwijder nu de juiste Bestuurder
            voertuig.VerwijderBestuurder(bestuurder);

            //Controleer voertuig & bestuurder, beide moeten losgekoppeld zijn
            Assert.False(voertuig.HeeftVoertuigBestuurder);
            Assert.False(bestuurder.HeeftBestuurderVoertuig); //Reference Type is ook null
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
