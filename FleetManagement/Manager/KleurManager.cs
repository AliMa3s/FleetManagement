using FleetManagement.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager
{
    public class KleurManager : IVoertuigKleurRepository
    {
        private readonly IVoertuigKleurRepository _repo;

        public KleurManager(IVoertuigKleurRepository repo)
        {
            _repo = repo;
        }

        public IReadOnlyList<Kleur> GeefAlleVoertuigKleuren()
        {
            return _repo.GeefAlleVoertuigKleuren();
        }
    }
}
