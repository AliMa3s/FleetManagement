using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Interfaces {
    public interface IAdresRepository {
        bool BestaatAdres(Adres adres);
        void VoegAdresToe(Adres adres);
        void UpdateAdres(Adres adres);
        void VerwijderAdres(Adres adres);
        bool BestaatAdres(int adresId);
    }
}
