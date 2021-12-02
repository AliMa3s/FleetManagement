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

        public VoertuigZoeken(Managers managers)
        {
            InitializeComponent();
            _managers = managers;
            NummerplaatOfChassisnummer.Text = PlaceHolderNummerplaatOfChassis;
            AutomodelNaam.Text = PlaceHolderAutomodelNaam;

            ZoekWeergaveVoertuig.ItemsSource = _managers.VoertuigManager.GeefAlleVoertuigenFilter("");
        }
        private void ZoekWeergave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VoertuigWeergave = ZoekWeergaveVoertuig.SelectedItem as Voertuig;
        }

        private void ZoekenMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void FilterOpMerkEnAutomdel_Changed(object sender, TextChangedEventArgs e)
        {
            if (AutomodelNaam.Text != PlaceHolderAutomodelNaam)
                ZoekWeergaveVoertuig.ItemsSource = _managers.VoertuigManager.GeefAlleVoertuigenFilter(AutomodelNaam.Text);
        }

        private void Verwijderen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SluitVoertuigForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void KiesFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterWindow filterWindow = new(_managers)
            {
                Owner = Window.GetWindow(this),
                //Filter = _filter
            };

            bool? geslecteerd = filterWindow.ShowDialog();
            if (geslecteerd == true)
            {
                //_filter = filterWindow.Filter;

                StringBuilder stringBuilder = new("Filteren op:");

                //if (filterWindow.EnableAutopType)
                //    stringBuilder.AppendLine("Autotypes");
                //if (filterWindow.EnableBrandstof)
                //    stringBuilder.AppendLine("Brandstoffen");
                //if (filterWindow.EnableKleur)
                //    stringBuilder.AppendLine("Kleuren");

                //GekozenFilter.Text = stringBuilder;
                //KiesFilter.Content = "Filter wijzigen";
            }
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

            if(voertuigDB != null)
            {
                List<Voertuig> voertuigen = new();
                voertuigen.Add(voertuigDB);
                ZoekWeergaveVoertuig.ItemsSource = voertuigen;
            }
        }
    }
}
