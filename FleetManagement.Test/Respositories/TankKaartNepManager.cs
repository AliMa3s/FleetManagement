using FleetManagement.Model;
using FleetManagement.Test.Exceptions;
using FleetManagement.Test.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Test.Respositories
{
    class TankKaartNepManager : ITankKaartNepRepo
    {
        //Key = TankKaartNummer
        private readonly Dictionary<string, TankKaart> _tankKaarten = new();

        public TankKaartNepManager()
        {
            //Selecteerlijst voor Bestuurder
            //Persoon die TankKaart test, configureert voor alle andere teamleden de juiste instanties 

            //VoegTankKaartToe(TankKaart);
            //VoegTankKaartToe(TankKaart);
            //VoegTankKaartToe(TankKaart);
        }

        public TankKaart GeefTankKaart(string tankKaartNummer)
        {
            if (!IsTankKaartAanwezig(tankKaartNummer))
            {
                return _tankKaarten[tankKaartNummer]; //return null of object
            }

            return null;
        }

        public bool IsTankKaartAanwezig(string tankKaartNummer)
        {
            if (_tankKaarten.ContainsKey(tankKaartNummer))
            {
                return true;
            }

            return false;
        }

        private void VoegTankKaartToe(TankKaart tankKaart)
        {
            if (!IsTankKaartAanwezig(tankKaart.TankKaartNummer))
            {
                _tankKaarten.Add(tankKaart.TankKaartNummer, tankKaart);
            }
            else
            {
                throw new TankKaartNepManagerException("TankKaart staat reeds in de lijst");
            }
        }
    }
}
