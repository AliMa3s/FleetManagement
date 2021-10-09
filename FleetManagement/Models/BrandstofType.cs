using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Models {
    public class BrandstofType {
        public int BrandstofTypeId { get; private set; }
        public string BrandstofNaam { get; private set; }

        public BrandstofType(int brandstofTypeId, string brandstofNaam) {
            BrandstofTypeId = brandstofTypeId;
            BrandstofNaam = brandstofNaam;
        }
    }
}
