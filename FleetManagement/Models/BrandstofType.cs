﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model {

    public class BrandstofType {

        public int BrandstofTypeId { get; }

        public string BrandstofNaam { get; }

        public string BrandstofAfkorting { get; set; }

        //Properties uitbreiding mogelijk: etikettering, normen, beschrijving, enz.

        public BrandstofType(string brandstofNaam, string brandstofAfkorting) {
            BrandstofNaam = brandstofNaam;
            BrandstofAfkorting = brandstofAfkorting;
        }

        public BrandstofType(int brandstofTypeId, string brandstofNaam, string brandstofAfkorting)
            : this(brandstofNaam, brandstofAfkorting) {
            BrandstofTypeId = brandstofTypeId;
        }
    }
}