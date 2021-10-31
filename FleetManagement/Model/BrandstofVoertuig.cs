using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model
{
    public class BrandstofVoertuig : BrandstofType
    {
        public bool Hybride { get; set; }

        public string aandrijving => Hybride ? "Hybride " + BrandstofNaam : BrandstofNaam;
        
        public BrandstofVoertuig(string brandstofNaam, bool isHybride) : base(brandstofNaam)
        {
            Hybride = isHybride;
        }
    }
}
