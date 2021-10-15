using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model
{
    public class AutoModel
    {
        public AutoModel() { }

        public int AutoModelId { get; set; }
        public string Merk { get; set; }
        public string AutoModelNaam { get; set; }
        public AutoType AutoType { get; set; }

    }
}

