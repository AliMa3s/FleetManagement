using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.Test.ModelTest {
    public class TankKaartTest {
        [Fact]
        public void Test_NewTankkaart() {
            List<string> l1 = new List<string>();
            l1.Add("gas");
            TankKaart t = new TankKaart("123", new DateTime(2000, 01, 02), "1234", l1);

        }
    }
}
