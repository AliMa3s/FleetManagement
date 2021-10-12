using FleetManagement.Interfaces;
using FleetManagement.CheckFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Exceptions;

//Ik zou persoonlijk niet "Models" nemen maar eerder business(laag). Models zijn volgens mij objecten in MVC presentatielaag. 
//Nog bezig met het implenteren

namespace FleetManagement.Models 
{
    public class Bestuurder : IBestuurder
    {
        public int BestuurderId { get; } 

        public string Voornaam { get; set; }

        public string Achternaam { get; set; } 

        public DateTime GeboorteDatum { get; }

        public Adres Adres { get; set; } = null; 

        //Besloten string te maken omwille van meerdere mogelijkheden voor één RijbewijsNummer: B, C, D1+E, enz. 
        //Combinatie kan van bestuurder tot bestuurder variëren
        public string TypeRijbewijs { get; set; } 

        public StatusBestuurder StatusBestuurder { get; private set; } = StatusBestuurder.Beschikbaar;

        public string RijBewijsNummer { get; } 

        public string RijksRegisterNummer { get; } 

        public Voertuig Voertuig { get; private set; }  

        public TankKaart TankKaart { get; private set; }

        public Bestuurder(string voornaam, string achternaam,  DateTime geboorteDatum, string typeRijbewijs,
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

        public void TankKaartToevoegen(TankKaart tankKaart)
        {
            throw new BestuurderException("Moet nog worden geimplementeerd");
        }

        public bool TankKaartVerwijderen(TankKaart tankKaart)
        {
            throw new BestuurderException("Moet nog worden geimplementeerd");
        }

        public void VoertuigToevoegen(Voertuig voertuig, StatusBestuurder statusBestuurder)
        {
            if(Voertuig == null 
                && statusBestuurder != StatusBestuurder.Beschikbaar)
            {
                //Voertuig Toevoegen in Bestuurder
                Voertuig = voertuig;
                StatusBestuurder = statusBestuurder; //ingegeven arg van statusBestuurder mag nooit status beschikbaar zijn

                //Bestuurder in voertuig toevoegen
                voertuig.BestuurderToevoegen(this, StatusVoertuig.Bezet);
            }

            if(statusBestuurder == StatusBestuurder.Beschikbaar)
            {
                throw new BestuurderException($"Er wordt een {nameof(Voertuig)} toegevoegd en status wordt op beschikbaar ingesteld");
            } 

            throw new BestuurderException($"{nameof(Bestuurder)} heeft al een {nameof(Voertuig)}");
        }

        public void VoertuigVerwijderen(Voertuig voertuig, StatusBestuurder statusBestuurder)
        {
            throw new BestuurderException("Moet nog worden geimplementeerd");
        }

        //Vergelijk twee instanties van Bestuurder met: ID, Voornaam en Achternaam. 
        //Id wel of niet? 
        public override bool Equals(object obj)
        {
            if (obj is Bestuurder)
            {
                Bestuurder ander = obj as Bestuurder;
                return BestuurderId == ander.BestuurderId 
                    && Voornaam == ander.Voornaam 
                    && Achternaam == ander.Achternaam;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return BestuurderId.GetHashCode() ^ Voornaam.GetHashCode() + Achternaam.GetHashCode();
        }
    }
}