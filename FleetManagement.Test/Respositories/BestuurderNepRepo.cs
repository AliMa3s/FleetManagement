using FleetManagement.Model;
using FleetManagement.Test.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Test.Respositories
{
    class BestuurderNepRepo
    {
        //Key = RijksRegisterNummer
        private readonly Dictionary<string, Bestuurder> _bestuurders = new();

        public BestuurderNepRepo()
        {
            //Selecteerlijst voor Voertuig & TankKaart
            //Persoon die Bestuurder test, configureert voor alle andere teamleden de juiste instanties 

            VoegBestuurderToe(new Bestuurder(1, "Filip","Rigoir", "1976-03-31", "B","1514081390", "76033101986")); //key = 76033101986
            VoegBestuurderToe(new Bestuurder(2, "Dirk", "Frimout", "1976-00-31", "C", "9514081390", "76003101965")); //key = 76003101965
            VoegBestuurderToe( new Bestuurder("Nathalie", "Meskens", "1976", "A,D,E+1", "9514081390", "76000001925")); //key = 76000001925
        }

        public Bestuurder GeefBestuurder(string rijksRegisterNummer)
        {
            if (IsBestuurderAanwezig(rijksRegisterNummer))
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
                throw new BestuurderNepRepoException("Bestuurder staat al in de lijst");
            }
        }
    }
}