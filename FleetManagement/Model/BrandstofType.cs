using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model {

    public class BrandstofType {

        public int BrandstofTypeId { get; }
        public string BrandstofNaam { get; private set; }

        public BrandstofType(string brandstofNaam) {
            VoegBrandstofToe(brandstofNaam);
        }

        public BrandstofType(int brandstofTypeId, string brandstofNaam) : this(brandstofNaam) {
            BrandstofTypeId = brandstofTypeId;
        }

        public void VoegBrandstofToe(string naam) {
            if (!string.IsNullOrWhiteSpace(naam)) {
                BrandstofNaam = naam;
            } else {
                throw new BrandstofTypeException("Brandstof kan niet leeg zijn");
            }
        }
    }
}