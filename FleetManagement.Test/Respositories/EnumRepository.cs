using FleetManagement.Models;
using FleetManagement.Test.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Test.Respositories
{
    class EnumRepository : IEnumRepo
    {
        public Array GeefAantalDeuren()
        {
            return Enum.GetValues(new AantalDeuren().GetType());
        }

        public Array GeefAutoTypes()
        {
            return Enum.GetValues(new AutoType().GetType());
        }

        public Array GeefKleuren()
        {
            return Enum.GetValues(new StatusKleur().GetType());
        }
    }
}
