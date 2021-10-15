using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Exceptions;
using FleetManagement.Model;

//fout gecorrigeerd in namespace
namespace FleetManagement.Model
{
    public class Voertuig
    {
        public int VoertuigId { get; }
        public string ChassisNummer { get; } //voertuig kan niet veranderen tenzij je de nummer uitvijlt ;) 
        public string NummerPlaat { get; private set; }
        public StatusKleur? Kleur { get; set; } = null;
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

        public void BestuurderToevoegen(Bestuurder bestuurder)
        {
            if(bestuurder == null)
            {
                throw new VoertuigException($"Ingegeven argument {nameof(Bestuurder)} mag niet null zijn");
            }

            if(Bestuurder == null)
            {
                Bestuurder = bestuurder;
            }
            else
            {
                throw new BestuurderException($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}");
            }
        }

        public void UpdateNummerplaat(string nummerplaat)
        {
            if (nummerplaat.Length < 0) throw new VoertuigException("voertuig - nummerplaat niet gevonden.");
            NummerPlaat = nummerplaat;
        }

        public void SetAutoKleur(StatusKleur kleur)
        {
            Kleur = kleur;
        }
        public void SetBrandstof(BrandstofType brandstof)
        {
            Brandstof = brandstof;
        }

        public void SetAantalDeuren(AantalDeuren deurenaantal)
        {
            AantalDeuren = deurenaantal;
        }
    }
}

