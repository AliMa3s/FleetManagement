using FleetManagement.Exceptions;
using FleetManagement.Model;
using FleetManagement.Test.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test.ModelTest.ModelScenario
{
    public class ScenarioVoertuigToevoegen
    {
        private readonly BestuurderNepRepo _bestuurderRepo = new();

        [Fact]
        public void VoegBestuurderToe_En_Verwijder()
        {
            //Selecteer een bestuurder uit de lijst
            Bestuurder bestuurder = _bestuurderRepo.GeefBestuurder("76033101986");

            //Maak een autoType & Benzine
            BrandstofType bezine = new("benzine");
            AutoModel automodel = new("ferrari", "ferrari enzo", AutoType.GT);

            //Maak een voertuig aan (zonder ID)
            Voertuig voertuig = new(automodel, "WAUZZZ8V5KA106598", "1ABC495", bezine);

            //Controleer dat Voertuig nog geen Bestuurder heeft
            Assert.False(voertuig.HeeftVoertuigBestuurder);

            //Voeg de bestuurder toe
            voertuig.VoegBestuurderToe(bestuurder);

            //controleer nu dat bestuurder aanwezig is
            Assert.True(voertuig.HeeftVoertuigBestuurder);

            //Controleer de relatie: Bestuurder moet nu ook het voertuig kennen
            Assert.True(voertuig.Bestuurder.HeeftBestuurderVoertuig);

            //Controleer dat alle chassisNummers gelijk zijn
            Assert.Equal(voertuig.ChassisNummer, voertuig.Bestuurder.Voertuig.ChassisNummer);
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.ChassisNummer);
            Assert.Equal("WAUZZZ8V5KA106598", voertuig.Bestuurder.Voertuig.ChassisNummer);

            //Controleer de Nummerplaten
            Assert.Equal(voertuig.NummerPlaat, voertuig.Bestuurder.Voertuig.NummerPlaat);
            Assert.Equal("1ABC495", voertuig.NummerPlaat);
            Assert.Equal("1ABC495", voertuig.Bestuurder.Voertuig.NummerPlaat);

            //Voeg een andere Bestuurder toe via Voertuig (selecteer ander Bestuurder uit repo)
            Bestuurder anderBestuurder = _bestuurderRepo.GeefBestuurder("76003101965");

            var ex = Assert.Throws<VoertuigException>(() =>
            {
                voertuig.VoegBestuurderToe(anderBestuurder);
            });

            Assert.Equal($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}", ex.Message);

            //Voeg nu een ander bestuurder toe via de relatie
            ex = Assert.Throws<VoertuigException>(() =>
            {
                voertuig.Bestuurder.Voertuig.VoegBestuurderToe(anderBestuurder);
            });

            Assert.Equal($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}", ex.Message);

            //Voeg een Voertuig toe via de relatie
            var ex2 = Assert.Throws<BestuurderException>(() =>
            {
                voertuig.Bestuurder.VoegVoertuigToe(
                        new Voertuig(5, automodel, "GDTKBSD1256YFES56", "2BDO563", bezine)
                    );
            });

            Assert.Equal($"{nameof(Bestuurder)} heeft al een {nameof(Voertuig)}", ex2.Message);

            //Controleer de eerste Bestuurder uit repo die we hebben toegevoegd
            //Via Reference Type moet dat gekoppeld zijn aan het Voertuig
            Assert.True(bestuurder.HeeftBestuurderVoertuig);


            //Probeer eerst anderBestuurder mee te geven om te verwijderen      
            var ex3 = Assert.Throws<VoertuigException>(() =>
            {
                voertuig.VerwijderBestuurder(anderBestuurder);
            });

            Assert.Equal($"{nameof(Bestuurder)} kan niet worden verwijderd", ex3.Message);

            //Probeer nog eens null mee te geven om te verwijderen  
            var ex4 = Assert.Throws<VoertuigException>(() =>
            {
                voertuig.VerwijderBestuurder(null);
            });

            Assert.Equal($"Ingegeven {nameof(Bestuurder)} mag niet null zijn", ex4.Message);

            //Verwijder nu de juiste Bestuurder
            voertuig.VerwijderBestuurder(voertuig.Bestuurder);

            //Controleer voertuig & bestuurder, beide moeten losgekoppeld zijn
            Assert.False(voertuig.HeeftVoertuigBestuurder);
            Assert.False(bestuurder.HeeftBestuurderVoertuig); //Reference Type is ook null
        }


    }
}
