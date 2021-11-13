using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager.Roles
{
    public class Role
    {
        public string Naam { get; private set; }
        public bool IsAdmin { get; set; }

        public Role(string naam, bool isAdmin)
        {
            Naam = naam;
            IsAdmin = isAdmin;
        }
    }
}
