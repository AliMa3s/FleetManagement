using FleetManagement.Interfaces;
using FleetManagement.CheckFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Exceptions;

//bezig met de class, enums moeten ook nog aangemaakt worden
namespace FleetManagement.Models
{
    class Bestuurder : IBestuurder
    {
        public int BestuurderId { get; } 

        public string Voornaam { get; set; }

        public string Achternaam { get; set; } 

        public DateTime GeboorteDatum { get; }

        public Adres? Adres { get; set; } = null; 

        public string TypeRijbewijs { get; set; } 

        public int StatusBestuurder { get; private set; } = 0; 

        public string RijBewijsNummer { get; } 

        public string RijksRegisterNummer { get; } 

        public Voertuig? Voertuig { get; private set; } = null;  

        public TankKaart? TankKaart { get; private set; } = null;

        public Bestuurder(string voornaam, string achternaam,  DateTime geboorteDatum, string typeRijbewijs,
            string rijBewijsNummer, string rijksRegisterNummer)
        {
            if (!CheckFormat.IsRijksRegisterNumberGeldig(rijksRegisterNummer, geboorteDatum)) {
                throw new BestuurderException("RijksregisterNummer voldoet niet aan het juiste formaat");
            }
            else
            {
                RijksRegisterNummer = rijksRegisterNummer;
            }

            //Rijbewijs nog checken

            Voornaam = voornaam;
            Achternaam = achternaam;
            GeboorteDatum = geboorteDatum;
            TypeRijbewijs = typeRijbewijs;
        }

        public Bestuurder(int bestuurderId, string voornaam, string achternaam, DateTime geboorteDatum, string typeRijbewijs,
            string rijBewijsNummer, string rijksRegisterNummer) : this(voornaam, achternaam, geboorteDatum, 
                typeRijbewijs, rijBewijsNummer, rijksRegisterNummer)
        {
            BestuurderId = bestuurderId;
        }

        public bool IsRijksRegisterGeldig(string rijksregister, DateTime geboorteDatum)
        {
            throw new BestuurderException("Moet nog worden geimplementeerd");  //weet nog niet. Kan doorgeven naar Checkformat
        }

        public void TankKaartToevoegen(TankKaart tankKaart)
        {
            throw new BestuurderException("Moet nog worden geimplementeerd");
        }

        public bool TankKaartVerwijderen(TankKaart tankKaart)
        {
            throw new BestuurderException("Moet nog worden geimplementeerd");
        }

        public void VoertuigToevoegen(Voertuig voertuig)
        {
            throw new BestuurderException("Moet nog worden geimplementeerd");
        }

        public void VoertuigVerwijderen(Voertuig voertuig)
        {
            throw new BestuurderException("Moet nog worden geimplementeerd");
        }
    }
}
