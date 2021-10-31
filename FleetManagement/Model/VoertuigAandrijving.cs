using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model
{
    public class VoertuigAandrijving : BrandstofType
    {
        public bool Hybride { get; set; }

        public string Aandrijving => Hybride ? "Hybride " + BrandstofNaam : BrandstofNaam;
        
        public VoertuigAandrijving(string brandstofNaam, bool isHybride) : base(brandstofNaam)
        {
            Hybride = isHybride;
        }
    }
}
