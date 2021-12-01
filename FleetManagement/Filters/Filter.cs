﻿using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Filters
{
    public class Filter
    {
        public string Kleuren { get; }
        public string AutoTypes { get; }
        public string Brandstof { get; }

        public Filter(List<string> kleuren, List<string> autoTypes, List<string> brandstof)
        {
#warning gooi excteption en test
            //if(kleuren == null) exception
            //if(autoTypes == null) exception
            //if(brandstof == null) exception

            if (kleuren.Count > 0)
                Kleuren = "'" + string.Join("','", kleuren) + "'";

            if(autoTypes.Count > 0)
                AutoTypes = "'" + string.Join("','", autoTypes) + "'";

            if(brandstof.Count > 0)
                Brandstof = "'" + string.Join("','", brandstof) + "'";
        }
    }
}
