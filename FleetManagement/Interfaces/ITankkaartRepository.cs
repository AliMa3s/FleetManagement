using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces {
    public interface ITankkaartRepository {
        IReadOnlyList<TankKaart> GeefAlleTankkaart();
        IReadOnlyList<TankKaart> ZoekTankKaarten(string tankkaartNr, BrandstofType brandstof);
        TankKaart GetTankKaart(string tankkaartNr);
        TankKaart ZoekTankKaart(string tankkaartNr, BrandstofType branstof);
        bool BestaatTankKaart(TankKaart tankkaart);
        void VoegTankKaartToe(TankKaart tankkaart);
        void UpdateTankKaart(TankKaart tankkaart);
        void VerwijderTankKaart(TankKaart tankkaart);
    }
}
