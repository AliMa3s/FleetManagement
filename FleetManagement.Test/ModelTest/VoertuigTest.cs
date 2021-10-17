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

        //Cadeau van Bestuurder voor Voertuig:
        private readonly IBestuurderNepRepo _bestuurderRepo = new BestuurderNepManager();

        //[Fact]
        public void VoorbeeldVoorAhmet()
        {
            //Vraag eerst correcte instantie van Bestuurder aan in Repo: (zoals je selecteert in de lijst)
            Bestuurder bestuurderZonderIetsTeDoen = _bestuurderRepo.GeefBestuurder("76033101986");

            //Geeft zekerheid dat repo een instantie van bestuurder afleverd
            Assert.True(_bestuurderRepo.IsBestuurderAanwezig("76033101986"), "Bestuurder moet aanwezig zijn");

            //Maak uw Voertuig aan


            //Voeg nu bestuurder toe aan uw Voertuig


            //Probeer nog eens bestuurder toe te voegen, je moet exception krijgen:


            //Zo, hoef je je niet bezig te houden met een geldige bestuurder aan te maken
            //De bedoeling is dat je ook bij VoertuigNepManager geldige instantie aanmaakt van Voertuig,
            //meer hoe je niet te doen, zo ben ik ook in staat een geldig voertuig op te vragen
        }

    }
}
