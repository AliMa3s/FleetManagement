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

        //Nieuwe of bestaande voertuig toevoegen
        public virtual void VoegVoertuigToe(Voertuig ingegevenVoertuig)
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

        //Voertuig verwijder maar ID & ChassisNummer moet overeenkomen
        public virtual void VerwijderVoertuig(Voertuig ingegevenVoertuig)
        {
            if (ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn.");
            }

            if (Voertuig == null)
            {
                if (Voertuig.Equals(ingegevenVoertuig))
                {
                    Voertuig = ingegevenVoertuig; //Override Equals met ChassisNummer & VoertuigId
                }
            }
            else
            {
                throw new BestuurderException($"Er is geen {nameof(Voertuig)} om te verwijderen");
            }
        }

        //Nieuwe of bestaande TankKaart toevoegen
        public virtual void VoegTankKaartToe(TankKaart ingegevenTankKaart)
        {
            if (ingegevenTankKaart == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(ingegevenTankKaart)} mag niet null zijn.");
            }

            if (TankKaart == null)
            {
                TankKaart = ingegevenTankKaart;
            }
            else
            {
                throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(TankKaart)}");
            }
        }

        //TankKaart verwijderen maar BankKaartNummer & GeligheidsDatum moet overeenkomen
        public virtual bool VerwijderTankKaart(TankKaart ingegevenTankKaart)
        {
            if (TankKaart != null)
            {
                if (TankKaart.Equals(ingegevenTankKaart))
                { //Ali: overriden van Equals TankKaart met BankKaartNummer en GeldegheidsDatum, 
                    TankKaart = null;
                }

                throw new BestuurderException($"{nameof(TankKaart)} verwijderen is mislukt omdat TankKaartnummer niet overeenkomt");
            }

            throw new BestuurderException($"Er is geen {nameof(TankKaart)} om te verwijderen");
        }

        //Vergelijk twee instanties van Bestuurder met: ID & rijksRegisterNummer
        public override bool Equals(object obj)
        {
            if (obj is Bestuurder)
            {
                Bestuurder ander = obj as Bestuurder;
                return BestuurderId == ander.BestuurderId && RijksRegisterNummer == ander.RijksRegisterNummer;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return BestuurderId.GetHashCode() ^ RijksRegisterNummer.GetHashCode();
        }
    }
}