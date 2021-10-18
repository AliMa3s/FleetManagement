using System;
using System.Collections.Generic;
using FleetManagement.Exceptions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Model {
    public class TankKaart {

        //Zone Properties
        //Tankkaar id verwijderd omdat kaartnummer is uid
        public string KaartNummer { get; private set; }
        public DateTime VervalDatum { get; set; }
        public DateTime UitgeefDatum { get; set; }
        public string Pincode { get; private set; } = string.Empty;
        public bool Actief { get; private set; } = true; //ingevoegd door Filip volgens instructies van Tom:
        public List<BrandstofType> BrandstofType { get; private set; }
        public Bestuurder Bestuurder { get; set; } = null;
        public bool HeeftTankKaartBestuurder => Bestuurder != null;

        //Ctor 
        public TankKaart(string kaartnummer, DateTime vervaldatum, string pincode = "") { //pincode mag leeg zijn, is niet verplicht

            //Ingevoegd door Filip: Check pincode via class static. Er wordt exception opgegooid als het niet voldoet aan het format
            if (CheckFormats.CheckFormat.IsTankKaartNummerGeldig(kaartnummer))
            {
                Pincode = pincode;
            }

            KaartNummer = kaartnummer;
            VervalDatum = vervaldatum;

            //Ingevoegd door Filip: Check pincode via class static. Er wordt exception opgegooid als het niet voldoet aan het format
            if(CheckFormats.CheckFormat.IsPincodeGeldig(pincode))
            {
                Pincode = pincode;
            }
        }

        public TankKaart(string kaartNummer, DateTime vervalDatum, string pincode, List<BrandstofType> brandstofType) 
            : this(kaartNummer, vervalDatum, pincode) {
            BrandstofType = brandstofType;
        }

        public TankKaart(string kaartNummer, DateTime vervalDatum, string pincode, DateTime uitgeefdatum, Bestuurder bestuurder)
        {
            KaartNummer = kaartNummer;
            VervalDatum = vervalDatum;
            UitgeefDatum = uitgeefdatum;
            Pincode = pincode;
            //BrandstofType = brandstofType;
            Bestuurder = bestuurder;


            BrandstofType = new List<BrandstofType>();
        }

        //Zone Methodes
        public bool IsTankKaartVervallen() {
            if (VervalDatum >= DateTime.Now) {
                return false;
            }
            return true;
        }
        public bool BlokkeerTankKaart(string kaartnummer) {
            if (KaartNummer == kaartnummer) {
                return true;
            }
            return false;

            //Wordt gewoon: zie property Actief
            //Actief = false;
        }
        public void UpdatePincode(string nummer) {
            VoegPincodeToe(nummer);
        }

        //Te vragen van Tom of goed is hier anders kan verwijderd wordern! <NO Stress> 
        //public void VoegKaartnummerIdToe(string kaartnummer) {
        //    if (id > 0) {
        //        TankKaartId = id;
        //    } else {
        //        throw new TankKaartException("TankkaarId moet groter zijn dan 0");
        //    }
        //}

        public void VoegKaartNummerToe(string kaartnummer) {
            if (!string.IsNullOrWhiteSpace(kaartnummer)) {
                KaartNummer = kaartnummer;
            } else {
                throw new TankKaartException("Kaart nummer kan niet leeg zijn");
            }
        }

        public void VoegPincodeToe(string pincode) {

            //Ingevoegd door Filip: Check pincode via class static. Er wordt exception opgegooid als het niet voldoet aan het format
            if (CheckFormats.CheckFormat.IsPincodeGeldig(pincode))
            {
                Pincode = pincode;
            }
        }
        //public void VoegBestuurderToe(Bestuurder bestuurder) {
        //    if (bestuurder != null) {
        //        Bestuurder = bestuurder;
        //        bestuurder.SetTankKaart(this);
        //    } else {
        //        throw new TankKaartException("Bestuurder mag niet null zijn.");
        //    }
        //}

        public void VoegBrandstofType(BrandstofType brandstoftype) {
            if (!BrandstofType.Contains(brandstoftype)) {
                BrandstofType.Add(brandstoftype);
            }
        }
        public void VerwijderBrandstofType(BrandstofType brandstoftype) {

            if (BrandstofType.Contains(brandstoftype)) {
                BrandstofType.Remove(brandstoftype);
            } else {
                throw new TankKaartException("Brandstof bestaat niet");
            }
        }
        public bool IsBrandstofTypeAanwezig(BrandstofType brandstof) {
            if (brandstof == null) throw new TankKaartException("Brandstof mag niet null zijn");

            if (BrandstofType.Contains(brandstof)) {
                return true; //omdat brandstof is string en method is bool dus kan geen string terug returnen
            }
            return false;
        }
        //Voeg bestuurder tankkaart
        public void VoegBestuurderAanTankKaart(Bestuurder bestuurder) {
            if (bestuurder != null) {

                if(Bestuurder == null)
                {
                    Bestuurder = bestuurder;
                }
                else
                {
                    throw new TankKaartException("Er is al een bestuurder aan de TankKaart toegevoegd");
                }

                //if (bestuurder.TankKaart == null) { //Dat is niet de taak van tankkaart, maar wel van bestuurder zelf
                //    Bestuurder = bestuurder;
                //    bestuurder.TankKaartToevoegen(this);
                //} else {
                //    throw new TankKaartException("Bestuurder heef tankkaart");
                //}
            }
            else
            {
                throw new TankKaartException("Bestuurder mag niet null zijn");
            }
        }


    }
}
