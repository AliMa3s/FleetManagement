using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.CheckFormats;
using FleetManagement.Exceptions;

//fout gecorrigeerd in namespace
namespace FleetManagement.Model
{
    public class Voertuig
    {
        //Zone properties
        public int VoertuigId { get; private set; }
        public AutoModel AutoModel { get; set; }
        public string ChassisNummer { get; }
        public string NummerPlaat { get; private set; }
        public BrandstofVoertuig Brandstof { get; set; }
        public Kleur VoertuigKleur { get; set; }
        public DateTime? InBoekDatum { get; set; }
        public AantalDeuren? AantalDeuren { get; set; }
        public Bestuurder Bestuurder { get; private set; }
        public bool HeeftVoertuigBestuurder => Bestuurder != null;
        public string VoertuigNaam => AutoModel.Merk + " " + AutoModel.AutoModelNaam + "";

#warning VoertuigNaam property voor WPF binding ingevoegd: nog niet getest

        public Voertuig(AutoModel autoModel, string chassisnummer, string nummerplaat, BrandstofVoertuig brandstof)
        {
            AutoModel = autoModel ?? throw new VoertuigException($"{nameof(AutoModel)} mag niet leeg zijn");

            if (CheckFormat.IsChassisNummerGeldig(chassisnummer))
            {
                ChassisNummer = chassisnummer.ToUpper();
            }

            if (CheckFormat.IsNummerplaatGeldig(nummerplaat))
            {
                NummerPlaat = nummerplaat.ToUpper();
            }

            Brandstof = brandstof ?? throw new VoertuigException($"{nameof(Brandstof)} mag niet leeg zijn");
        }

        public Voertuig(int voertuigId, AutoModel autoModel, string chassisnummer, string nummerplaat, 
            BrandstofVoertuig brandstof) : this(autoModel, chassisnummer, nummerplaat, brandstof)
        {
            if(voertuigId > 0)
            {
                VoertuigId = voertuigId;
            }
            else
            {
                throw new VoertuigException("VoertuigId moet meer zijn dan 0");
            }
        }

        public void VoegIdToe(int voertuigid)
        {
            if (voertuigid > 0)
            {
                if (VoertuigId == 0)
                {
                    VoertuigId = voertuigid;
                }
                else
                {
                    throw new BestuurderException($"{nameof(VoertuigId)} is al aanwezig en kan niet gewijzigd worden");
                }
            }
            else
            {
                throw new BestuurderException($"{nameof(VoertuigId)} moet meer zijn dan 0");
            }
        }

        //Maakt de relatie en plaatst entiteit
        public virtual void VoegBestuurderToe(Bestuurder ingegevenBestuurder)
        {
            if(ingegevenBestuurder == null)
            {
                throw new VoertuigException($"Ingegeven {nameof(Bestuurder)} mag niet null zijn");
            }

            if (!HeeftVoertuigBestuurder)
            {
                Bestuurder = ingegevenBestuurder;
                Bestuurder.VoegVoertuigToe(VoertuigId, this);
            }
            else
            {
                throw new VoertuigException($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}");
            }
        }

        //Vangt de relatie op en plaatst de entiteit
        public virtual void VoegBestuurderToe(int bestuurderId, Bestuurder ingegevenBestuurder)
        {
            if (ingegevenBestuurder == null)
            {
                throw new VoertuigException($"Ingegeven {nameof(Bestuurder)} mag niet null zijn");
            }

            if(bestuurderId < 0)
            {
                throw new VoertuigException($"De {nameof(Bestuurder)} is niet geslecteerd uit lijst bestuurders");
            }

            if (!HeeftVoertuigBestuurder)
            {
                Bestuurder = ingegevenBestuurder;
            }
            else
            {
                throw new VoertuigException($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}");
            }
        }

        //Maakt de relatie en verwijdert entiteit
        public virtual void VerwijderBestuurder(Bestuurder ingegevenBestuurder)
        {
            if (ingegevenBestuurder == null)
            {
                throw new VoertuigException($"Ingegeven {nameof(Bestuurder)} mag niet null zijn");
            }

            if (Bestuurder.Equals(ingegevenBestuurder))
            {
                Bestuurder.VerwijderVoertuig(VoertuigId, this);
                Bestuurder = null;
            }
            else
            {
                throw new VoertuigException($"{nameof(Bestuurder)} kan niet worden verwijderd");
            }
        }

        //Vangt de relatie op en verwijdert de entiteit
        public virtual void VerwijderBestuurder(int bestuurderId, Bestuurder ingegevenBestuurder)
        {
            if (ingegevenBestuurder == null)
            {
                throw new VoertuigException($"Ingegeven {nameof(Bestuurder)} mag niet null zijn");
            }

            if (Bestuurder.Equals(ingegevenBestuurder) && bestuurderId >= 0)
            {
                Bestuurder = null;
            }
            else
            {
                throw new VoertuigException($"{nameof(Bestuurder)} kan niet worden verwijderd");
            }
        }

        //Static check ingevoegd
        public virtual void UpdateNummerplaat(string nummerplaat)
        {
           if(CheckFormat.IsNummerplaatGeldig(nummerplaat))
            {
                NummerPlaat = nummerplaat;
            }
        }

        //Vergelijk twee instanties van Voertuig met: ChassisNummer & NummerPlaat
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

