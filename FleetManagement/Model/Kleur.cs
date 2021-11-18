using System;
namespace FleetManagement.Model
{
    public class Kleur
    {
        public int KleurId { get; set; }
        public string KleurNaam { get; set; }

        public Kleur(string kleurNaam)
        {
            if (kleurNaam == null)
            {
                //exception te maken
            }

            KleurNaam = kleurNaam;
        }

        public Kleur(int kleureId, string kleurNaam) : this(kleurNaam)
        {
            if (kleureId < 1)
            {
                //exception te maken
            }

            KleurId = kleureId;
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

    //verhuist naar DB
    //   public enum Kleur
    //{
    //	Wit, Zwart, Grijs, Zilver, Blauw, Rood, BruinBeige, GoudGeel, Groen
    //}
}