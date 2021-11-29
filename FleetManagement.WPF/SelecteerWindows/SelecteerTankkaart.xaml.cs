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

namespace FleetManagement.WPF.SelecteerWindows
{
    /// <summary>
    /// Interaction logic for SdelecteerTankkaart.xaml
    /// </summary>
    public partial class SelecteerTankkaart : Window
    {
        private readonly TankkaartManager _tankkaartManager;
        private TankKaart _tankkaart;

        public TankKaart Tankkaart {
            get => _tankkaart;
            set {
                _tankkaart = value;
                TankkaartLijst.SelectedItem = value;
            }
        }
        public SelecteerTankkaart(TankkaartManager manager)
        {
            InitializeComponent();
            _tankkaartManager = manager;

            TankkaartLijst.ItemsSource = _tankkaartManager.TankaartenZonderBestuurder();
        }
        //tankkaart bewaren telkens een Bestuurder wordt geselecteerd
        private void BewaarTankkaart_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            Tankkaart = TankkaartLijst.SelectedItem as TankKaart; 
        }

        //Ga terug wanneer een tankkaart is gekozen
        private void ButtonKiesToevoegen_Click(object sender, RoutedEventArgs e) 
        {
            //_bestuurder mag niet leeg zijn anders geen terugkeer
            if (Tankkaart!= null)
            {
                DialogResult = true;
            }
        }

        //Ga terug bij dubbelklik op een rij in de lijst 
        private void TankkaartToevoegenDoubleClick(object sender, MouseButtonEventArgs e) 
        {
            //_bestuurder mag niet leeg zijn anders geen terugkeer
            if (Tankkaart != null)
            {
                DialogResult = true;
            }
        }

        private void ButtonAnnuleer_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }
    }
}
