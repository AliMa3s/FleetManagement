using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Helpers
{
    public class SorteerOptie
    {
        private readonly int _maxPerPagina = 100;
        public string OrderBy { get; }
        public string Sort { get; }
        public int HuidigePaginaNummer { get; }
        public int AantalPerPagina { get; }

        public SorteerOptie(string orderBy, string sorteerOptie, int huidigePaginaNummer, int aantalPerPagina)
        {
            OrderBy = orderBy.Trim().ToLower();
            Sort = sorteerOptie.Trim().ToUpper() == "DESC" ? "DESC" : "ASC";
            HuidigePaginaNummer = huidigePaginaNummer < 1 ? 1 : huidigePaginaNummer;

            if (aantalPerPagina > _maxPerPagina)
            {
                aantalPerPagina = _maxPerPagina;
            }

            AantalPerPagina = aantalPerPagina;
        }
    }
}
