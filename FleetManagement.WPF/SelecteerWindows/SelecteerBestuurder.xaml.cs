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
        private Bestuurder _bestuurder;

        public string PlaceholderName { get; }

        public Bestuurder Bestuurder
        {
            get => _bestuurder;
            set
            {
                _bestuurder = value;
                BestuurdersLijst.SelectedItem = value;
            }
        }

        public SelecteerBestuurder(BestuurderManager manager)
        {
            InitializeComponent();
            _bestuurderManager = manager;

            BestuurdersLijst.ItemsSource = _bestuurderManager.FilterOpBestuurdersNaam("", true);

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
            BestuurdersLijst.ItemsSource = _bestuurderManager.FilterOpBestuurdersNaam(TextBoxFilterOpNaam.Text, true);
        }

        private void FilterOpNaam_GotFus(object sender, RoutedEventArgs e)
        {
            if (TextBoxFilterOpNaam.Text == PlaceholderName)
            {
                TextBoxFilterOpNaam.Text = "";
                TextBoxFilterOpNaam.Foreground = Brushes.Black;
            }
        }

        private void FiliterOpNaam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxFilterOpNaam.Text))
            {
                TextBoxFilterOpNaam.Text = PlaceholderName;
                TextBoxFilterOpNaam.Foreground = Brushes.LightGray;
            }
        }
    }
}
