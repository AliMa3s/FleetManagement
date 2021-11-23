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

namespace FleetManagement.WPF.UpdateWindows {
    /// <summary>
    /// Interaction logic for UpdateAdres.xaml
    /// </summary>
    public partial class UpdateAdres : Window {
        public UpdateAdres() {
            InitializeComponent();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e) {

        }

        private void SluitForm_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }
    }
}
