using FleetManagement.Model;
using FleetManagement.Test.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Test.Respositories
{
    class EnumNepManager : IEnumRepo
    {
        public Array GeefAantalDeuren()
        {
            return Enum.GetValues(new AantalDeuren().GetType());
        }
        //filip ik begrijp niet waarom jij hier "array" gebruikt hebt ipv void.
        //zo iets hebben wij niet geleerd zou je dit kunnen uitleggen wanner je tijd hebt volgende keer wanneer we vergaderen aub.
        
        /* 
         * Antwoord: Het is een lijst met alle mogelijke AutoTypes
         * Dit stelt de NepRepo voor die met dependency injection wordt geinjecteerd in de test
        */
        public Array GeefAutoTypes()
        {
            return Enum.GetValues(new AutoType().GetType());
        }

        public Array GeefKleuren()
        {
            return Enum.GetValues(new StatusKleur().GetType());
        }

        public bool ControleerDeuren(string aantal)
        {
            return Enum.IsDefined(typeof(AantalDeuren), aantal);
        }

        public bool ControleerAutoType(string autoType)
        {
            return Enum.IsDefined(typeof(AutoType), autoType);
        }

        public bool ControleerKleur(string kleur)
        {
            return Enum.IsDefined(typeof(StatusKleur), kleur);
        }
    }
}
