using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Interfaces;
using FleetManagement.Exceptions;
using FleetManagement.Models;

//fout gecorrigeerd in namespace
namespace FleetManagement.Models
{
    public class Voertuig : IVoertuig
    {
        public int VoertuigId { get; }
        public string ChassisNummer { get; private set; }
        public string NummerPlaat { get; private set; }
        public StatusVoertuig StatusVoertuig { get; private set; }
        public StatusKleur Kleur { get; set; }
        public BrandstofType Brandstof { get; set; }
        public DateTime InBoekDatum { get; set; }
        public AantalDeuren? AantalDeuren { get; set; } = null; //int?=> nullable waarde
        public Bestuurder Bestuurder { get; private set; }


        public Voertuig()
        {

        }
        public Voertuig(int voertuigId, string chassisnummer, string nummerplaat, StatusKleur kleur, BrandstofType brandstof, DateTime inboekdatum, AantalDeuren aantaldeuren)
        {
            this.VoertuigId = voertuigId;
            this.ChassisNummer = chassisnummer;
            this.NummerPlaat = nummerplaat;
            Kleur = kleur;
            this.Brandstof = brandstof;
            this.InBoekDatum = inboekdatum;
            this.AantalDeuren = aantaldeuren;
        }

        public void GetVoertuigID(Voertuig voertuig)
        {
            if (voertuig.VoertuigId < 0) throw new VoertuigException("voertuig - VoertuigId niet  gevonden.");

            
        }

        public void VoertuigIsBezet()
        {
            StatusVoertuig = StatusVoertuig.Bezet;
        }

        public void VoertuigKomtVrij()
        {
            //Mss refectoring met meegegeven voertuig om te vergelijken alvorens wordt verwijderd
            Bestuurder = null;
            StatusVoertuig = StatusVoertuig.Beschikbaar;
        }

        public void BestuurderToevoegen(Bestuurder bestuurder)
        {
            if(bestuurder == null)
            {
                throw new VoertuigException($"Ingegeven argument {nameof(Bestuurder)} mag niet null zijn");
            }

            if(Bestuurder == null)
            {
                Bestuurder = bestuurder;
                Bestuurder.BestuurderIsBezet();
            }

            throw new BestuurderException($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}");
        }

        public void GetChassisNummer(string chassisnummer)
        {
            return ChassisNummer = chassisnummer;
        }

        public void UpdateNummerplaat(string nummerplaat)
        {
            if (nummerplaat.Length < 0) throw new VoertuigException("voertuig - nummerplaat niet gevonden.");
            NummerPlaat = nummerplaat;
        }

        public void SetAutoKleur(StatusKleur kleur)
        {
            if (kleur is null)
            {
                throw new ArgumentNullException(nameof(kleur));
            }
            Kleur = kleur;
        }
        public void SetBrandstof(BrandstofType brandstof)
        {
            Brandstof = brandstof;
        }

        public void Getinboekdatum(DateTime inboekdatum)
        {
            return inboekdatum;
        }

        public void SetAantalDeuren(int deurenaantal)
        {
            AantalDeuren = deurenaantal;
        }
        public void GetAantalDeuren(int deurenaantal)
        {
            return deurenaantal;
        }

        //deze method is zeker fout moet nog aangepast worden.
        public void GetBestuurder(Bestuurder bestuurder)
        {
            if (!bestuurder.BestuurderId) throw new VoertuigException("voertuig - geen bestuurder gevonden.");
        }
    }
}

