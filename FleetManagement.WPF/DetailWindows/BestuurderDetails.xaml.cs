using FleetManagement.Manager;
using FleetManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FleetManagement.WPF.DetailWindows {
    /// <summary>
    /// Interaction logic for BestuurderDetails.xaml
    /// </summary>
    public partial class BestuurderDetails : Window {

        public BestuurderDetails(Managers managers, Bestuurder bestuurder) {
            InitializeComponent();
            
            //Controleer bestuurder om extra info op te vragen aan manager
            if(!bestuurder.HeeftBestuurderVoertuig || !bestuurder.HeeftBestuurderTankKaart)
            {
                //Bestuurder bestuurder = managers.BestuurderManager.GeefBestuurderInfo();
            }

            //bind de bestuurder
            DataContext = bestuurder;
        }

        private void SluitenDetail_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
