using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Models {
    public class BrandstofType {
        public int BrandstofTypeId { get; private set; }  //Id mag nooit veranderen. get alleen zou ik overwegen
        public string BrandstofNaam { get; private set; } //brandstofnaam moet de klant kunnen wijzigen (Diesel/Hybride naar Hybride-diesel bijvoorbeeld, dat mag de klant zelf bepalen)

        public BrandstofType(int brandstofTypeId, string brandstofNaam) { //Als je instantie maakt van waar komt het brandstofTypeId?
            BrandstofTypeId = brandstofTypeId;
            BrandstofNaam = brandstofNaam;
        }
    }
}

/* 
 * Er is een beslingskeuze hier:
 * Brandstoftype kan wijzigen. Zo kan bijvoorbeeld diesel na 5 jaar verboden worden
 * Of kan een andere Brandstof de wereld veroveren
 * De klant moet dat in de hand hebben en kunnen aanmaken zonder programmeur
 * Maar kan ook uiteraard enum zijn (dan kan de klant het vragen, dat gebeurt waarschijnlijk niet zoveel)
 * Als het enum is kan de Id in constructor. Maar dan moet je wel de vraag stellen. Wie controleerd dat best? Presentatielaag (id in ctor; id van buitenaf) of businesslaag (id checken in logica aan de hand van de naam in enum/db).
 * Beslis wijs en implementeer één van die twee aub! De keuze is aan de maker van deze class. 
*/
