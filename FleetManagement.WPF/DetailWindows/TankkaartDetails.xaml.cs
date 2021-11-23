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
    /// Interaction logic for TankkaartDetails.xaml
    /// </summary>
    public partial class TankkaartDetails : Window {
        public TankkaartDetails() {
            InitializeComponent();
        }

        private void SluitForm_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        private void WijzigButton_Click(object sender, RoutedEventArgs e) {
            

        }
    }
}
