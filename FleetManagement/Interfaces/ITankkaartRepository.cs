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
        bool BestaatTankKaart(TankKaart tankkaart);
        void VoegTankKaartToe(TankKaart tankkaart);
        void UpdateTankKaart(TankKaart tankkaart);
        TankKaart UpdateTankKaart(TankKaart tankkaart, string AnderTankkaartNummer);
        void VerwijderTankKaart(TankKaart tankkaart);
        public IReadOnlyList<BrandstofType> BrandstoffenVoorTankaart(TankKaart tankkaart);

        public void VerwijderBrandstoffen(TankKaart tankKaart);
        public void VoegTankkaartBrandstofToe(TankKaart tankkaart, BrandstofType brandstof);
        public bool BestaatTankkaartBrandstof(TankKaart tankkaart, BrandstofType brandstof);
    } 
}
