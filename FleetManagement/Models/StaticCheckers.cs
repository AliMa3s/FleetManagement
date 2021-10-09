using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Exceptions;

namespace FleetManagement.Models {

    //Ik heb ClassNaam CheckFormat()
    public class StaticCheckers {

        public static bool IsPincodeGeldig(string pincode) {
            TankKaart tk = new TankKaart();
            if (pincode.Trim() != tk.Pincode) {
                throw new StaticCheckersException("Pincode is niet geldig");
            }
            return true;
        }
        //public static bool IsChassisNummerGeldig(string nr) { }
        //public static bool IsNummerplaatGeldig(string nr) { }
        //public static bool IsRijksRegisterGeldig(string nr) { }

    }
}
