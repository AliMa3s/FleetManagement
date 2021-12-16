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
    /// Interaction logic for BestuurderZoeken.xaml
    /// </summary>
    public partial class BestuurderZoeken : UserControl
    {
        private readonly Managers _managers;
        private Bestuurder _bestuurder;
        private string _filterOpNaam = "";
        private string _zoekOpRijksregister = "";

        public Bestuurder Bestuurderweergave
        {
            get => _bestuurder;
            set
            {
                _bestuurder = value;
                BestuurderZoekWeergave.SelectedItem = value;
            }
        }

        public string PlaceholderName { get; } = "Achternaam + Voornaam";
        private string PlaceHolderRijksregister { get; } = "Rijksregisternummer";
        public BestuurderZoeken(Managers managers)
        {
            InitializeComponent();
            _managers = managers;

            FilterOpNaam.Text = PlaceholderName;
            RijksregisterBox.Text = PlaceHolderRijksregister;
            BestuurderZoekWeergave.ItemsSource = _managers.BestuurderManager.FilterOpBestuurdersNaam(_filterOpNaam); 
        }

        private void ZoekWeergave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bestuurderweergave = BestuurderZoekWeergave.SelectedItem as Bestuurder;
        }

        private void ZoekenMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(Bestuurderweergave != null)
            {
              GetDetailWindow();
            }
        }

        private void ZoekRijksregister_Click(object sender, RoutedEventArgs e)
        {
            infoBestuurderMess.Text = string.Empty;

            if(RijksregisterBox.Text != PlaceHolderRijksregister)
            {
                _zoekOpRijksregister = RijksregisterBox.Text;

                List<Bestuurder> bestuurders = new();
                Bestuurder bestuurderDB = _managers.BestuurderManager.ZoekBestuurder(_zoekOpRijksregister);

                if (bestuurderDB != null)
                {
                    bestuurders.Add(bestuurderDB);
                    BestuurderZoekWeergave.ItemsSource = bestuurders;
                }
                else
                {
                    infoBestuurderMess.Foreground = Brushes.Red;
                    infoBestuurderMess.Text = "Geen resultaten";
                    BestuurderZoekWeergave.ItemsSource = bestuurders;
                }

                BestuurderZoekWeergave.ItemsSource = bestuurders;
            }
        }

        private void FilterOpNaam_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterOpNaam.Text != PlaceholderName)
            {
                _filterOpNaam = FilterOpNaam.Text;
                BestuurderZoekWeergave.ItemsSource = _managers.BestuurderManager.FilterOpBestuurdersNaam(_filterOpNaam); 
            }
        }

        private void FilterOpNaam_GotFocus(object sender, RoutedEventArgs e)
        {
           if(FilterOpNaam.Text == PlaceholderName)
            {
                FilterOpNaam.Text = string.Empty;
                FilterOpNaam.Foreground = Brushes.Black;
            }
        }

        private void FilterOpNaam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilterOpNaam.Text))
            {
                FilterOpNaam.Text = PlaceholderName;
                FilterOpNaam.Foreground = Brushes.LightSlateGray;
            }
        }

        private void SluitWindow_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        private void KiesDetail_Click(object sender, RoutedEventArgs e)
        {
            if (Bestuurderweergave != null)
            {
                GetDetailWindow();
            }
        }

        private void Rijksregister_GotFocus(object sender, RoutedEventArgs e)
        {
            if(RijksregisterBox.Text == PlaceHolderRijksregister)
            {
                RijksregisterBox.Text = string.Empty;
                RijksregisterBox.Foreground = Brushes.Black;

            }
        }

        private void Rijksregister_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RijksregisterBox.Text))
            {
                RijksregisterBox.Text = PlaceHolderRijksregister;
                RijksregisterBox.Foreground = Brushes.LightSlateGray;
            }
        }

        private void GetDetailWindow()
        {
            BestuurderDetails detailWindow = new BestuurderDetails(_managers, Bestuurderweergave)
            {
                Owner = Window.GetWindow(this),
            };

            bool? action = detailWindow.ShowDialog();
            if ((bool)action)
            {
                //vrij iets in te doen er werd een flexibelere manier geconfigureerd hieronder
            }

            if ((bool)detailWindow.Updatetet)
            {
                if(!string.IsNullOrWhiteSpace(_zoekOpRijksregister))
                {
                    List<Bestuurder> bestuurders = new();
                    Bestuurder bestuurderDB = _managers.BestuurderManager.ZoekBestuurder(_zoekOpRijksregister);

                    if (bestuurderDB != null)
                    {
                        bestuurders.Add(bestuurderDB);
                        BestuurderZoekWeergave.ItemsSource = bestuurders;
                    }
                    else
                    {
                        BestuurderZoekWeergave.ItemsSource = _managers.BestuurderManager.FilterOpBestuurdersNaam("");
                    }
                }
                else
                {
                    BestuurderZoekWeergave.ItemsSource = _managers.BestuurderManager.FilterOpBestuurdersNaam(_filterOpNaam); 
                }

                Bestuurderweergave = detailWindow.Bestuurder;
            }
        }
    }
}
