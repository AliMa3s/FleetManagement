using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model
{
    public class BrandstofVoertuig : BrandstofType
    {
        public override int BrandstofTypeId { get; }

        public bool Hybride { get; }

        public BrandstofVoertuig(string brandstofNaam, bool isHybride) : base(brandstofNaam)
        {
            Hybride = isHybride;
        }
        public BrandstofVoertuig(int brandstofid, string brandstofNaam, bool isHybride) : this(brandstofNaam, isHybride)
        {
            BrandstofTypeId = brandstofid;
        }
    }
}
