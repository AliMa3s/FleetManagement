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
        public AutoModel AutoModel { get; set; }  //Ingevoegd Filip, dit ontbreekte
        public string ChassisNummer { get; }
        public string NummerPlaat { get; private set; }
        public StatusKleur? Kleur { get; set; } = null;
        public BrandstofType Brandstof { get; set; }
        public DateTime InBoekDatum { get; set; }
        public AantalDeuren? AantalDeuren { get; set; } = null;
        public Bestuurder Bestuurder { get; private set; }

        //Static checks ingevoegd
        public Voertuig(AutoModel autoModel, string chassisnummer, string nummerplaat, BrandstofType brandstof)
        {
            if(CheckFormats.CheckFormat.IsChassisNummerGeldig(chassisnummer))
            {
                this.ChassisNummer = chassisnummer;
            }

            if (CheckFormats.CheckFormat.IsNummerplaatGeldig(nummerplaat))
            {
                this.NummerPlaat = nummerplaat;
            }

            this.AutoModel = autoModel;
            this.Brandstof = brandstof;
        }

        public Voertuig(int voertuigId, AutoModel autoModel, string chassisnummer, string nummerplaat, BrandstofType brandstof) 
            : this(autoModel, chassisnummer, nummerplaat, brandstof)
        {
            this.VoertuigId = voertuigId;
        }

        public void VoegBestuurderToe(Bestuurder bestuurder)
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

        //VerwijderBestuurder nog invoegen

        //Static check ingevoegd
        public void UpdateNummerplaat(string nummerplaat)
        {
           if(CheckFormats.CheckFormat.IsNummerplaatGeldig(nummerplaat))
            {
                NummerPlaat = nummerplaat;
            }
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

