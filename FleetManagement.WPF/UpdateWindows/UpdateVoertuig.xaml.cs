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

namespace FleetManagement.WPF.UpdateWindows
{
    /// <summary>
    /// Interaction logic for UpdateVoertuig.xaml
    /// </summary>
    public partial class UpdateVoertuig : Window
    {
        private readonly Managers _managers;
        private Voertuig _voertuig;

        public Voertuig VoertuigDetail
        {
            get => _voertuig;
            set
            {
                _voertuig = value;
            }
        }

        public UpdateVoertuig(Managers managers, Voertuig voertuig)
        {
            InitializeComponent();
            _managers = managers;
            _voertuig = voertuig;
        }

        private void UpdateVoertuigButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SluitVoertuigForm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void KiesBestuurder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void KiesAutoModel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
