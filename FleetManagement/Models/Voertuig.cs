using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Interfaces;
using FleetManagement.Exceptions;
using FleetManagement.Models;
namespace.fleetmanagement.Models
{
    public class Voertuig : IVoertuig, Bestuurder
    {
        int VoertuigId { get; }
        string ChassisNummer { get; private set; }
        string NummerPlaat { get; private set; }
        Kleur _kleur { get; set; }
        BrandstofType Brandstof { get; set; }
        DateTime InBoekDatum { get; set; }
        int? AantalDeuren { get; set; } //int?=> nullable waarde



        public Voertuig()
        {

        }
        public Voertuig(int voertuigId, string chassisnummer, string nummerplaat, Kleur kleur, BrandstofType brandstof, DateTime inboekdatum, int aantaldeuren)
        {
            this.VoertuigId = voertuigId;
            this.ChassisNummer = chassisnummer;
            this.NummerPlaat = nummerplaat;
            this.kleur = kleur;
            this.Brandstof = brandstof;
            this.InBoekDatum = inboekdatum;
            this.AantalDeuren = aantaldeuren;
        }

        public void GetVoertuigID(Voertuig voertuigID)
        {
            if (Voertuigid < 0) throw new VoertuigException("voertuig - VoertuigId niet  gevonden.");
            return voertuigID;

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

        public void SetAutoKleur(Kleur kleur)
        {
            if (kleur is null)
            {
                throw new ArgumentNullException(nameof(kleur));
            }
            _kleur = kleur;
        }
        public void SetBrandstof(BrandStofType brandstof)
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

