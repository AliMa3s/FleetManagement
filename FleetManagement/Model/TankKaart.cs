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
        public DateTime VervalDatum { get; }
        public DateTime UitgeefDatum { get; set; }
        public string Pincode { get; private set; } = string.Empty;
        public bool Actief { get; private set; } = true; //ingevoegd door Filip volgens instructies van Tom:
        public List<BrandstofType> BrandstofType { get; private set; } = new List<BrandstofType>();
        public Bestuurder Bestuurder { get; private set; } = null;
        public bool HeeftTankKaartBestuurder => Bestuurder != null;

        //Ctor 
        public TankKaart(string kaartnummer, bool actief, DateTime vervalDatum, string pincode = "")
        {
            //Ingevoegd door Filip: Check pincode via class static. Er wordt exception opgegooid als het niet voldoet aan het format
            if (CheckFormats.CheckFormat.IsTankKaartNummerGeldig(kaartnummer))
            {
                Pincode = pincode;
            }

            KaartNummer = kaartnummer;
            Actief = actief;
            VervalDatum = vervalDatum;

            //Ingevoegd door Filip: test slaagt anders niet als ingegeven datum is vervallen
            if (IsTankKaartVervallen())
            {
                Actief = false;
            }

            //Ingevoegd door Filip: Naar VoegPincodeToe
            if (pincode != string.Empty)
            {
                if (CheckFormats.CheckFormat.IsPincodeGeldig(pincode))
                {
                    Pincode = pincode;
                }
            }
        }

        public TankKaart(string kaartnummer, DateTime vervaldatum, string pincode = "")
            : this(kaartnummer, true, vervaldatum, pincode) { }  //Mogelijkheid zonder Status mee te geven

        public TankKaart(string kaartNummer, DateTime vervalDatum, string pincode, List<BrandstofType> brandstofType) 
            : this(kaartNummer, vervalDatum, pincode) {
            BrandstofType = brandstofType;
        }

        //Zone Methodes
        public bool IsTankKaartVervallen() {
            if (VervalDatum >= DateTime.Now) {
                return false;
            }
            return true;
        }
        public void BlokkeerTankKaart(string kaartnummer) {
            Actief = false;
        }

        //UpdatePincode kan niet verwijzen naar VoegPincodeToe
        //Dat is de reden waarom ik zei bij Tom: dat doet hetzelfde; dus dit mag weg
        //Ik heb het juiste uitgeschreven, met elk hun eigen exception
        public void UpdatePincode(string ingegevenPincode)
        {
            if (!Actief) throw new TankKaartException($"kan {Pincode} niet updaten want de TankKaart is niet (meer) actief");  

            if (Pincode != string.Empty)
            {
                //Ingevoegd door Filip: Check pincode via class static.
                if (ingegevenPincode == string.Empty)
                {
                    Pincode = ingegevenPincode;
                }
                else
                {
                    if (CheckFormats.CheckFormat.IsPincodeGeldig(ingegevenPincode))
                    {
                        Pincode = ingegevenPincode;
                    }
                }
            }
            else
            {
                throw new PincodeException($"Een lege {nameof(Pincode)} kan niet worden geüpdatet");
            }
        }

        //Te vragen van Tom of goed is hier anders kan verwijderd wordern! <NO Stress> 
        //public void VoegKaartnummerIdToe(string kaartnummer) {
        //    if (id > 0) {
        //        TankKaartId = id;
        //    } else {
        //        throw new TankKaartException("TankkaarId moet groter zijn dan 0");
        //    }
        //}

        //Dat mag niet mogelijk zijn. KaartNummer is geen ID uit onze DB maar wel van de bank.
        //Een kaart met een andere KaartNummer moet een nieuwe aangemaakt worden.

        //public void VoegKaartNummerToe(string kaartnummer) {
        //    if (!string.IsNullOrWhiteSpace(kaartnummer)) {
        //        KaartNummer = kaartnummer;
        //    } else {
        //        throw new TankKaartException("Kaart nummer kan niet leeg zijn"); //KaartNummer kan nooit leeg zijn! Static CheckFormat!!
        //    }
        //}

        //Bestaande vervallen TankKaarten met pincode gaan via constructor 
        public void VoegPincodeToe(string ingegevenPincode) {

            if (!Actief) throw new TankKaartException($"kan {Pincode} niet toevoegen want de TankKaart is niet (meer) actief");

            if (Pincode == string.Empty)
            {
                //Ingevoegd door Filip: Check pincode via class static.
                if (CheckFormats.CheckFormat.IsPincodeGeldig(ingegevenPincode))
                {
                    Pincode = ingegevenPincode;
                }
            }
            else
            {
                throw new PincodeException($"Er is al een {nameof(Pincode)} toegevoegd");
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

        public bool IsBrandstofAanwezig(BrandstofType brandstofType)
        {
            if (brandstofType == null) throw new TankKaartException("Brandstof mag niet null zijn");

            if (BrandstofType.Contains(brandstofType))
            {
                return true;
            }

            return false;
        }

        public void VoegBrandstofTypeToe(BrandstofType brandstofType) {
            if (!IsBrandstofAanwezig(brandstofType)) {
                BrandstofType.Add(brandstofType);
            }
        }
        public void VerwijderBrandstofType(BrandstofType brandstofType) {

            if (IsBrandstofAanwezig(brandstofType)) {
                BrandstofType.Remove(brandstofType);
            } else {
                throw new TankKaartException("Brandstof bestaat niet");
            }
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
