﻿using FleetManagement.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager
{
    public class AutoModelManager : IAutoModelRepository
    {
        private readonly IAutoModelRepository _repo;

        public AutoModelManager(IAutoModelRepository repo)
        {
            _repo = repo;
        }

        public AutoModel FilterOpAutoModelNaam(string autoModelNaam)
        {
            throw new NotImplementedException();
        }
    }
}
