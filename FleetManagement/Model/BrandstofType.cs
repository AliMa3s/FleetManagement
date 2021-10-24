using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model {

    public class BrandstofType {

        public int BrandstofTypeId { get; }
        public string BrandstofNaam { get; }

        public BrandstofType(string brandstofNaam) {

            if (!string.IsNullOrWhiteSpace(brandstofNaam))
            {
                BrandstofNaam = brandstofNaam;
            }
            else
            {
                throw new BrandstofTypeException("Brandstof kan niet leeg zijn");
            }
        }

        public BrandstofType(int brandstofTypeId, string brandstofNaam) : this(brandstofNaam) {

            if (brandstofTypeId > 0)
            {
                BrandstofTypeId = brandstofTypeId;
            }
            else
            {
                throw new BrandstofTypeException("BrandstofTypeId moet meer zijn dan 0");
            }
        }
    }
}