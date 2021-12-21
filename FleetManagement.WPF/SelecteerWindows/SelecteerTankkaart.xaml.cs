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

        public string Placeholder { get; } = "Tankkaartnummer";

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

            TankkaartNummerText.Text = Placeholder;
            TankkaartLijst.ItemsSource = _tankkaartManager.TankaartenZonderBestuurder();
        }
        //tankkaart bewaren telkens een Bestuurder wordt geselecteerd
        private void BewaarTankkaart_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            infoTankkaartMess.Text = string.Empty;
            Tankkaart = TankkaartLijst.SelectedItem as TankKaart; 
        }

        //Ga terug wanneer een tankkaart is gekozen
        private void ButtonKiesToevoegen_Click(object sender, RoutedEventArgs e) 
        {
            infoTankkaartMess.Text = string.Empty;

            //_bestuurder mag niet leeg zijn anders geen terugkeer
            if (Tankkaart!= null)
            {
                DialogResult = true;
            }
        }

        //Ga terug bij dubbelklik op een rij in de lijst 
        private void TankkaartToevoegenDoubleClick(object sender, MouseButtonEventArgs e) 
        {
            infoTankkaartMess.Text = string.Empty;

            //_bestuurder mag niet leeg zijn anders geen terugkeer
            if (Tankkaart != null)
            {
                DialogResult = true;
            }
        }

        private void ButtonAnnuleer_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }

        private void ZoekOpTankkaartNummer_Click(object sender, RoutedEventArgs e)
        {
            infoTankkaartMess.Text = string.Empty;

            TankKaart tankkaart = _tankkaartManager.ZoekTankKaart(TankkaartNummerText.Text);

            if(tankkaart != null)
            {
                List<TankKaart> tankKaarten = new();
                tankKaarten.Add(tankkaart);
                TankkaartLijst.ItemsSource = tankKaarten;
            }
            else
            {
                infoTankkaartMess.Text = "Geen resultaten";
            }
        }

        private void TankkaartNummerText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TankkaartNummerText.Text == Placeholder)
            {
                TankkaartNummerText.Text = string.Empty;
                TankkaartNummerText.Foreground = Brushes.Black;
                TankkaartLijst.SelectedItem = null;
            }
        }

        private void TankkaartNummerText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TankkaartNummerText.Text))
            {
                TankkaartNummerText.Text = Placeholder;
                TankkaartNummerText.Foreground = Brushes.LightSlateGray;
            }
        }
    }
}
