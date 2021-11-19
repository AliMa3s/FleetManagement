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
        private readonly BestuurderManager _bestuurderManagers;
        private Bestuurder _bestuurder;
        
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
            _bestuurderManagers = manager;

            BestuurdersLijst.ItemsSource = _bestuurderManagers.FilterOpBestuurdersNaam("", true);
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
            BestuurdersLijst.ItemsSource = _bestuurderManagers.FilterOpBestuurdersNaam(TextBoxFilterOpNaam.Text, true);
        }
    }
}
