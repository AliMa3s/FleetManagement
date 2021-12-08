using FleetManagement.CheckFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Exceptions;

//Nog bezig met het implementeren

namespace FleetManagement.Model
{
    public class Bestuurder
    {
        #region Properties
        public int BestuurderId { get; private set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string GeboorteDatum { get; }
        public Adres Adres { get; set; }
        public string TypeRijbewijs { get; set; }
        public string RijksRegisterNummer { get; }
        public Voertuig Voertuig { get; private set; }
        public TankKaart Tankkaart { get; private set; }
        public DateTime? AanmaakDatum { get; set; }
        public bool HeeftBestuurderVoertuig => Voertuig != null;
        public bool HeeftBestuurderTankKaart => Tankkaart != null;
        #endregion

        //Mag hier een property komen om een date format te geven ToString("dd MMMM yyyy")? 

        #region Ctors
        //Nieuw Bestuurder: Enkel verplichte velden
        public Bestuurder(string voornaam, string achternaam, string geboorteDatum, string typeRijbewijs, string rijksRegisterNummer)
        {

            

            if(string.IsNullOrWhiteSpace(voornaam)) throw new BestuurderException($"{nameof(Voornaam)} moet ingevuld zijn");
            if (string.IsNullOrWhiteSpace(achternaam)) throw new BestuurderException($"{nameof(Achternaam)} moet ingevuld zijn");

            Voornaam = voornaam;
            Achternaam = achternaam;

            if (CheckFormat.IsRijksRegisterGeldig(rijksRegisterNummer, geboorteDatum))
            {
                GeboorteDatum = geboorteDatum;
                RijksRegisterNummer = rijksRegisterNummer;
            }

            if (string.IsNullOrWhiteSpace(typeRijbewijs)) throw new BestuurderException("Type rijbewijs moet ingevuld zijn");

            TypeRijbewijs = typeRijbewijs;
        }

        //Bestaande Bestuurder: ID met verplichte velden
        public Bestuurder(int bestuurderId, string voornaam, string achternaam, string geboorteDatum, string typeRijbewijs,
            string rijksRegisterNummer) : this(voornaam, achternaam, geboorteDatum, typeRijbewijs, rijksRegisterNummer)
        {
            VoegIdToe(bestuurderId);
        }
        #endregion

        public void VoegIdToe(int bestuurderId)
        {
            if (bestuurderId > 0)
            {
                if(BestuurderId == 0)
                {
                    BestuurderId = bestuurderId;
                }
                else
                {
                    throw new BestuurderException($"{nameof(BestuurderId)} is al aanwezig en kan niet gewijzigd worden");
                }
            }
            else
            {
                throw new BestuurderException($"{nameof(BestuurderId)} moet meer zijn dan 0");
            }
        }

        #region Voertuig
        //Maakt de relatie en plaatst de entiteit
        public virtual void VoegVoertuigToe(Voertuig ingegevenVoertuig)
        {
            if (ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn");
            }

            if (!HeeftBestuurderVoertuig)
            {
                Voertuig = ingegevenVoertuig;
                Voertuig.VoegBestuurderToe(BestuurderId, this);
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(Voertuig)}");
            }
        }

        //Vangt de relatie op en plaatst de entiteit
        public virtual void VoegVoertuigToe(int voertuigId, Voertuig ingegevenVoertuig)
        {
            if (ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn");
            }

            //Voertuig hoeft niet geslecteerd te zijn
            if (!HeeftBestuurderVoertuig && voertuigId >= 0)
            {
                Voertuig = ingegevenVoertuig;
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(Voertuig)}");
            }
        }

        //Maakt de relatie en verwijdert de entiteit
        public virtual void VerwijderVoertuig(Voertuig ingegevenVoertuig)
        {
            if (ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn.");
            }

            if (HeeftBestuurderVoertuig)
            {
                if (Voertuig.Equals(ingegevenVoertuig))
                {
                    Voertuig.VerwijderBestuurder(BestuurderId, this);
                    Voertuig = null; 
                }
                else
                {
                    throw new BestuurderException($"{nameof(Voertuig)} kan niet verwijderd worden");
                }
            }
            else
            {
                throw new BestuurderException($"Er is geen {nameof(Voertuig)} om te verwijderen");
            }
        }

        //Vangt de relatie op en plaatst de entiteit
        public virtual void VerwijderVoertuig(int voertuigId, Voertuig ingegevenVoertuig)
        {
            if (ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn.");
            }

            if (HeeftBestuurderVoertuig)
            {
                if (Voertuig.Equals(ingegevenVoertuig) && voertuigId >= 0)
                {
                    Voertuig = null;
                }
                else
                {
                    throw new BestuurderException($"{nameof(Voertuig)} kan niet verwijderd worden");
                }
            }
            else
            {
                throw new BestuurderException($"Er is geen {nameof(Voertuig)} om te verwijderen");
            }
        }
        #endregion

        #region Tankkaart
        //Maakt de relatie en plaatst de entiteit
        public virtual void VoegTankKaartToe(TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Tankkaart)} mag niet null zijn.");
            }

            if (!HeeftBestuurderTankKaart)
            {
                Tankkaart = ingegevenTankKaart;
                Tankkaart.VoegBestuurderToe(BestuurderId, this); //Plaatst Bestuurder
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(Tankkaart)}");
            }
        }

        //Vangt de relatie op en plaatst de entiteit
        public virtual void VoegTankKaartToe(string tankKaartNummer,TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Tankkaart)} mag niet null zijn.");
            }

            if (!HeeftBestuurderTankKaart)
            {
                Tankkaart = ingegevenTankKaart;
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(Tankkaart)}");
            }
        }

        //Maakt de relatie en verwijdert de entiteit
        public virtual void VerwijderTankKaart(TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"{nameof(Tankkaart)} mag niet null zijn");
            }

            if (HeeftBestuurderTankKaart)
            {
                if (Tankkaart.Equals(ingegevenTankKaart))
                {
                    Tankkaart.VerwijderBestuurder(BestuurderId, this);
                    Tankkaart = null;
                }
                else
                {
                    throw new BestuurderException($"{nameof(Tankkaart)} kan niet verwijderd worden");
                }
            }
            else
            {
                throw new BestuurderException($"Er is geen {nameof(Tankkaart)} om te verwijderen");
            }
        }

        //Vangt de relatie op en verwijdert entiteit
        public virtual void VerwijderTankKaart(string tankKaartNummer, TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"{nameof(Tankkaart)} mag niet null zijn");
            }

            if (HeeftBestuurderTankKaart)
            {
                if (Tankkaart.Equals(ingegevenTankKaart) && tankKaartNummer != null)
                { 
                    Tankkaart = null;
                }
                else
                {
                    throw new BestuurderException($"{nameof(Tankkaart)} kan niet verwijderd worden");
                }
            }
            else
            {
                throw new BestuurderException($"Er is geen {nameof(Tankkaart)} om te verwijderen");
            }
        }
        #endregion

        #region Overridables
        //Vergelijk twee instanties van Bestuurder met: rijksRegisterNummer
        public override bool Equals(object obj)
        {
            if (obj is Bestuurder)
            {
                Bestuurder ander = obj as Bestuurder;
                return RijksRegisterNummer == ander.RijksRegisterNummer
                    && GeboorteDatum == ander.GeboorteDatum;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return RijksRegisterNummer.GetHashCode() ^ GeboorteDatum.GetHashCode();
        }
        #endregion
    }
}