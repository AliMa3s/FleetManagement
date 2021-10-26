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
        #region Properties
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
        #endregion

        #region ctors
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
        #endregion

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

            if(BestuurderId < 1)
            {
                throw new BestuurderException($"De {nameof(Bestuurder)} is niet geslecteerd uit lijst bestuurders");
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
                throw new BestuurderException($"Ingegeven {nameof(TankKaart)} mag niet null zijn.");
            }

            if (!HeeftBestuurderTankKaart)
            {
                TankKaart = ingegevenTankKaart;
                TankKaart.VoegBestuurderToe(BestuurderId, this); //Plaatst Bestuurder
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(TankKaart)}");
            }
        }

        //Vangt de relatie op en plaatst de entiteit
        public virtual void VoegTankKaartToe(string tankKaartNummer,TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(TankKaart)} mag niet null zijn.");
            }

            if(tankKaartNummer == null)
            {
                throw new BestuurderException($"De {nameof(TankKaart)} is niet geslecteerd uit lijst tankkaarten");
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

        //Maakt de relatie en verwijdert de entiteit
        public virtual void VerwijderTankKaart(TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"{nameof(TankKaart)} mag niet null zijn");
            }

            if (HeeftBestuurderTankKaart)
            {
                if (TankKaart.Equals(ingegevenTankKaart))
                {
                    TankKaart.VerwijderBestuurder(BestuurderId, this);
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

        //Vangt de relatie op en verwijdert entiteit
        public virtual void VerwijderTankKaart(string tankKaartNummer, TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"{nameof(TankKaart)} mag niet null zijn");
            }

            if (HeeftBestuurderTankKaart)
            {
                if (TankKaart.Equals(ingegevenTankKaart) && tankKaartNummer != null)
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
        #endregion

        #region overridables
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