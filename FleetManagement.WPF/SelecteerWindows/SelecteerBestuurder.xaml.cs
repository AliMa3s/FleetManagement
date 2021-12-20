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
    /// Interaction logic for SelecteerBestuurder.xaml
    /// </summary>
    public partial class SelecteerBestuurder : Window
    {
        private readonly BestuurderManager _bestuurderManager;
        private readonly string _selector;
        private Bestuurder _bestuurder;

        public string PlaceholderName { get; } = "Achternaam + Voornaam";

        public Bestuurder Bestuurder
        {
            get => _bestuurder;
            set
            {
                _bestuurder = value;
                BestuurdersLijst.SelectedItem = value;
            }
        }

        public SelecteerBestuurder(BestuurderManager manager, string selector)
        {
            InitializeComponent();
            _bestuurderManager = manager;
            _selector = selector;

            if (_selector == "voertuig") { Title = "Bestuurders zonder voertuig"; }
            else { Title = "Bestuurders zonder tankkaart"; }
            
            SelecteerBestuurders(_selector);
            TextBoxFilterOpNaam.Text = PlaceholderName;
        }

        //Bestuurder bewaren telkens een Bestuurder wordt geselecteerd
        private void BewaarBestuurder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bestuurder = BestuurdersLijst.SelectedItem as Bestuurder;
        }

        //Ga terug wanneer een Bestuurder is gekozen
        private void ButtonKiesToevoegen_Click(object sender, RoutedEventArgs e)
        {
            //_bestuurder mag niet leeg zijn anders geen terugkeer
            if (_bestuurder != null)
            {
                DialogResult = true;
            }
        }
        
        //Ga terug bij dubbelklik op een rij in de lijst 
        private void BestuurderToevoegenDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //_bestuurder mag niet leeg zijn anders geen terugkeer
            if (_bestuurder != null)
            {
                DialogResult = true;
            }
        }

        private void ButtonAnnuleer_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        //Filteren van naam
        private void TextBoxFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            SelecteerBestuurders(_selector);
        }

        private void SelecteerBestuurders(string selector)
        {
            switch (selector)
            {
                case "voertuig":
                    if (TextBoxFilterOpNaam.Text != PlaceholderName)
                    {
                        BestuurdersLijst.ItemsSource = _bestuurderManager.SelecteerBestuurdersZonderVoertuig(TextBoxFilterOpNaam.Text);
                    }
                    break;

                case "tankkaart":
                    if (TextBoxFilterOpNaam.Text != PlaceholderName)
                    {
                        BestuurdersLijst.ItemsSource = _bestuurderManager.SelecteerBestuurdersZondertankkaart(TextBoxFilterOpNaam.Text);
                    }
                    break;
                default:
                    break;
            }
        }

        private void TextBoxFilterOpNaam_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxFilterOpNaam.Text == PlaceholderName)
            {
                TextBoxFilterOpNaam.Text = string.Empty;
                TextBoxFilterOpNaam.Foreground = Brushes.Black;
                BestuurdersLijst.SelectedItem = null;
            }
        }

        private void TextBoxFilterOpNaam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxFilterOpNaam.Text))
            {
                TextBoxFilterOpNaam.Text = PlaceholderName;
                TextBoxFilterOpNaam.Foreground = Brushes.LightSlateGray;
            }
        }
    }
}
