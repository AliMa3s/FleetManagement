using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager.Roles
{
    public class Authenticatie
    {
        public IDictionary<string, Role> Roles { get; private set; } = new Dictionary<string, Role>();

        public Authenticatie()
        {
            Roles.Add("Filip Rigoir", new("Filip Rigoir", true));
            Roles.Add("Ali Maes", new("Ali Maes", true));
            Roles.Add("Ahmet Yilmaz", new("Ahmet Yilmaz", true));
            Roles.Add("Anoniem", new("Anoniem", false));
        }
    }
}
