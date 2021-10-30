using FleetManagement.Model;
using FleetManagement.Test.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Test.Respositories
{
    class VoertuigNepRepo
    {
        //Key = ChassisNummer
        private readonly Dictionary<string, Voertuig> _voertuigen = new();

        public VoertuigNepRepo()
        {
            //Selecteerlijst voor Bestuurder
            //Persoon die Voeertuig test, configureert voor alle andere teamleden de juiste instanties 

            AutoModel model = new("Toyota","Celica", AutoType.GT);
            BrandstofType brandstof = new("Benzine");

            VoegVoertuigToe(new(1, model, "ABCDEFGHJKLMN1234", "1FEG830", brandstof, true)); //Key = ABCDEFGHJKLMN1234 
            VoegVoertuigToe(new(2, model, "1234ABCDEFGHJKLMN", "2FEG830", brandstof, false)); //Key = 1234ABCDEFGHJKLMN
            VoegVoertuigToe(new(model, "ABCDEFG1234HJKLMN", "3FEG830", brandstof,false)) ; //Key = ABCDEFG1234HJKLMN
        }

        public Voertuig GeefVoertuig(string chassisNummer)
        {
            if (IsVoertuigAanwezig(chassisNummer))
            {
                return _voertuigen[chassisNummer];
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
                throw new VoertuigNepRepoException("Voertuig staat al in de lijst");
            }
        }
    }
}
