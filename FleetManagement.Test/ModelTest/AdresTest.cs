using FleetManagement.Exceptions;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test.ModelTest
{
    
    public class AdresTest
    {
        [Fact]
        public void Adres_Ctor_NoId()
        {
            Adres adres = new Adres("moerstraat", "16w2", "9240", "zele");
            Assert.Equal("moerstraat", adres.Straat);
            Assert.Equal("16w2", adres.Nr);
            Assert.Equal("9240", adres.Postcode);
            Assert.Equal("zele", adres.Stad);
            adres.ToString();

        }

        [Fact]
        public void Adres_Ctor_WithId()
        {
            Adres adres = new(1, "", "", "", ""); //adres mag lege strings hebben
            Assert.Equal(1, adres.AdresId);
        }

        [Fact]
        public void Adres_Ctor_Id_fout()
        {
            var e = Assert.Throws<AdresException>(() => new Adres(-56, "", "20", "", "Gent"));
            Assert.Equal("AdresId moet meer zijn dan 0", e.Message);
        }
    }
}
