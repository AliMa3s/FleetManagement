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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FleetManagement.WPF.UserControls.Zoeken
{
    /// <summary>
    /// Interaction logic for VoertuigZoeken.xaml
    /// </summary>
    public partial class VoertuigZoeken : UserControl
    {
        public string PlaceHolder { get;} = "Nummerplaat of Chassisnummer";
        public string PlaceHolderChassisnummer { get; } = "Chassisnummer";
        private readonly Managers _managers;

        //private string _filter; 

        public VoertuigZoeken(Managers managers)
        {
            InitializeComponent();
            _managers = managers;
            Nummerplaat.Text = "Nummerplaat of Chassisnummer";
            Chassisnummer.Text = "Chassisnummer";
        }
        private void ZoekWeergave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ZoekenMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void TextBoxFilterTextChanged(object sender, TextChangedEventArgs e)
        {

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
            
            if(Nummerplaat.Text == PlaceHolder)
            {
                Nummerplaat.Text = string.Empty;
                Nummerplaat.Foreground = Brushes.LightSlateGray;
            }
        }

        private void Nummerplaat_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Nummerplaat.Text))
            {
                Nummerplaat.Text = PlaceHolder;
                Nummerplaat.Foreground = Brushes.LightSlateGray;
            }
        }

        private void Chassisnummer_GotFocus(object sender, RoutedEventArgs e)
        {
            if(Chassisnummer.Text == PlaceHolderChassisnummer)
            {
                Chassisnummer.Text = string.Empty;
                Chassisnummer.Foreground = Brushes.LightSlateGray;
            }
        }

        private void Chassisnummer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Chassisnummer.Text))
            {
                Chassisnummer.Text = PlaceHolderChassisnummer;
                Chassisnummer.Foreground = Brushes.LightSlateGray;
            }
        }
    }
}
