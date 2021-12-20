using FleetManagement.Exceptions;
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
        public List<string> Kleuren { get; }
        public List<string> AutoTypes { get; }
        public List<string> Brandstoffen { get; }
        public bool Hybride { get; set; }

        public Filter(List<string> kleuren, List<string> autoTypes, List<string> brandstoffen, bool hybride = false)
        {
            Kleuren = kleuren ?? throw new FilterException("auto kleur mag niet null zijn");
            AutoTypes = autoTypes ?? throw new FilterException("autoType naam mag niet null zijn");
            Brandstoffen = brandstoffen ?? throw new FilterException("brandstofType mag niet null zijn");
            Hybride = hybride;
        }
    }
}
