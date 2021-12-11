using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model
{
    public class AutoType
    {
        public string AutoTypeNaam { get; set;  }

        public AutoType(string autoTypeNaam)
        {
            if (string.IsNullOrWhiteSpace(autoTypeNaam)) throw new AutoTypeException("AutoType moet ingevuld zijn");
            

            AutoTypeNaam = autoTypeNaam;
        }

        #region Overridables
        //Vergelijk twee instanties van Kleur met: kleurnaam
        public override bool Equals(object obj)
        {
            if (obj is AutoType)
            {
                AutoType ander = obj as AutoType;
                return AutoTypeNaam == ander.AutoTypeNaam;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return AutoTypeNaam.GetHashCode();
        }
        #endregion
    }

}