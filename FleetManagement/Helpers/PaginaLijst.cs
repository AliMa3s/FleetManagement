using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Helpers
{
    public class PaginaLijst<T> : List<T>
    {
        public int HuidigePagina { get; }
        public int WeergavePerPagina { get; }
        public int TotaleResultaten { get; }
        public int AantalPaginaNummers { get; }
        public bool HeeftVorigePagina => HuidigePagina > 1;
        public bool HeeftVolgendePagina => HuidigePagina < AantalPaginaNummers;

        public PaginaLijst(List<T> lijst, int totaleResultaten, int huidigePagina, int weergavePerPagina)
        {
            TotaleResultaten = totaleResultaten;
            HuidigePagina = huidigePagina;
            WeergavePerPagina = weergavePerPagina;
            AantalPaginaNummers = (int)Math.Ceiling(totaleResultaten / (double)weergavePerPagina);

            AddRange(lijst);
        }
    }
}
