﻿using FleetManagement.Model;
using FleetManagement.Test.Exceptions;
using FleetManagement.Test.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Test.Respositories
{
    class VoertuigNepManager : IVoertuigNepRepo
    {
        //Key = ChassisNummer
        private readonly Dictionary<string, Voertuig> _voertuigen = new();

        public VoertuigNepManager()
        {
            //Selecteerlijst voor Bestuurder
            //Persoon die Voeertuig test, configureert voor alle andere teamleden de juiste instanties 

            //VoegVoertuigToe(Voertuig);
            //VoegVoertuigToe(Voertuig);
            //VoegVoertuigToe(Voertuig);
        }

        public Voertuig GeefVoertuig(string chassisNummer)
        {
            if (!IsVoertuigAanwezig(chassisNummer))
            {
                return _voertuigen[chassisNummer]; //return null of object
            }

            return null;
        }

        public bool IsVoertuigAanwezig(string chassisNummer)
        {
            if (_voertuigen.ContainsKey(chassisNummer))
            {
                return true;
            }

            return false;
        }

        private void VoegVoertuigToe(Voertuig voertuig)
        {
            if (!IsVoertuigAanwezig(voertuig.ChassisNummer))
            {
                _voertuigen.Add(voertuig.ChassisNummer, voertuig);
            }
            else
            {
                throw new VoertuigNepManagerException("Voertuig staat al in de lijst");
            }
        }
    }
}