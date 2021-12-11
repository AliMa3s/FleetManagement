using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces {

    public interface ITankkaartRepository
    {
        IReadOnlyList<TankKaart> GeefAlleTankkaarten();
        IReadOnlyList<TankKaart> ZoekTankKaarten(bool isTankkaartGeldig);
        TankKaart ZoekTankKaart(string tankkaartNr);
        IReadOnlyList<TankKaart> TankaartenZonderBestuurder();
        bool BestaatTankkaart(string tankkaartNummer);
        void VoegTankKaartToe(TankKaart tankkaart);
        void UpdateTankKaart(TankKaart tankkaart);
        TankKaart UpdateTankKaart(TankKaart tankkaart, string AnderTankkaartNummer);
        IReadOnlyList<BrandstofType> BrandstoffenVoorTankaart(TankKaart tankkaart);

        void VerwijderBrandstoffen(TankKaart tankKaart);
        void VoegTankkaartBrandstofToe(TankKaart tankkaart, BrandstofType brandstof);
        bool BestaatTankkaartBrandstof(TankKaart tankkaart, BrandstofType brandstof);
    } 
}
