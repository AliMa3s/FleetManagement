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
#warning nog een unit test maken voor filter if kleur => null gooi exception.
            //if(kleuren == null) exception
            //if(autoTypes == null) exception
            //if(brandstof == null) exception
            if (kleuren == null) throw new FilterException("auto kleur mag niet null zijn");
            if (autoTypes == null) throw new FilterException("autoType naam mag niet null zijn");
            if (brandstoffen == null) throw new FilterException("brandstofType mag niet null zijn");
                Kleuren = kleuren;
                AutoTypes = autoTypes;
                Brandstoffen = brandstoffen;
                Hybride = hybride;
        }
    }
}
