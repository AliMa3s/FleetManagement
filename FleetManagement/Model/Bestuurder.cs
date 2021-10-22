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
        public int BestuurderId { get; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string GeboorteDatum { get; }
        public Adres Adres { get; set; } = null;
        public string TypeRijbewijs { get; set; }
        public string RijBewijsNummer { get; }
        public string RijksRegisterNummer { get; }
        public Voertuig Voertuig { get; private set; } = null;
        public TankKaart TankKaart { get; private set; } = null;
        public DateTime AanMaakDatum { get; }
        public bool HeeftBestuurderVoertuig => Voertuig != null;
        public bool HeeftBestuurderTankKaart => TankKaart != null;

        //Nieuw Bestuurder: Enkel verplichte velden
        public Bestuurder(string voornaam, string achternaam, string geboorteDatum, string typeRijbewijs,
            string rijBewijsNummer, string rijksRegisterNummer)
        {
            if (CheckFormat.IsRijksRegisterGeldig(rijksRegisterNummer, geboorteDatum))
            {
                GeboorteDatum = geboorteDatum;
                RijksRegisterNummer = rijksRegisterNummer;
            }

            if (CheckFormat.IsRijbewijsNummerGeldig(rijBewijsNummer))
            {
                TypeRijbewijs = typeRijbewijs;
                RijBewijsNummer = rijBewijsNummer;
            }

            Voornaam = voornaam;
            Achternaam = achternaam;
        }

        //Bestaande Bestuurder: ID met verplichte velden
        public Bestuurder(int bestuurderId, string voornaam, string achternaam, string geboorteDatum, string typeRijbewijs,
            string rijBewijsNummer, string rijksRegisterNummer) : this(voornaam, achternaam, geboorteDatum,
                typeRijbewijs, rijBewijsNummer, rijksRegisterNummer)
        {
            BestuurderId = bestuurderId;
        }

        //bellen
        public void VoegVoertuigToe(Voertuig ingegevenVoertuig)
        {
            if (ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn");
            }

            if (Voertuig == null)
            {
                Voertuig = ingegevenVoertuig;
                Voertuig.KrijgBestuurder(this);
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(Voertuig)}");
            }
        }

        //opnemen
        public void KrijgVoertuig(Voertuig ingegevenVoertuig)
        {
            if (ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn");
            }

            if (Voertuig == null)
            {
                Voertuig = ingegevenVoertuig;
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(Voertuig)}");
            }
        }

        //bellen
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
                    Voertuig.VerwijderBestuurder(this);
                    Voertuig = null; //Override Equals met ChassisNummer & VoertuigId
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

        //opnemen
        public void WisVoertuig(Voertuig ingegevenVoertuig)
        {
            if (ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn.");
            }

            if (HeeftBestuurderVoertuig)
            {
                if (Voertuig.Equals(ingegevenVoertuig))
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
                throw new BestuurderException($"Er is geen {nameof(Voertuig)} om te verwijderen"); //nog invoegen bij Tank & Voertuig
            }
        }

        //Voegt TankKaart toe naar relatie
        public virtual void VoegTankKaartToe(TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(TankKaart)} mag niet null zijn.");
            }

            if (!HeeftBestuurderTankKaart)
            {
                TankKaart = ingegevenTankKaart;
                TankKaart.KrijgBestuurder(this); //Plaatst Bestuurder
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(TankKaart)}");
            }
        }

        //Krijgt TankKaart van relatie (Geen verwijzing terug)
        public void KrijgTankKaart(TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(TankKaart)} mag niet null zijn");
            }

            if (!HeeftBestuurderTankKaart)
            {
                TankKaart = ingegevenTankKaart;
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(TankKaart)}");
            }
        }

        //TankKaart verwijderen maar BankKaartNummer & GeligheidsDatum moeten overeenkomen
        //bellen
        public virtual void VerwijderTankKaart(TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"{nameof(TankKaart)} mag niet null zijn");
            }

            if (HeeftBestuurderTankKaart)
            {
                if (TankKaart.Equals(ingegevenTankKaart))
                { //Ali: overriden van Equals TankKaart met BankKaartNummer en GeldegheidsDatum, 
                    TankKaart.VerwijderBestuurder(this);
                    TankKaart = null;
                }
                else
                {
                    throw new BestuurderException($"{nameof(TankKaart)} kan niet verwijderd worden");
                }
            }
            else
            {
                throw new BestuurderException($"Er is geen {nameof(TankKaart)} om te verwijderen");
            }
        }

        //opnemen
        public virtual void WisTankKaart(TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"{nameof(TankKaart)} mag niet null zijn");
            }

            if (HeeftBestuurderTankKaart)
            {
                if (TankKaart.Equals(ingegevenTankKaart))
                {
                    TankKaart = null;
                }
                else
                {
                    throw new BestuurderException($"{nameof(TankKaart)} kan niet gewist worden");
                }
            }
            else
            {
                throw new BestuurderException($"Er is geen {nameof(TankKaart)} om te wissen");
            }
        }

        //Vergelijk twee instanties van Bestuurder met: rijksRegisterNummer
        public override bool Equals(object obj)
        {
            if (obj is Bestuurder)
            {
                Bestuurder ander = obj as Bestuurder;
                return RijksRegisterNummer == ander.RijksRegisterNummer;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return RijksRegisterNummer.GetHashCode();
        }
    }
}