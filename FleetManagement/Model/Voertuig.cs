using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Exceptions;

//fout gecorrigeerd in namespace
namespace FleetManagement.Model
{
    public class Voertuig
    {
        public int VoertuigId { get; }
        public AutoModel AutoModel { get; }  //Ingevoegd Filip, dit ontbreekte
        public string ChassisNummer { get; }
        public string NummerPlaat { get; private set; }
        public StatusKleur? Kleur { get; set; } = null;
        public BrandstofType Brandstof { get; }
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
        public Voertuig(AutoModel autoModel, string chassisnummer, string nummerplaat, BrandstofType brandstof, StatusKleur statusKleur)
            :this(autoModel, chassisnummer, nummerplaat, brandstof)
        {
            Kleur = statusKleur;
        }

        public Voertuig(int voertuigId, AutoModel autoModel, string chassisnummer, string nummerplaat, BrandstofType brandstof) 
            : this(autoModel, chassisnummer, nummerplaat, brandstof)
        {
            if(voertuigId > 0)
            {
                this.VoertuigId = voertuigId;
            }
            else
            {
                throw new VoertuigException($"{nameof(VoertuigId)} moet meer zijn dan 0");
            }
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
                Bestuurder.VoegVoertuigToe("connecteren", this);
            }
            else
            {
                throw new VoertuigException($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}");
            }
        }

        //Voeg Bestuurder toe aan Voertuig
        public void VoegBestuurderToe(string actie, Bestuurder ingegevenBestuurder)
        {
            if (ingegevenBestuurder == null)
            {
                throw new VoertuigException($"Ingegeven {nameof(Bestuurder)} mag niet null zijn");
            }

            if (!HeeftVoertuigBestuurder && actie.ToLower() == "connecteren")
            {
                Bestuurder = ingegevenBestuurder;
            }
            else
            {
                throw new VoertuigException($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}");
            }
        }

        //Verwijder Bestuurder met relatie
        public void VerwijderBestuurder(Bestuurder ingegevenBestuurder)
        {
            if (ingegevenBestuurder == null)
            {
                throw new VoertuigException($"Ingegeven {nameof(Bestuurder)} mag niet null zijn");
            }

            if (Bestuurder.Equals(ingegevenBestuurder))
            {
                Bestuurder.VerwijderVoertuig("deconnecteren", this);
                Bestuurder = null;
            }
            else
            {
                throw new VoertuigException($"{nameof(Bestuurder)} kan niet worden verwijderd");
            }
        }

        //Verwijder Bestuurder met relatie
        public void VerwijderBestuurder(string actie, Bestuurder ingegevenBestuurder)
        {
            if (ingegevenBestuurder == null)
            {
                throw new VoertuigException($"Ingegeven {nameof(Bestuurder)} mag niet null zijn");
            }

            if (Bestuurder.Equals(ingegevenBestuurder) && actie.ToLower() == "deconnecteren")
            {
                Bestuurder = null;
            }
            else
            {
                throw new VoertuigException($"{nameof(Bestuurder)} kan niet worden verwijderd");
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

        //Dat heeft property access: ik heb dat ook in de test aangepast
        //=> Dit wordt dan gewoon: Kleur = VoertuigKleur.Blauw
        //public void SetAutoKleur(StatusKleur kleur)
        //{
        //    Kleur = kleur;
        //}

        //De vraag is moet een Voertuig zomaar van Brandstof kunnen veranderen? Ik kan dat in ieder geval niet met mijn auto
        //public void SetBrandstof(BrandstofType brandstof)
        //{
        //    Brandstof = brandstof;
        //}

        //Dat heeft property access: ik heb dat ook in de testen aangepast
        //public void SetAantalDeuren(AantalDeuren deurenaantal)
        //{
        //    AantalDeuren = deurenaantal;
        //}

        //Vergelijk twee instanties van Voertuig met: ChassisNummer
        public override bool Equals(object obj)
        {
            if (obj is Voertuig)
            {
                Voertuig ander = obj as Voertuig;
                return ChassisNummer == ander.ChassisNummer && NummerPlaat == ander.NummerPlaat;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ChassisNummer.GetHashCode() ^ NummerPlaat.GetHashCode();
        }
    }
}

