using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Test.Interfaces
{
    interface IEnumRepo
    {
        public Array GeefKleuren();

        public Array GeefAutoTypes();

        public Array GeefAantalDeuren();

        public bool ControleerKleur(string kleur);

        public bool ControleerDeuren(string aantal);

        public bool ControleerAutoType(string autoType);

    }
}
