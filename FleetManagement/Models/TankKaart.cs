using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Models {
    public class TankKaart {

        //Zone Properties
        public int TankKaartId { get; private set; }
        public string KaartNummer { get; private set; }
        public DateTime VervalDatum { get; set; } 
        public DateTime UitgeefDatum { get; set; }
        public string Pincode { get; private set; } = string.Empty;
        public List<string> BrandstofType { get; private set; }
        public Bestuurder? Bestuurder { get; set; } = null;

        //Ctor 
        public TankKaart() { 
        }
        public TankKaart(int tankKaartId, string kaartNummer, DateTime vervalDatum, string pincode, DateTime uitgeefdatum/*, Bestuurder bestuurder*/) {
            TankKaartId = tankKaartId; 
            KaartNummer = kaartNummer;  
            VervalDatum = vervalDatum;
            UitgeefDatum = uitgeefdatum;
            Pincode = pincode; 
            BrandstofType = brandstofType;
            //this.bestuurder = bestuurder;
            //

            BrandstofType = new List<string>();
        }

        //Zone Methodes
        public bool IsTankKaartVervallen() {
            if (VervalDatum <= DateTime.Now) {
                return false;
            }
            return true;
        }
        public void BlokkeerTankKaart(string kaartnummer) {
            if (KaartNummer == kaartnummer) {
                kaartnummer = null;
            }
        }
        public void UpdatePincode(string nummer) {
            VoegPincodeToe(nummer);
        }

        //Te vragen van Tom of goed is hier anders kan verwijderd wordern! <NO Stress> 
        public void VoegKaartnummerIdToe(int id) {
            if (id > 0) {
                TankKaartId = id;
            } else {
                throw new TankKaartException("TankkaarId moet groter zijn dan 0");
            }
        }

        public void VoegKaartNummerToe(string kaartnummer) {
            if (!string.IsNullOrWhiteSpace(kaartnummer)) { 
                KaartNummer = kaartnummer;
            } else {
                throw new TankKaartException("Kaart nummer kan niet leeg zijn"); 
            }
        }

        public void VoegPincodeToe(string pincode) {
            if (!string.IsNullOrWhiteSpace(pincode)) {
                if (pincode.Length < 4) {
                    throw new TankKaartException("Pincode moet 4 karakter zijn"); 
                } else {
                    Pincode = pincode;
                }
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
        public void VoegBrandstofTypeToe(string brandstoftype) {
            brandstoftype = brandstoftype.ToLower().Trim();
            if (!BrandstofType.Contains(brandstoftype)) {
                BrandstofType.Add(brandstoftype);
            }
        }
        public void VerwijderBrandstofType(string brandstoftype) {
            brandstoftype = brandstoftype.ToLower().Trim();
            if (BrandstofType.Contains(brandstoftype)) {
                BrandstofType.Remove(brandstoftype);
            } else {
                throw new TankKaartException("Brandstof bestaat niet");
            }
        }
    }
}
