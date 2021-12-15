using FleetManagement.Exceptions;
using System;
namespace FleetManagement.Model
{
    public class Kleur
    {
        public int KleurId { get; private set; }
        public string KleurNaam { get; private set; }

        public Kleur(string kleurNaam)
        {
            KleurNaam = kleurNaam ?? throw new VoertuigKleurException("kleur mag niet null zijn");
        }

        public Kleur(int kleurId, string kleurNaam) : this(kleurNaam)
        {
            if (kleurId < 1) throw new VoertuigKleurException("id mag niet kleiner zijn dan 1");

            KleurId = kleurId;
        }

        #region Overridables
        //Vergelijk twee instanties van Kleur met: kleurnaam
        public override bool Equals(object obj)
        {
            if (obj is Kleur)
            {
                Kleur ander = obj as Kleur;
                return KleurNaam == ander.KleurNaam;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return KleurNaam.GetHashCode();
        }
        #endregion
    }
}