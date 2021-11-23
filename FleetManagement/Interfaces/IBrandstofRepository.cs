using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces
{
    public interface IBrandstofRepository
    {
        public IReadOnlyList<BrandstofType> GeeAlleBrandstoffen();
    }
}
