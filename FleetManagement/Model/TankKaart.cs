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
        public string Pincode { get; private set; } = null;
        public bool Actief { get; private set; } = true;
        public List<BrandstofType> Brandstoffen { get; private set; } = new List<BrandstofType>();
        public Bestuurder Bestuurder { get; private set; } = null;
        public bool HeeftTankKaartBestuurder => Bestuurder != null;
        public bool IsGeldigheidsDatumVervallen => GeldigheidsDatum.Date <= DateTime.Today;

        //Ctor 
        public TankKaart(string kaartnummer, bool actief, DateTime geldigheidsDatum, string pincode = null)
        {
            //Kaart mag niet null of leeg zijn
            if(string.IsNullOrEmpty(kaartnummer))
            {
                throw new TankKaartException($"{nameof(TankKaart)} Kan niet null of leeg zijn");
            }

            //Static class die tankkaartnummer controleert
            if (CheckFormat.IsTankKaartNummerGeldig(kaartnummer))
            {
                TankKaartNummer = kaartnummer;
            }

            Actief = actief;
            GeldigheidsDatum = geldigheidsDatum;

            //Controleert of de kaart in tussentijds niet is vervallen
            if (IsGeldigheidsDatumVervallen)
            {
                Actief = false;
            }

            //Pincode is niet verplicht in te vullen
            if (pincode != null)
            {
                //Indien wel ingevuld format controleren
                if (CheckFormat.IsPincodeGeldig(pincode))
                {
                    Pincode = pincode;
                }
            }
        }

        //Tweede ctor dat nu ook de pincode kan laten meegeven
        public TankKaart(string kaartnummer, DateTime vervaldatum, string pincode = null)
            : this(kaartnummer, true, vervaldatum, pincode) { }

        //TankKaart blokkeren 
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

        //Pincode enkel toevoegen wanneer pincode op null staat
        public void VoegPincodeToe(string ingegevenPincode)
        {
            if (String.IsNullOrEmpty(ingegevenPincode))
            {
                throw new TankKaartException("Ingegeven Pincode mag niet null zijn");
            }

            if (!Actief)
                throw new TankKaartException("Kan Pincode niet toevoegen want de TankKaart is niet (meer) actief");

            if (Pincode == null)
            {
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

        //Pincode updaten als pincode is ingevuld of leeg is
        public void UpdatePincode(string ingegevenPincode)
        {
            if (ingegevenPincode == null) {
                throw new TankKaartException($"Ingegeven Pincode mag niet null zijn");
            }

            if (!Actief) 
                throw new TankKaartException($"Kan Pincode niet updaten want de TankKaart is niet (meer) actief");  

            if (Pincode != null)
            {
                if (ingegevenPincode == string.Empty)
                {
                    Pincode = ingegevenPincode; //Pincode mag leeg zijn
                }
                else
                {
                    if (CheckFormat.IsPincodeGeldig(ingegevenPincode))
                    {
                        Pincode = ingegevenPincode;
                    }
                }
            }
            else
            {
                throw new TankKaartException($"Een lege {nameof(Pincode)} kan niet worden geüpdatet");
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

        //Vergelijk twee instanties van TankKaart met: TankKaartNummer & GeldigheidsDatum
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
