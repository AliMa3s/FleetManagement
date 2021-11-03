using FleetManagement.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.ADO.Repositories.Instances
{
    internal static class BouwInstance
    {
        //Spreek method aan om Bestuurder aan te maken
        public static Bestuurder BestuurderInstance(SqlDataReader dataReader)
        {
            return new(
                (int)dataReader["bestuurderId"],
                (string)dataReader["voornaam"],
                (string)dataReader["achternaam"],
                (string)dataReader["geboorteDatum"],
                (string)dataReader["rijbewijsType"],
                (string)dataReader["rijbewijsNummer"],
                (string)dataReader["rijksRegisterNummer"]
            ) {
                AanMaakDatum = (DateTime)dataReader["aanmaakDatum"]
            };
        }

        public static Adres AdresInstance(SqlDataReader dataReader)
        {
            return new(
                (int)dataReader["adresId"],
                (string)dataReader["straat"],
                (string)dataReader["nr"],
                (string)dataReader["postcode"],
                (string)dataReader["gemeente"]
            );
        }

        public static Voertuig VoertuigInstance(SqlDataReader dataReader)
        {
            //AutoType kan nog veranderen naar ConfigFile
            AutoType autoType = (AutoType)Enum.Parse(typeof(AutoType), (string)dataReader["autotype"]);

            //Kleur verschuift naar DB
            Kleur? kleur = (Kleur?)Enum.Parse(typeof(Kleur?), (string)dataReader["kleurnaam"]);
            AantalDeuren? aantalDeuren = (AantalDeuren?)Enum.Parse(typeof(AantalDeuren?), (string)dataReader["aantalDeuren"]);

            return new(
                new AutoModel(
                    (int)dataReader["autoModelId"],
                    (string)dataReader["merk"],
                    (string)dataReader["autoModelNaam"],
                    autoType
                ),
                (string)dataReader["chassisNummer"],
                (string)dataReader["nummerPlaat"],
                new BrandstofVoertuig((string)dataReader["brandstofNaam"], (bool)dataReader["hybride"])
            ) {
                AantalDeuren = aantalDeuren,
                VoertuigKleur = kleur,
                InBoekDatum = (DateTime)dataReader["inboekDatum"]
            };
        }

        public static TankKaart TankkaartInstance(SqlDataReader dataReader)
        {
            return new(
                (string)dataReader["tankKaartNummer"],
                (bool)dataReader["actief"],
                (DateTime)dataReader["geldigheidsDatum"],
                (string)dataReader["pincode"]
            ) {
                UitgeefDatum = (DateTime)dataReader["uitgeefDatum"]
            };
        }
    }
}
