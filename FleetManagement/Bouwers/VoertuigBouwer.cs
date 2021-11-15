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

        #region alle velden vrij in te vullen (via event)
        public AutoModel AutoModel { get; set; }
        public string Chassisnummer { get; set;  }
        public string Nummerplaat { get; set; }
        public bool? Hybride { get; set; } = null;
        public string Brandstof { get; set; }
        public string Kleur { get; set; }
        public string AantalDeuren { get; set; }
        public Bestuurder Bestuurder { get; set; }
        #endregion

        public VoertuigBouwer(IVoertuigManager voertuigManager)
        {
            _voertuigManager = voertuigManager;
        }

        #region controleer alle verplichte velden op alle geldigheden
        public bool IsGeldig() 
        {
            return AutoModel != null
                && AutoModel.AutoModelId > 0
                && !string.IsNullOrWhiteSpace(Chassisnummer)
                && !string.IsNullOrWhiteSpace(Nummerplaat)
                && CheckFormat.IsChassisNummerGeldig(Chassisnummer)
                && CheckFormat.IsNummerplaatGeldig(Nummerplaat)
                && Hybride != null
                && !string.IsNullOrWhiteSpace(Brandstof)
                && Bestuurder != null
                && Bestuurder.BestuurderId > 0
                && IsChassisOfNummerplaatGeldig();
        }
        #endregion

        #region checkers
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
        #endregion

        #region levert correcte & complete instantie van Voertuig indien alles juist is
        public Voertuig BouwVoertuig()
        {
            if (!IsGeldig())
            {
                throw new VoertuigBouwerException("Voertuig kan niet worden gebouwd");
            }

            Voertuig voertuig = new(
                AutoModel,
                Chassisnummer,
                Nummerplaat,
                new(Brandstof, (bool)Hybride)
            );

            
            //Niet verplichte velden al dan niet toevoegen
            if (Enum.IsDefined(typeof(Kleur), Kleur))
            {
                voertuig.VoertuigKleur = (Kleur)Enum.Parse(typeof(Kleur), Kleur);
            }
            else
            {
                throw new VoertuigBouwerException("Kleur van Voertuig bestaat niet");
            }

            if (Enum.IsDefined(typeof(AantalDeuren), AantalDeuren))
            {
                voertuig.AantalDeuren = (AantalDeuren)Enum.Parse(typeof(AantalDeuren), AantalDeuren);
            } 
            else
            {
                throw new VoertuigBouwerException("Aantal deuren bestaat niet");
            }

            voertuig.VoegBestuurderToe(Bestuurder);
            return voertuig;
        }
        #endregion

        #region vraag op wat allemmal onjuist is 
        public string Status()
        {
            StringBuilder message = new();

            #region zend foutbericht van verplichte velden
            if (AutoModel == null) { message.AppendLine($"{nameof(AutoModel)} mag niet leeg zijn"); }
            if (string.IsNullOrWhiteSpace(Chassisnummer)) { message.AppendLine($"{nameof(Chassisnummer)} mag niet leeg zijn"); }
            if (string.IsNullOrWhiteSpace(Nummerplaat)) { message.AppendLine($"{nameof(Nummerplaat)} mag niet leeg zijn"); }
            if (Bestuurder == null) { message.AppendLine($"{nameof(Bestuurder)} mag niet leeg zijn"); }
            if (Hybride == null) { message.AppendLine($"{nameof(Hybride)} moet ja of neen zijn"); }
            if (string.IsNullOrWhiteSpace(Brandstof)) { message.AppendLine($"{nameof(Brandstof)} mag niet leeg zijn"); }

            if (!string.IsNullOrEmpty(message.ToString()))
            {
                return message.ToString();
            }
            #endregion

            #region zend foutbericht van ongeldige velden
            if (AutoModel.AutoModelId < 1) { message.AppendLine($"{nameof(AutoModel)} is niet gelecteerd uit de lijst"); }
            if (Bestuurder.BestuurderId < 1) { message.AppendLine($"{nameof(Bestuurder)} is niet geslecteerd uit de lijst"); }
            if (!CheckFormat.IsChassisNummerGeldig(Chassisnummer)) { message.AppendLine($"{nameof(Chassisnummer)} is niet het correcte formaat"); }
            if (!CheckFormat.IsNummerplaatGeldig(Nummerplaat)) { message.AppendLine($"{nameof(Nummerplaat)} is niet het correcte formaat"); }

            if (!string.IsNullOrEmpty(message.ToString()))
            {
                return message.ToString();
            }
            #endregion

            #region zend foutbericht indien niet kan geparst worden naar enumlijst
            if (!Enum.IsDefined(typeof(Kleur), Kleur))
            {
                message.AppendLine($"{nameof(Kleur)} is niet geslecteerd uit de lijst");
            }

            if (!Enum.IsDefined(typeof(AantalDeuren), AantalDeuren))
            {
                message.AppendLine($"{nameof(AantalDeuren)} is niet geslecteerd uit de lijst");
            }

            if (!string.IsNullOrEmpty(message.ToString()))
            {
                return message.ToString();
            }
            #endregion

            #region zend foutbericht indien dubbel is aangtroffen
            if (!IsChassisNummerGeldig()) { message.AppendLine($"{nameof(Chassisnummer)} bestaat reeds"); }
            if (!IsNummerplaatGeldig()) { message.AppendLine($"{nameof(Nummerplaat)} bestaat reeds"); }
            #endregion

            return message.ToString();
        }
        #endregion
    }
}
