using FleetManagement.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Manager {
    public class TankkaartManager : ITankkaartRepository {
        public bool BestaatTankKaart(TankKaart tankkaart) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TankKaart> GeefAlleTankkaart() {
            throw new NotImplementedException();
        }

        public TankKaart GetTankKaart(string tankkaartNr) {
            throw new NotImplementedException();
        }

        public void UpdateTankKaart(TankKaart tankkaart) {
            throw new NotImplementedException();
        }

        public void VerwijderTankKaart(TankKaart tankkaart) {
            throw new NotImplementedException();
        }

        public void VoegTankKaartToe(TankKaart tankkaart) {
            throw new NotImplementedException();
        }

        public TankKaart ZoekTankKaart(string tankkaartNr, BrandstofType branstof) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TankKaart> ZoekTankKaarten(string tankkaartNr, BrandstofType brandstof) {
            throw new NotImplementedException();
        }
    }
}
