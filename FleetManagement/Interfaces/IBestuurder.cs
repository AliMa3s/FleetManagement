
using FleetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces
{
    //Implementatie Bestuurder om de businessroles af te dwingen
    interface IBestuurder
    { 
        public void VoertuigToevoegen(Voertuig voertuig);
        public void VoertuigVerwijderen(Voertuig voertuig);
	    public void TankKaartToevoegen(TankKaart tankKaart);
	    public bool TankKaartVerwijderen(TankKaart tankKaart);
    }
}
