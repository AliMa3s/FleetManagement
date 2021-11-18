
using FleetManagement.Exceptions;
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

        public VoertuigBouwer() { }

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
                StringBuilder mess = new StringBuilder("");
                if (AutoModel == null) mess.AppendLine($"{nameof(AutoModel)} mag niet leeg zijn");
                if (string.IsNullOrWhiteSpace(Chassisnummer)) mess.AppendLine($"{nameof(Chassisnummer)} mag niet leeg zijn");
                if (string.IsNullOrWhiteSpace(Nummerplaat)) mess.AppendLine($"{nameof(Nummerplaat)} mag niet leeg zijn");
                if (string.IsNullOrWhiteSpace(Brandstof)) mess.AppendLine($"{nameof(Brandstof)} mag niet leeg zijn");
                if (Bestuurder == null) mess.AppendLine($"{nameof(Bestuurder)} mag niet leeg zijn");

                throw new VoertuigBouwerException(mess.ToString());
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