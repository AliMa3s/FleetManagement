using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model
{
    public class AutoModel
    {
        public int AutoModelId { get; set; }
        public string Merk { get; set; }
        public string AutoModelNaam { get; set; }
        public AutoType AutoType { get; set; }

        public AutoModel(string merk, string autoModelNaam, AutoType autoType) 
        {
            Merk = merk;
            AutoModelNaam = autoModelNaam;
            AutoType = autoType;
        }

        public AutoModel(int autoModelId, string merk, string autoModelNaam, AutoType autoType) 
            : this(merk, autoModelNaam, autoType)
        {
            if (autoModelId > 0)
            {
                AutoModelId = autoModelId;
            }
            else
            {
                throw new AutoModelException($"{nameof(AutoModelId)} moet meer zijn dan 0");
            }
        } 
    }
}

