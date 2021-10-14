using FleetManagement.CheckFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Exceptions;

//Nog bezig met het implementeren

namespace FleetManagement.Models 
{
    public class Bestuurder
    {
        public int BestuurderId { get; } 

        public string Voornaam { get; set; }

        public string Achternaam { get; set; } 

        public DateTime GeboorteDatum { get; }

        public Adres Adres { get; set; } = null; 

        //Besloten string te maken omwille van meerdere mogelijkheden voor één RijbewijsNummer: B, C, D1+E, enz. 
        //Combinatie kan van bestuurder tot bestuurder variëren
        public string TypeRijbewijs { get; set; } 

        public string RijBewijsNummer { get; } 

        public string RijksRegisterNummer { get; } 

        //Vraag: Moeten we voertuig kunnen veranderen eenmaal een instantie is aangemaakt? Ja of neen, en kort waarom je dat vindt.
        //Antw: Filip: 
        //Antw Ali: 
        //Antw Amhet: 
        public Voertuig Voertuig { get; private set; }  

        public TankKaart TankKaart { get; private set; }

        public Bestuurder(string voornaam, string achternaam, DateTime geboorteDatum, string typeRijbewijs,
            string rijBewijsNummer, string rijksRegisterNummer)
        {
            if (CheckFormat.IsRijksRegisterGeldig(rijksRegisterNummer, geboorteDatum)) {

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
       
        public Bestuurder(int bestuurderId, string voornaam, string achternaam, DateTime geboorteDatum, string typeRijbewijs,
            string rijBewijsNummer, string rijksRegisterNummer) : this(voornaam, achternaam, geboorteDatum, 
                typeRijbewijs, rijBewijsNummer, rijksRegisterNummer)
        {
            BestuurderId = bestuurderId;
        }

        public virtual void VoertuigToevoegen(Voertuig ingegevenVoertuig)
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

        public virtual void VoertuigVerwijderen(Voertuig ingegevenVoertuig)
        {
            if(ingegevenVoertuig == null)
            {
                throw new BestuurderException($"Ingegeven {nameof(Voertuig)} mag niet null zijn.");
            }

            if (Voertuig != null)
            {

            }

            throw new BestuurderException($"Er is geen {nameof(Voertuig)} om te verwijderen");
        }

        public virtual void TankKaartToevoegen(TankKaart ingegevenTankKaart)
        {
            if(ingegevenTankKaart == null)
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

        public virtual bool TankKaartVerwijderen(TankKaart ingegevenTankKaart)
        {
            if(TankKaart != null)
            {
                if (TankKaart.Equals(ingegevenTankKaart)) { //Ali: overriden van Equals TankKaart met BankKaartNummer en GeldegheidsDatum, 
                    TankKaart = null;
                }

                throw new BestuurderException($"{nameof(TankKaart)} verwijderen is mislukt omdat TankKaartnummer niet overeenkomt");
            }

            throw new BestuurderException($"Er is geen {nameof(TankKaart)} om te verwijderen");
        }

        //Vergelijk twee instanties van Bestuurder met: ID, rijksRegisterNummer
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