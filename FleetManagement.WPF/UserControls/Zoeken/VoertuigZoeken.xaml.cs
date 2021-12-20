using FleetManagement.Filters;
using FleetManagement.Manager;
using FleetManagement.Model;
using FleetManagement.WPF.DetailWindows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FleetManagement.WPF.UserControls.Zoeken
{
    /// <summary>
    /// Interaction logic for VoertuigZoeken.xaml
    /// </summary>
    public partial class VoertuigZoeken : UserControl
    {
        private readonly Managers _managers;
        //private string _filter; 

        private Voertuig _voertuig;
        private Filter _filter;

        public string PlaceHolderNummerplaatOfChassis { get;} = "Nummerplaat of Chassisnummer";
        public string PlaceHolderAutomodelNaam { get; } = "Merk + Automodel";

        public Voertuig VoertuigWeergave
        {
            get => _voertuig;
            set
            {
                _voertuig = value;
                ZoekWeergaveVoertuig.SelectedItem = value;
            }
        }

        public Filter FilterWeergave
        {
            get => _filter;
            set
            {
                _filter = value;
            }
        }

        public VoertuigZoeken(Managers managers)
        {
            InitializeComponent();
            _managers = managers;
            NummerplaatOfChassisnummer.Text = PlaceHolderNummerplaatOfChassis;
            AutomodelNaam.Text = PlaceHolderAutomodelNaam;

            FilterWeergave = new(
                    new(),
                    new(),
                    new()
                );

            FilterVoertuigDB();
        }
        private void ZoekWeergave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VoertuigWeergave = ZoekWeergaveVoertuig.SelectedItem as Voertuig;
        }

        private void ZoekenMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_voertuig != null)
            {
                VoertuigDetails detailWindow = new(_managers, _voertuig)
                {
                    Owner = Window.GetWindow(this),
                };

                bool? verwijderd = detailWindow.ShowDialog();
                if (verwijderd == true)
                {
                    FilterVoertuigDB();
                }
            }
        }

        private void FilterVoertuigDB()
        {
            string automodelnaam = "";
            if (AutomodelNaam.Text != PlaceHolderAutomodelNaam)
                automodelnaam = AutomodelNaam.Text;

            ZoekWeergaveVoertuig.ItemsSource = _managers.VoertuigManager.GeefAlleVoertuigenFilter(automodelnaam, FilterWeergave);
        } 

        private void FilterOpMerkEnAutomdel_Changed(object sender, TextChangedEventArgs e)
        {
            if (FilterWeergave != null)
                FilterVoertuigDB();
        }

        private void SluitVoertuigForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void KiesFilter_Click(object sender, RoutedEventArgs e)
        {
            infoVoertuigMess.Text = string.Empty;

            FilterWindow FilterWindow = new(_managers, FilterWeergave)
            {
                Owner = Window.GetWindow(this),
            };

            string automodelnaam = "";

            if (AutomodelNaam.Text != PlaceHolderAutomodelNaam)
                automodelnaam = AutomodelNaam.Text;

            bool? geslecteerd = FilterWindow.ShowDialog();
            if (geslecteerd == true)
            {
                KiesFilter.Content = "Filter wijzigen";
            }
            else
            {
                KiesFilter.Content = "Filter invoegen";
            }

            FilterWeergave = FilterWindow.Filter;
            FilterVoertuigDB();
        }

        private void Nummerplaat_GotFocus(object sender, RoutedEventArgs e)
        {
            
            if(NummerplaatOfChassisnummer.Text == PlaceHolderNummerplaatOfChassis)
            {
                NummerplaatOfChassisnummer.Text = string.Empty;
                NummerplaatOfChassisnummer.Foreground = Brushes.Black;
            }
        }

        private void Nummerplaat_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NummerplaatOfChassisnummer.Text))
            {
                NummerplaatOfChassisnummer.Text = PlaceHolderNummerplaatOfChassis;
                NummerplaatOfChassisnummer.Foreground = Brushes.LightSlateGray;
            }
        }

        private void Chassisnummer_GotFocus(object sender, RoutedEventArgs e)
        {
            if(AutomodelNaam.Text == PlaceHolderAutomodelNaam)
            {
                AutomodelNaam.Text = string.Empty;
                AutomodelNaam.Foreground = Brushes.Black;
                ZoekWeergaveVoertuig.SelectedItem = null;
            }
        }

        private void Chassisnummer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AutomodelNaam.Text))
            {
                AutomodelNaam.Text = PlaceHolderAutomodelNaam;
                AutomodelNaam.Foreground = Brushes.LightSlateGray;
            }
        }

        private void ZoekNummerplaatOfChassisnummer_Click(object sender, RoutedEventArgs e)
        {
            Voertuig voertuigDB = _managers.VoertuigManager.ZoekOpNummerplaatOfChassisNummer(NummerplaatOfChassisnummer.Text);
            
            infoVoertuigMess.Text = string.Empty;
            if (voertuigDB != null)
            {
                List<Voertuig> voertuigen = new();
                voertuigen.Add(voertuigDB);
                ZoekWeergaveVoertuig.ItemsSource = voertuigen;
            }
            else
            {
                infoVoertuigMess.Foreground = Brushes.Red;
                infoVoertuigMess.Text = "Geen resultaten gevonden";
            }
        }

        private void KiesDetail_Click(object sender, RoutedEventArgs e)
        {
            if (_voertuig != null)
            {
                VoertuigDetails detailWindow = new(_managers, _voertuig)
                {
                    Owner = Window.GetWindow(this),
                };

                bool? verwijderd = detailWindow.ShowDialog();
                if (verwijderd == true)
                {
                    FilterVoertuigDB();
                    infoVoertuigMess.Foreground = Brushes.Green;
                    infoVoertuigMess.Text = "Voertuig succesvol verwijderd";
                }
            }
        }
    }
}
