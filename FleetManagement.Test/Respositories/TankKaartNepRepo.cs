using FleetManagement.Model;
using FleetManagement.Test.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Test.Respositories
{
    class TankKaartNepRepo
    {
        //Key = TankKaartNummer
        private readonly Dictionary<string, TankKaart> _tankKaarten = new();

        public TankKaartNepRepo()
        {
            //Selecteerlijst voor Bestuurder
            //Persoon die TankKaart test, configureert voor alle andere teamleden de juiste instanties 

            DateTime geldigheidsDatum = DateTime.Now.AddDays(365);

            VoegTankKaartToe(new("1234567890123456789", true, geldigheidsDatum)); //1234567890123456789
            //VoegTankKaartToe( new("1234567890123456789", geldigheidsDatum) );
            //VoegTankKaartToe( new("1234567890123456789", geldigheidsDatum) );
        }

        //ABCDEFGHJKLMN1234

        public TankKaart GeefTankKaart(string tankKaartNummer)
        {
            if (IsTankKaartAanwezig(tankKaartNummer))
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
                throw new TankKaartNepRepoException("TankKaart staat reeds in de lijst");
            }
        }
    }
}
