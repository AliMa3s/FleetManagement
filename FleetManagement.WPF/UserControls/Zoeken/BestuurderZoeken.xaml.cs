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
    /// Interaction logic for BestuurderZoeken.xaml
    /// </summary>
    public partial class BestuurderZoeken : UserControl
    {
        private readonly Managers _managers;

        public string PlaceholderName { get; } = "Achternaam + Voornaam";

        public BestuurderZoeken(Managers managers)
        {
            InitializeComponent();
            _managers = managers;

            FilterOpNaam.Text = "Achternaam + Voornaam";
        }

        private void ZoekWeergave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ZoekenMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void ZoekRijksregister_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FilterOpNaam_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FilterOpNaam_GotFocus(object sender, RoutedEventArgs e)
        {
           if(FilterOpNaam.Text == PlaceholderName)
            {
                FilterOpNaam.Text = string.Empty;
                FilterOpNaam.Foreground = Brushes.Black;
            }
        }

        private void FiliterOpNaam_LostFocus(object sender, RoutedEventArgs e)
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
    }
}
