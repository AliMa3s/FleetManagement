using FleetManagement.Exceptions;
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
    public class BrandstofManager : IBrandstofRepository
    {
        private readonly IBrandstofRepository _repo;

        public BrandstofManager(IBrandstofRepository repo)
        {
            _repo = repo;
        }

        public IReadOnlyList<BrandstofType> GeeAlleBrandstoffen()
        {
            try
            {
                return _repo.GeeAlleBrandstoffen();
            }
            catch (Exception ex)
            {
                throw new BrandstofTypeManagerException(ex.Message);
            } 
        }
    }
}
