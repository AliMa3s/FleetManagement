using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Filters
{
    public class Filter
    {
        public List<Kleur> Kleuren { get; }
        public List<AutoType> AutoTypes { get; }
        public List<BrandstofVoertuig> Brandstof { get; }

        public Filter(List<Kleur> kleuren, List<AutoType> autoTypes, List<BrandstofVoertuig> brandstof)
        {
            Kleuren = kleuren;
            AutoTypes = autoTypes;
            Brandstof = brandstof;
        }
    }
}
