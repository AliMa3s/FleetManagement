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
        public bool HeeftVoertuigBestuurder => Bestuurder != null;

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
        //ok?
        public Voertuig(AutoModel autoModel, string chassisnummer, string nummerplaat, BrandstofType brandstof, StatusKleur statusKleur):this(autoModel, chassisnummer, nummerplaat, brandstof)
        {
            Kleur = statusKleur;
        }

        public Voertuig(int voertuigId, AutoModel autoModel, string chassisnummer, string nummerplaat, BrandstofType brandstof) 
            : this(autoModel, chassisnummer, nummerplaat, brandstof)
        {
            this.VoertuigId = voertuigId;
        }

        //Voeg Bestuurder toe aan Voertuig
        public void VoegBestuurderToe(Bestuurder ingegevenBestuurder)
        {
            if(ingegevenBestuurder == null)
            {
                throw new VoertuigException($"Ingegeven {nameof(Bestuurder)} mag niet null zijn");
            }

            if (!HeeftVoertuigBestuurder)
            {
                Bestuurder = ingegevenBestuurder;
                Bestuurder.GeefVoertuig(this);
            }
            else
            {
                throw new BestuurderException($"voertuig heeft al een bestuurder");
            }
        }

        //Geef Voertuig een bestuurder
        public void GeefBestuurder(Bestuurder ingegevenBestuurder)
        {
            if (ingegevenBestuurder == null)
            {
                throw new VoertuigException($"Ingegeven {nameof(Bestuurder)} mag niet null zijn");
            }

            if (!HeeftVoertuigBestuurder)
            {
                Bestuurder = ingegevenBestuurder;
            }
            else
            {
                throw new BestuurderException($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}");
            }
        }

        //VerwijderBestuurder nog invoegen
        public void VerwijderBestuurder(Bestuurder ingegevenBestuurder)
        {
            if (ingegevenBestuurder != null)
            {
                if (Bestuurder.Equals(ingegevenBestuurder))
                {
                    Bestuurder.VerwijderVoertuig(this);
                    Bestuurder = null;
                }
                else
                {
                    throw new BestuurderException($"{nameof(Bestuurder)} kan niet verwijderd worden");
                }
            }
            else
            {
                throw new BestuurderException($"Er is geen {nameof(Bestuurder)} om te verwijderen");
            }
        }
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

