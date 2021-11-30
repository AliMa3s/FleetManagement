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

        }

        private void ZoekWeergave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TankkaartWeergave = ZoekweergaveTankkaart.SelectedItem as TankKaart;
        }

        private void ZoekenMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_tankkaart != null)
            {
                TankkaartDetails detailWindow = new TankkaartDetails(_managers, _tankkaart)
                {
                    Owner = Window.GetWindow(this),
                };

                //Uitgezet anders geen update status mogelijk
                //detailWindow.Show();

                bool? verwijderd = detailWindow.ShowDialog();
                if (verwijderd == true)
                {
                    ZoekweergaveTankkaart.ItemsSource = _managers.TankkaartManager.GeefAlleTankkaarten();
                }
            }
        }

        private void KiesDetail_Click(object sender, RoutedEventArgs e)
        {
            if (_tankkaart != null)
            {
                TankkaartDetails detailWindow = new TankkaartDetails(_managers, _tankkaart)
                {
                    Owner = Window.GetWindow(this),
                };

                //Uitgezet anders geen update status mogelijk
                //detailWindow.Show();

                bool? verwijderd = detailWindow.ShowDialog();
                if (verwijderd == true)
                {
                    ZoekweergaveTankkaart.ItemsSource = _managers.TankkaartManager.GeefAlleTankkaarten();
                }
            }
        }
        private void ZoekTankkaartFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            infoTankkaartMess.Text = "";

            switch (ZoekTankkaartFilter.SelectedIndex)
            {
                case 1:
                    ZoekweergaveTankkaart.ItemsSource = _managers.TankkaartManager.ZoekTankKaarten(true);
                    break;

                case 2:
                    ZoekweergaveTankkaart.ItemsSource = _managers.TankkaartManager.ZoekTankKaarten(false);
                    break;

                default:
                    ZoekweergaveTankkaart.ItemsSource = _managers.TankkaartManager.GeefAlleTankkaarten();
                    break;
            }
        }

        private void ZoektankkaartNummer_Click(object sender, RoutedEventArgs e)
        {
            infoTankkaartMess.Text = "";

            if (!string.IsNullOrWhiteSpace(TankkaartNummer.Text))
            {
                List<TankKaart> tankkaarten = new();
                TankKaart tankkaart = _managers.TankkaartManager.ZoekTankKaart(TankkaartNummer.Text);
                
                if(tankkaart != null)
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
    }
}
