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
        public int AutoModelId { get; private set; }
        public string Merk { get; private set; }
        public string AutoModelNaam { get; private set; }
        public AutoType AutoType { get; private set; }

        public AutoModel(string merk, string autoModelNaam, AutoType autoType) 
        {
#warning exceptions voor test

            if (string.IsNullOrWhiteSpace(merk)) { throw new AutoModelException("Merk moet ingevuld zijn"); }
            if (string.IsNullOrWhiteSpace(autoModelNaam)) { throw new AutoModelException("AutoModelnaam moet ingevuld zijn"); }
            Merk = merk;
            AutoModelNaam = autoModelNaam;
            AutoType = autoType ?? throw new AutoModelException("Autotype moet ingevuld zijn");
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
                throw new AutoModelException("AutoModelId moet meer zijn dan 0");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is AutoModel)
            {
                AutoModel ander = obj as AutoModel;
                return AutoModelId == ander.AutoModelId
                    && AutoModelNaam == ander.AutoModelNaam
                    && AutoType.AutoTypeNaam == ander.AutoType.AutoTypeNaam;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return AutoModelId.GetHashCode() ^ AutoModelNaam.GetHashCode();
        }
    }
}

