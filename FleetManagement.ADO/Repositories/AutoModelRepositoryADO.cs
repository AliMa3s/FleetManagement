using FleetManagement.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.Repositories
{
    public class AutoModelRepositoryADO : RepoConnection, IAutoModelRepository
    {
        public AutoModelRepositoryADO(string connectionstring) : base(connectionstring) { }

        public AutoModel FilterOpAutoModelNaam(string autoModelNaam)
        {
            throw new NotImplementedException();
        }
    }
}
