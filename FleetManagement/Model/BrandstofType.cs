using FleetManagement.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model {

    public class BrandstofType : IComparable {

        public int BrandstofTypeId { get; }
        public string BrandstofNaam { get; }

        public BrandstofType(string brandstofNaam) {

            BrandstofNaam = !string.IsNullOrWhiteSpace(brandstofNaam) ? brandstofNaam
                : throw new BrandstofTypeException("Brandstof kan niet leeg zijn");
        }

        public BrandstofType(int brandstofTypeId, string brandstofNaam) : this(brandstofNaam) {

            BrandstofTypeId = brandstofTypeId > 0 ? brandstofTypeId 
                : throw new BrandstofTypeException("BrandstofTypeId moet meer zijn dan 0");
        }

#warning Testen: controleer dat sort() gaat
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj is BrandstofType)
            {
                BrandstofType anderBrandstofstype = obj as BrandstofType;
                return BrandstofNaam.CompareTo(anderBrandstofstype.BrandstofNaam);
            }

            return 0;
        }
    }
}