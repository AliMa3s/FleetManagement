using FleetManagement.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager
{
    public class BrandstofManager : IBrandstofRepository
    {
        private readonly IBrandstofRepository _repo;

        public BrandstofManager(IBrandstofRepository repo)
        {
            _repo = repo;
        }

        public IReadOnlyList<BrandstofType> GeeAlleBrandstoffen()
        {
            throw new NotImplementedException();
        }
    }
}
