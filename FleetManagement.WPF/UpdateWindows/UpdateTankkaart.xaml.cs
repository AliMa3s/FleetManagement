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
    /// Interaction logic for UpdateTankkaart.xaml
    /// </summary>
    public partial class UpdateTankkaart : Window
    {
        private readonly object _managers;

        private TankKaart _tankaart;

        public TankKaart TankkaartDetail
        {
            get => _tankaart;
            set
            {
                _tankaart = value;
            }
        }

        public UpdateTankkaart(Managers managers, TankKaart tankkaart)
        {
            InitializeComponent();
            _managers = managers;
        }

        private void TankKaartUpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetVeldenButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SluitTankKaartWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetGekozenBrandstofButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BrandstofToevoegenButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
