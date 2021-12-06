using FleetManagement.Interfaces;
using FleetManagement.ManagerExceptions;
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
            try
            {
                return _repo.GeefAlleVoertuigKleuren();
            }
            catch (Exception ex)
            {
                throw new KleurManagerException("Geef alle Kleuren is gefaald", ex);
            }

            
        }
    }
}
