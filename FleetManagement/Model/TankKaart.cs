using System;
using System.Collections.Generic;
using FleetManagement.Exceptions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.CheckFormats;

namespace FleetManagement.Model {
    public class TankKaart {

        //Zone Properties
        public string TankKaartNummer { get; private set; }
        public DateTime GeldigheidsDatum { get; }
        public DateTime UitgeefDatum { get; set; }
        public string Pincode { get; private set; } = string.Empty;
        public bool Actief { get; private set; } = true; //ingevoegd door Filip volgens instructies van Tom:
        public List<BrandstofType> Brandstoffen { get; private set; } = new List<BrandstofType>();
        public Bestuurder Bestuurder { get; private set; } = null;
        public bool HeeftTankKaartBestuurder => Bestuurder != null;
        public bool IsGeldigheidsDatumVervallen => GeldigheidsDatum.Date <= DateTime.Today;

        //Ctor 
        public TankKaart(string kaartnummer, bool actief, DateTime geldigheidsDatum, string pincode = "")
        {
            if(string.IsNullOrEmpty(kaartnummer))
            {
                throw new TankKaartException($"{nameof(TankKaart)} Kan niet null of leeg zijn");
            }

            //Ingevoegd door Filip: Check KaartNummer
            if (CheckFormat.IsTankKaartNummerGeldig(kaartnummer))
            {
                TankKaartNummer = kaartnummer;
            }

            Actief = actief;
            GeldigheidsDatum = geldigheidsDatum;

            //Ingevoegd door Filip: test slaagt anders niet als ingegeven datum is vervallen
            if (IsGeldigheidsDatumVervallen)
            {
                Actief = false;
            }

            if (pincode != string.Empty)
            {
                //Ingevoegd door Filip: Naar static CheckFormat
                if (CheckFormat.IsPincodeGeldig(pincode))
                {
                    Pincode = pincode;
                }
            }
        }

        //Nieuw kaartnummer aanmaken = Actief op true. 
        //GeldigeheidsDatum wordt wel gecontroleerd of dat niet op false moet komen te staan
        public TankKaart(string kaartnummer, DateTime vervaldatum, string pincode = "")
            : this(kaartnummer, true, vervaldatum, pincode) { }

        //Ctor met Brandstof verwijderd. Je kan een lijst met dubbels ingeven dat niet gecontroleerd wordt.
        //We passeren de method zodat het altijd juist moet zijn (default is altijd lege lijst)

        public void BlokkeerTankKaart() {

            if(Actief)
            {
                Actief = false;
            }
            else
            {
                if(!IsGeldigheidsDatumVervallen)
                {
                     throw new TankKaartException($"{nameof(TankKaart)} is al geblokkeerd");
                }
                else
                {
                    throw new TankKaartException($"{nameof(TankKaart)} is reeds vervallen");
                }
            }
        }

        //UpdatePincode kan niet verwijzen naar VoegPincodeToe
        //Dat is de reden waarom ik zei bij Tom: dat doet hetzelfde; dus dit mag weg
        //Ik heb het juiste uitgeschreven, met elk hun eigen exception
        public void UpdatePincode(string ingegevenPincode)
        {
            if (String.IsNullOrEmpty(ingegevenPincode)) {
                throw new TankKaartException($"Ingegeven Pincode mag niet null zijn");
            }

            if (!Actief) 
                throw new TankKaartException($"Kan Pincode niet updaten want de TankKaart is niet (meer) actief");  

            if (Pincode != string.Empty)
            {
                if (String.IsNullOrEmpty(ingegevenPincode))
                {
                    Pincode = ingegevenPincode; //Pincode mag leeg zijn
                }
                else
                {
                    //Ingevoegd door Filip: Check pincode via class static.
                    if (CheckFormat.IsPincodeGeldig(ingegevenPincode))
                    {
                        Pincode = ingegevenPincode;  //Wanneer pincode is ingevuld moet het  aan de eisen voldoen
                    }
                }
            }
            else
            {
                throw new TankKaartException($"Een lege {nameof(Pincode)} kan niet worden geüpdatet");
            }
        }

        //Bestaande vervallen TankKaarten met pincode gaan via constructor
        public void VoegPincodeToe(string ingegevenPincode) {

            if(String.IsNullOrEmpty(ingegevenPincode)) {
                throw new TankKaartException("Ingegeven Pincode mag niet null zijn");
            }

            if (!Actief) 
                throw new TankKaartException("Kan Pincode niet toevoegen want de TankKaart is niet (meer) actief");

            if (Pincode == string.Empty)
            {
                //Ingevoegd door Filip: Check pincode via class static.
                if (CheckFormat.IsPincodeGeldig(ingegevenPincode))
                {
                    Pincode = ingegevenPincode;
                }
            }
            else
            {
                throw new TankKaartException("Er is al een Pincode toegevoegd");
            }
        }

        public bool IsBrandstofAanwezig(BrandstofType brandstofType)
        {
            if (brandstofType == null) throw new TankKaartException("Brandstof mag niet null zijn");

            if (Brandstoffen.Contains(brandstofType))
            {
                return true;
            }

            return false;
        }

        public void VoegBrandstofToe(BrandstofType brandstofType) {

            if (!IsBrandstofAanwezig(brandstofType)) {
                Brandstoffen.Add(brandstofType);
            }
        }
        public void VerwijderBrandstof(BrandstofType brandstofType) {

            if (IsBrandstofAanwezig(brandstofType)) {
                Brandstoffen.Remove(brandstofType);
            } else {
                throw new TankKaartException("Brandstof bestaat niet");
            }
        }

        //Voeg bestuurder toe aan tankkaart
        public void VoegBestuurderToe(Bestuurder ingegevenBestuurder) {
            if (ingegevenBestuurder == null) {
                throw new TankKaartException($"{nameof(Bestuurder)} mag niet null zijn");
            }

            if (!HeeftTankKaartBestuurder)
            {
                Bestuurder = ingegevenBestuurder;
                Bestuurder.VoegTankKaartToe("connecteren", this);
            }
            else
            {
                throw new TankKaartException($"Er is al een {nameof(Bestuurder)} aan de TankKaart toegevoegd");
            }
        }

        //Voeg bestuurder toe aan tankkaart
        public void VoegBestuurderToe(string actie, Bestuurder ingegevenBestuurder)
        {
            if (ingegevenBestuurder == null)
            {
                throw new TankKaartException($"{nameof(Bestuurder)} mag niet null zijn");
            }

            if (!HeeftTankKaartBestuurder && actie.ToLower() == "connecteren")
            {
                Bestuurder = ingegevenBestuurder;
            }
            else
            {
                throw new TankKaartException("Er is al een bestuurder aan de TankKaart toegevoegd");
            }
        }

        //Bellen
        public void VerwijderBestuurder(Bestuurder ingegevenBestuurder)
        {
            if (ingegevenBestuurder == null)
            {
                throw new TankKaartException($"Ingegeven {nameof(Bestuurder)} mag niet null zijn");
            }

            if (Bestuurder.Equals(ingegevenBestuurder))
            {
                Bestuurder.VerwijderTankKaart("deconnecteren", this);
                Bestuurder = null;
            }
            else
            {
                throw new TankKaartException($"{nameof(Bestuurder)} kan niet worden verwijderd");
            }
        }

        //Opnemen
        public void VerwijderBestuurder(string actie, Bestuurder ingegevenBestuurder)
        {
            if (ingegevenBestuurder != null)
            {
                throw new TankKaartException($"Er is geen {nameof(Bestuurder)} om te verwijderen");
            }

            if (Bestuurder.Equals(ingegevenBestuurder) && actie.ToLower() == "deconnecteren")
            {
                Bestuurder = null;
            }
            else
            {
                throw new TankKaartException($"{nameof(Bestuurder)} kan niet verwijderd worden");
            }
        }

        //Vergelijk twee instanties van TankKaart met: TankKaartNummer
        public override bool Equals(object obj)
        {
            if (obj is TankKaart)
            {
                TankKaart ander = obj as TankKaart;
                return TankKaartNummer == ander.TankKaartNummer 
                    && GeldigheidsDatum == ander.GeldigheidsDatum;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return TankKaartNummer.GetHashCode() ^ GeldigheidsDatum.GetHashCode();
        }
    }
}
