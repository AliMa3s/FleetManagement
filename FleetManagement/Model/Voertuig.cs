﻿using System;
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
        public int VoertuigId { get; }
        public AutoModel AutoModel { get; set; } 
        public string ChassisNummer { get; }
        public string NummerPlaat { get; private set; }
        public Kleur? VoertuigKleur { get; set; } = null;
        public BrandstofType Brandstof { get; }
        public DateTime InBoekDatum { get; set; }
        public AantalDeuren? AantalDeuren { get; set; } = null;
        public Bestuurder Bestuurder { get; private set; }
        public bool HeeftVoertuigBestuurder => Bestuurder != null;

        //Ctor
        public Voertuig(AutoModel autoModel, string chassisnummer, string nummerplaat, BrandstofType brandstof)
        {
            if(CheckFormat.IsChassisNummerGeldig(chassisnummer))
            {
                ChassisNummer = chassisnummer;
            }

            if (CheckFormat.IsNummerplaatGeldig(nummerplaat))
            {
                NummerPlaat = nummerplaat;
            }
            
            AutoModel = autoModel;
            Brandstof = brandstof;
        }

        public Voertuig(int voertuigId, AutoModel autoModel, string chassisnummer, string nummerplaat, BrandstofType brandstof) 
            : this(autoModel, chassisnummer, nummerplaat, brandstof)
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
                Bestuurder.VoegVoertuigToe("connecteren", this);
            }
            else
            {
                throw new VoertuigException($"{nameof(Voertuig)} heeft al een {nameof(Bestuurder)}");
            }
        }

        //Vangt de relatie op en plaatst de entiteit
        public virtual void VoegBestuurderToe(string actie, Bestuurder ingegevenBestuurder)
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

        //Maakt de relatie en verwijdert entiteit
        public virtual void VerwijderBestuurder(Bestuurder ingegevenBestuurder)
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

        //Vangt de relatie op en verwijdert de entiteit
        public virtual void VerwijderBestuurder(string actie, Bestuurder ingegevenBestuurder)
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
        public virtual void UpdateNummerplaat(string nummerplaat)
        {
           if(CheckFormats.CheckFormat.IsNummerplaatGeldig(nummerplaat))
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

