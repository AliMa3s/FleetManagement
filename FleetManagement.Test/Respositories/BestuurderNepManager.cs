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
    class BestuurderNepManager : IBestuurderNepRepo
    {
        //Key = RijksRegisterNummer
        private readonly Dictionary<string, Bestuurder> _bestuurders = new();

        public BestuurderNepManager()
        {
            //Selecteerlijst voor Voertuig & TankKaart
            //Persoon die Bestuurder test, configureert voor alle andere teamleden de juiste instanties 

            //VoegBestuurderToe(Bestuurder);
            //VoegBestuurderToe(Bestuurder);
            //VoegBestuurderToe(Bestuurder);
        }

        public Bestuurder GeefBestuurder(string rijksRegisterNummer)
        {
            if (!IsBestuurderAanwezig(rijksRegisterNummer))
            {
                return _bestuurders[rijksRegisterNummer]; //return null of object
            }

            return null;
        }

        public bool IsBestuurderAanwezig(string rijksRegisterNummer)
        {
            if (_bestuurders.ContainsKey(rijksRegisterNummer))
            {
                return true;
            }

            return false;
        }

        private void VoegBestuurderToe(Bestuurder bestuurder)
        {
            if (!IsBestuurderAanwezig(bestuurder.RijksRegisterNummer))
            {
                _bestuurders.Add(bestuurder.RijksRegisterNummer, bestuurder);
            }
            else
            {
                throw new BestuurderNepManagerException("Bestuurder staat al in de lijst");
            }
        }
    }
}