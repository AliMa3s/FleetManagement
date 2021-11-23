using FleetManagement.Manager;
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

namespace FleetManagement.WPF.UpdateWindows
{
    /// <summary>
    /// Interaction logic for UpdateBestuurder.xaml
    /// </summary>
    public partial class UpdateBestuurder : Window
    {
        private readonly Managers _managers;

        public UpdateBestuurder(Managers managers)
        {
            InitializeComponent();
            _managers = managers;
        }

        private void Tankkaartverwijderen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AdresWijzigen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SluitForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
