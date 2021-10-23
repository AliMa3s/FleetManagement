using FleetManagement.CheckFormats;
using FleetManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test
{
    public class ChassisNummerTest
    {
        [Theory]
        [InlineData("ABCDEFGHJKLMN1234")]
        [InlineData("0234DkzpsGUKNt6Gb")]
        public void ChassisnummerValid(string ChassisNummer)
        {
            bool check = CheckFormat.IsChassisNummerGeldig(ChassisNummer);
            Assert.True(check);
        }
        
        

        //Voertuig hoort hier niet thuis. Dat is een gedeelte van de Static class CheckFormats
        //Wordt nog uitgebreid met de juiste zaken
        
        
        //[Theory]
        //[InlineData("88kloplkk144")]
        //public void ChassisnummerInvalid(string chassisnummer)
        //{
        //    AutoModel automodel = new AutoModel("mercedes", "mercedes-c klasse", AutoType.GT);
        //    Voertuig voertuig = new Voertuig(automodel, chassisnummer, "1FEG830", new("Diezel"));
        //    var ex = Assert.Throws<ChassisNummerException>(() => voertuig.ChassisNummer);
        //    Assert.Equal("chassisnummer moet string zijn van 17 cijfers/letters maar letter I/i, O/o en Q/q mag niet voorkomen", ex.Message);

        //}
    }
}