using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Exceptions
{
    [Serializable]
    public class BestuurderException : Exception //deze klasse was internal heb veranderd naar public 'ahmet'
    {
        public BestuurderException() { }

        public BestuurderException(string message) : base(message) { }

        public BestuurderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
