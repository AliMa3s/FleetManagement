using FleetManagement.Exceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test.ModelTest {

    public class AdresTest {
        [Fact]
        public void Adres_Ctor_NoId() {
            Adres adres = new("moerstraat", "16w2", "9240", "zele");
            Assert.Equal("moerstraat", adres.Straat);
            Assert.Equal("16w2", adres.Nr);
            Assert.Equal("9240", adres.Postcode);
            Assert.Equal("zele", adres.Gemeente);
            adres.ToString();
        }

        [Fact]
        public void Adres_Ctor_WithId() {
            Adres adres = new("", "", "", ""); //adres mag lege strings hebben
            adres.VoegIdToe(1);
            Assert.Equal(1, adres.AdresId);
        }

        [Fact]
        public void Adres_Ctor_Id_fout() {
            Adres adres = new("", "20", "", "Gent");
            var e = Assert.Throws<AdresException>(() => adres.VoegIdToe(-56));
            Assert.Equal("AdresId moet meer zijn dan 0", e.Message);
        }

        [Fact]
        public void StraatNull_Invalid()
        {
            var e = Assert.Throws<AdresException>(() => {
                Adres adres = new(null, "", "", "");
            });
            Assert.Equal("Straat mag niet null zijn", e.Message);
        }

        [Fact]
        public void NrNull_Invalid()
        {
            var e = Assert.Throws<AdresException>(() => {
                Adres adres = new("", null, "", "");
            });
            Assert.Equal("Nummer mag niet null zijn", e.Message);
        }

        [Fact]
        public void PostcodeNull_Invalid()
        {
            var e = Assert.Throws<AdresException>(() => {
                Adres adres = new("", "", null, "");
            });
            Assert.Equal("Postcode mag niet null zijn", e.Message);
        }

        [Fact]
        public void GemeenteNull_Invalid()
        {
            var e = Assert.Throws<AdresException>(() => {
                Adres adres = new("", "", "", null);
            });
            Assert.Equal("Gemeente mag niet null zijn", e.Message);
        }
    }
}
