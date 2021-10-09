using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Models {
    public class TankKaart {

        //Zone Properties
        public int TankKaartId { get; private set; }
        public string KaartNummer { get; private set; } //Is dat geen uniek ID door de bank bepaald? Kan nooit wijzigen
        public DateTime VervalDatum { get; set; } //Kan zeker niet wijzigen! Dat is wel een belangrijke! Je hebt een method (IsTankKaartVervallen) om te checken. Iemand kan nu zomaar verlengen van buitenaf. De Kaart is na het vervallen nooit meer te gebruiken.
        public DateTime UitgeefDatum { get; set; } //kan dat wijzigen? Vb.: Stundenkaart wordt uitegegevn in 2012. Kan dezelfde ooit 2013 worden? 
        public string Pincode { get; private set; }  //Zijn we zeker dat het enkel 4 digits zijn? Elke kaart kan ook 5 digits hebben. Weinige weten dat. Vraag aan Tom. Maar niet direct aub? Donderdag zodat iedereen de feedback hoort
        public List<string> BrandstofType { get; private set; }
        //public Bestuurder bestuurder { get; set; }  //overwegen om Bestuurder alleen toe te voegen als TankKaart niet is vervallen. Extra role dan: kan dan niet van buitenaf bepaald worden

        //Ctor 
        public TankKaart() {  //waarom een naamloze ctor? Je kan zomaar Tankkaart aanmaken zonder naam? Is dat wel toegestaan?
        }
        public TankKaart(int tankKaartId, string kaartNummer, DateTime vervalDatum, string pincode, List<string> brandstofType, DateTime uitgeefdatum/*, Bestuurder bestuurder*/) {
            TankKaartId = tankKaartId; 
            KaartNummer = kaartNummer;  
            VervalDatum = vervalDatum;
            UitgeefDatum = uitgeefdatum;
            Pincode = pincode;  //veld is niet verplicht, nu wel
            BrandstofType = brandstofType; //veld is niet verplicht nu wel | Je hebt al een list staan
            //this.bestuurder = bestuurder;  //veld is niet verplicht, nu wel           
        }

        //Zone Methodes
        public bool IsTankKaartVervallen() {
            if (VervalDatum < DateTime.Now) {
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
                throw new TankKaartException("TankkaarId moet groter zijn dan 0");  //Goede vraag voor Tom, dat schept zekerheid
            }
        }

        public void VoegKaartNummerToe(string kaartnummer) {
            if (!string.IsNullOrWhiteSpace(kaartnummer)) {  //TankKaartNummer is een Format. Dat moet nog verder gechekt worden. 
                KaartNummer = kaartnummer;
            } else {
                throw new TankKaartException("Kaart nummer kan niet leeg zijn"); 
            }
        }

        public void VoegPincodeToe(string pincode) {
            if (!string.IsNullOrWhiteSpace(pincode)) {
                if (pincode.Length < 4) {
                    throw new TankKaartException("Pincode moet 4 karakter zijn");  //Mss enkel als de kaart niet is geblokkeerd en een bestuurder heeft (is niet verplicht veld) waarom pincode veranderen als het is geblokkeerd? 
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
            if (!BrandstofType.Contains(brandstoftype)) {  //ik zou trim hier zetten, dan blijft je naam in Hoofdletter staan om te printen.
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

//Zoals gezegd, je past het zelf aan. T is alleen maar feedback. Je mag het ook verwijderen zonder iets te doen. T is vrijblijvend feedback.
//Gelieve de roles naast de methods te plaatsen (dat geeft motivatie waarom je op een bepaalde manier iets implementeert en je communiceert hier dan over)
//Voorbeeld: TanKaartNummer {get;} alleen get want Tankkaart is Uniek 
