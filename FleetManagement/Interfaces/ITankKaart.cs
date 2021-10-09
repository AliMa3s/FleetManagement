using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces
{
    //Implementatie TankKaart om de businessroles af te dwingen
    //Verander naam indien gewenst en vul aan met methods die de businessroles beschrijven
    interface ITankKaart
    {
        public void UpdatePincode(string pincodeNummer);
        public bool IsTankKaartVervallen();
        public void BlokkeerTankKaart();
    }
}
