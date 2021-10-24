﻿using FleetManagement.CheckFormats;
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
            if (bestuurderId > 0)
            {
                BestuurderId = bestuurderId;
            }
            else
            {
                throw new BestuurderException($"{nameof(BestuurderId)} moet meer zijn dan 0");
            }
        }

        //Voegt Voertuig toe en vraagt aan het voertuig de Bestuurder te connecteren
        public virtual void VoegVoertuigToe(Voertuig ingegevenVoertuig)
        {
            if (ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn");
            }

            if (!HeeftBestuurderVoertuig)
            {
                Voertuig = ingegevenVoertuig;
                Voertuig.VoegBestuurderToe("connecteren", this);
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(Voertuig)}");
            }
        }

        public virtual void VoegVoertuigToe(string actie, Voertuig ingegevenVoertuig)
        {
            if (ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn");
            }

            if (!HeeftBestuurderVoertuig && actie.ToLower() == "connecteren")
            {
                Voertuig = ingegevenVoertuig;
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(Voertuig)}");
            }
        }

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
                    Voertuig.VerwijderBestuurder("deconnecteren", this);
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

        //
        public virtual void VerwijderVoertuig(string actie, Voertuig ingegevenVoertuig)
        {
            if (ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn.");
            }

            if (HeeftBestuurderVoertuig)
            {
                if (Voertuig.Equals(ingegevenVoertuig) && actie.ToLower() == "deconnecteren")
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

        //Voegt TankKaart toe en vraagt aan de TankKaart de Bestuurder te connecteren
        public virtual void VoegTankKaartToe(TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(TankKaart)} mag niet null zijn.");
            }

            if (!HeeftBestuurderTankKaart)
            {
                TankKaart = ingegevenTankKaart;
                TankKaart.VoegBestuurderToe("connecteren", this); //Plaatst Bestuurder
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(TankKaart)}");
            }
        }

        public virtual void VoegTankKaartToe(string actie,TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(TankKaart)} mag niet null zijn.");
            }

            if (!HeeftBestuurderTankKaart && actie.ToLower() == "connecteren")
            {
                TankKaart = ingegevenTankKaart;
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(TankKaart)}");
            }
        }

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
                    TankKaart.VerwijderBestuurder("deconnecteren", this);
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

        public virtual void VerwijderTankKaart(string actie, TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"{nameof(TankKaart)} mag niet null zijn");
            }

            if (HeeftBestuurderTankKaart)
            {
                if (TankKaart.Equals(ingegevenTankKaart) && actie.ToLower() == "deconnecteren")
                { 
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
    }
}