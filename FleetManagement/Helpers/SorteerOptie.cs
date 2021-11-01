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
        public string Sorteer { get; } = "ASC";
        public int HuidigePaginaNummer { get; }
        public int AantalPerPagina { get; }

        public SorteerOptie(string orderBy, string sorteerOptie, int huidigePaginaNummer, int aantalPerPagina)
        {
            OrderBy = orderBy;
            Sorteer = sorteerOptie;
            HuidigePaginaNummer = huidigePaginaNummer;

            if (aantalPerPagina > _maxPerPagina)
            {
                aantalPerPagina = _maxPerPagina;
            }

            AantalPerPagina = aantalPerPagina;
        }
    }
}
