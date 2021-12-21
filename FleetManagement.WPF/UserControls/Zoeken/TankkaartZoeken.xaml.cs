using FleetManagement.Manager;
using FleetManagement.Model;
using FleetManagement.WPF.DetailWindows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FleetManagement.WPF.UserControls.Zoeken
{
    /// <summary>
    /// Interaction logic for TankkaartZoeken.xaml
    /// </summary>
    public partial class TankkaartZoeken : UserControl
    {
        private readonly Managers _managers;

        private TankKaart _tankkaart;
        private int _tankkaartItem;
        private string _zoekOpTankkaartNummer;

        private string PlaceHolderTankkaart { get; } = "Tankkaartnummer";
        public TankKaart TankkaartWeergave
        {
            get => _tankkaart;
            set
            {
                _tankkaart = value;
                ZoekweergaveTankkaart.SelectedItem = value;
            }
        }
  
        public TankkaartZoeken(Managers managers)
        {
            InitializeComponent();
            _managers = managers; 

            ZoekTankkaartFilter.Items.Add("Alle tankkaarten");
            ZoekTankkaartFilter.Items.Add("Actieve tankkaarten");
            ZoekTankkaartFilter.Items.Add("Inactieve tankkaarten");

            ZoekweergaveTankkaart.ItemsSource = _managers.TankkaartManager.GeefAlleTankkaarten();

            TankkaartNummer.Text = PlaceHolderTankkaart;
        }

        private void ZoekWeergave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TankkaartWeergave = ZoekweergaveTankkaart.SelectedItem as TankKaart;
        }

        private void ZoekenMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_tankkaart != null)
            {
                TankkaartDetails detailWindow = new(_managers, _tankkaart)
                {
                    Owner = Window.GetWindow(this),
                };

                bool? verwijderd = detailWindow.ShowDialog();
                if (verwijderd == true)
                {
                    //zelf iets flexibeler gemaakt, vrij iets in te doen
                }

                if ((bool)detailWindow.Updatetet)
                {
                    ZoekInFilter();
                    TankkaartWeergave = detailWindow.Tankkaart;
                }
            }
        }

        private void KiesDetail_Click(object sender, RoutedEventArgs e)
        {
            if (_tankkaart != null)
            {
                infoTankkaartMess.Text = string.Empty;
                TankkaartDetails detailWindow = new(_managers, _tankkaart)
                {
                    Owner = Window.GetWindow(this),
                };

                bool? verwijderd = detailWindow.ShowDialog();
                if (verwijderd == true)
                {
                    //zelf iets flexibeler gemaakt, vrij iets in te doen
                }

                if ((bool)detailWindow.Updatetet)
                {
                    ZoekInFilter();
                    TankkaartWeergave = detailWindow.Tankkaart;
                }
            }
        }
        private void ZoekTankkaartFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            infoTankkaartMess.Text = string.Empty;
            _tankkaartItem = ZoekTankkaartFilter.SelectedIndex;
            ZoekInFilter();
        }

        private void ZoekInFilter()
        {
            ZoekweergaveTankkaart.ItemsSource = _tankkaartItem switch
            {
                1 => _managers.TankkaartManager.ZoekTankKaarten(true),
                2 => _managers.TankkaartManager.ZoekTankKaarten(false),
                _ => _managers.TankkaartManager.GeefAlleTankkaarten(),
            };
        }

        private void ZoektankkaartNummer_Click(object sender, RoutedEventArgs e)
        {
            infoTankkaartMess.Text = string.Empty;
            _zoekOpTankkaartNummer = TankkaartNummer.Text;
            ZoekInTankkaartNummers();
        }

        private void ZoekInTankkaartNummers()
        {
            if (!string.IsNullOrWhiteSpace(_zoekOpTankkaartNummer))
            {
                List<TankKaart> tankkaarten = new();
                TankKaart tankkaart = _managers.TankkaartManager.ZoekTankKaart(_zoekOpTankkaartNummer);

                if (tankkaart != null)
                {
                    tankkaarten.Add(tankkaart);
                    ZoekweergaveTankkaart.ItemsSource = tankkaarten;
                }
                else
                {
                    infoTankkaartMess.Foreground = Brushes.Red;
                    infoTankkaartMess.Text = "Geen tankkaart gevonden!";
                }
            }
        }

        private void SluitWindow_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void TankkaartNummer_GotFocus(object sender, RoutedEventArgs e)
        {
            if(TankkaartNummer.Text == PlaceHolderTankkaart)
            {
                TankkaartNummer.Text = string.Empty;
                TankkaartNummer.Foreground = Brushes.Black;
                ZoekweergaveTankkaart.SelectedItem = null;
            }
        }

        private void TankkaartNummer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TankkaartNummer.Text))
            {
                TankkaartNummer.Text = PlaceHolderTankkaart;
                TankkaartNummer.Foreground = Brushes.LightSlateGray;
            }
        }
    }
}
