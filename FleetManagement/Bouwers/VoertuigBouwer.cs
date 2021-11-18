using FleetManagement.CheckFormats;
using FleetManagement.Exceptions;
using FleetManagement.Interfaces;
using FleetManagement.Manager;
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
        private readonly VoertuigManager _voertuigManager;

        #region alle velden vrij in te vullen
        public AutoModel AutoModel { get; set; }
        public string Chassisnummer { get; set;  }
        public string Nummerplaat { get; set; }
        public bool Hybride { get; set; }
        public string Brandstof { get; set; }
        public string Kleur { get; set; }
        public string AantalDeuren { get; set; }
        public Bestuurder Bestuurder { get; set; }
        #endregion

        public VoertuigBouwer(VoertuigManager voertuigManager)
        {
            _voertuigManager = voertuigManager;
            Hybride = false;
        }

        #region controleer alle verplichte velden op alle geldigheden
        public bool IsGeldig()
        {
            return AutoModel != null
                && !string.IsNullOrWhiteSpace(Chassisnummer)
                && !string.IsNullOrWhiteSpace(Nummerplaat)
                && !string.IsNullOrWhiteSpace(Brandstof)
                && Bestuurder != null;
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
                new(Brandstof, Hybride)
            )
            {
                VoertuigKleur = new Kleur(Kleur)
            };

            //Niet-verplichte-velden toevoegen of niet
            if(!string.IsNullOrEmpty(Kleur))
            {
                voertuig.VoertuigKleur = Enum.IsDefined(typeof(Kleur), Kleur)
                    ? (Kleur)Enum.Parse(typeof(Kleur), Kleur)
                    : throw new VoertuigBouwerException("Kleur van Voertuig bestaat niet");
            }

            if(!string.IsNullOrEmpty(AantalDeuren))
            {
                voertuig.AantalDeuren = Enum.IsDefined(typeof(AantalDeuren), AantalDeuren)
                    ? (AantalDeuren)Enum.Parse(typeof(AantalDeuren), AantalDeuren)
                    : throw new VoertuigBouwerException("Aantal deuren bestaat niet");
            }

            voertuig.VoegBestuurderToe(Bestuurder);
            return voertuig;
        }
        #endregion
    }
}