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
#warning gooi excteption en test de inkomende args
            //if(kleuren == null) exception
            //if(autoTypes == null) exception
            //if(brandstof == null) exception

                Kleuren = kleuren;
                AutoTypes = autoTypes;
                Brandstoffen = brandstoffen;
                Hybride = hybride;
        }
    }
}
