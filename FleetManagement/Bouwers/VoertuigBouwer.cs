using FleetManagement.CheckFormats;
using FleetManagement.Exceptions;
using FleetManagement.Interfaces;
using FleetManagement.Manager;
using FleetManagement.Manager.Interfaces;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace FleetManagement.Bouwers
{
    public class VoertuigBouwer
    {
        private readonly IVoertuigManager _voertuigManager;

        public AutoModel AutoModel { get; set; }
        public string Chassisnummer { get; set;  }
        public string Nummerplaat { get; set; }
        public BrandstofVoertuig Brandstof { get; set; }
        public Kleur? VoertuigKleur { get; set; }
        public AantalDeuren? AantalDeuren { get; set; }
        public Bestuurder Bestuurder { get; set; }

        public VoertuigBouwer(IVoertuigManager voertuigManager)
        {
            _voertuigManager = voertuigManager;
        }

        public bool IsGeldig() 
        {
            return AutoModel != null
                && AutoModel.AutoModelId > 0
                && !string.IsNullOrWhiteSpace(Chassisnummer)
                && !string.IsNullOrWhiteSpace(Nummerplaat)
                && CheckFormat.IsChassisNummerGeldig(Chassisnummer)
                && CheckFormat.IsNummerplaatGeldig(Nummerplaat)
                && Bestuurder != null
                && Bestuurder.BestuurderId > 0
                && Brandstof != null
                && IsChassisOfNummerplaatGeldig();
        }

        private bool IsChassisNummerGeldig()
        {
            //return !_voertuigManager.BestaatChassisNummer(Chassisnummer);
            throw new NotImplementedException("Interface bestaat niet");
        }

        private bool IsNummerplaatGeldig()
        {
            //return !_voertuigManager.BestaatNummerplaat(Nummerplaat);
            throw new NotImplementedException("Interface bestaat niet");

        }

        private bool IsChassisOfNummerplaatGeldig()
        {
            return !_voertuigManager.bestaatChassisOfNummerplaat(Chassisnummer, Nummerplaat);
        }

        public Voertuig BouwVoertuig()
        {
            if(!IsGeldig())
            {
                throw new VoertuigBouwerException("Voertuig kan niet worden gebouwd");
            }

            //Check kleur & aantal deuren mss nog indien ingevuld (indien API ipv WPF Parsen van selectors)

            Voertuig voertuig = new(
                AutoModel,
                Chassisnummer,
                Nummerplaat,
                Brandstof
            ) {
                VoertuigKleur = VoertuigKleur,
                AantalDeuren = AantalDeuren
            };

            voertuig.VoegBestuurderToe(Bestuurder);
            return voertuig;
        }

       public string Status()
        {
            //Controleer verplichte velden
            StringBuilder message = new();
            if (AutoModel == null) { message.AppendLine($"{nameof(AutoModel)} mag niet leeg zijn"); }
            if (string.IsNullOrWhiteSpace(Chassisnummer)) { message.AppendLine($"{nameof(Chassisnummer)} mag niet leeg zijn"); }
            if (string.IsNullOrWhiteSpace(Nummerplaat)) { message.AppendLine($"{nameof(Nummerplaat)} mag niet leeg zijn"); }
            if (Bestuurder == null) { message.AppendLine($"{nameof(Bestuurder)} mag niet leeg zijn"); }
            if (Brandstof == null) { message.AppendLine($"{nameof(Brandstof)} mag niet leeg zijn"); }

            if(!string.IsNullOrEmpty(message.ToString()))
            {
                return message.ToString();
            }

            //Controleer geldiheid van de velden
            if (AutoModel.AutoModelId < 1) { message.AppendLine($"{nameof(AutoModel)} is niet gelecteerd uit de lijst"); }
            if (Bestuurder.BestuurderId < 1) { message.AppendLine($"{nameof(Bestuurder)} is niet geslecteerd uit de lijst"); }
            if (!CheckFormat.IsChassisNummerGeldig(Chassisnummer)) { message.AppendLine($"{nameof(Chassisnummer)} is niet het correcte formaat"); }
            if (!CheckFormat.IsNummerplaatGeldig(Nummerplaat)) { message.AppendLine($"{nameof(Nummerplaat)} is niet het correcte formaat"); }

            if (!string.IsNullOrEmpty(message.ToString()))
            {
                return message.ToString();
            }

            //Check bij manager of deze geldig om toe te voegen
            if (!IsChassisNummerGeldig()) { message.AppendLine($"{nameof(Chassisnummer)} bestaat reeds"); }
            if (!IsNummerplaatGeldig()) { message.AppendLine($"{nameof(Nummerplaat)} bestaat reeds"); }

            return message.ToString();
        }
    }
}
