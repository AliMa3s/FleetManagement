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
    /// Interaction logic for SelecteerVoertuig.xaml
    /// </summary>
    public partial class SelecteerVoertuig : Window
    {
        private readonly VoertuigManager _manager;
        private Voertuig _gekozenVoertuig;

        public string Placeholder { get; } = "Merk + Automodel";

        public Voertuig GekozenVoertuig
        {
            get => _gekozenVoertuig;
            set
            {
                _gekozenVoertuig = value;
                VoertuigenLijst.SelectedItem = value;
            }
        }

        public SelecteerVoertuig(VoertuigManager VoertuigManager)
        {
            InitializeComponent();
            _manager = VoertuigManager;

            VoertuigenLijst.ItemsSource = _manager.SelecteerZonderBestuurderFilter(ZoekWeergaveVoertuig.Text);
            ZoekWeergaveVoertuig.Text = Placeholder;
        }

        //Automodel bewaren telkens een Model wordt geselecteerd
        private void BewaarVoertuig_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GekozenVoertuig = VoertuigenLijst.SelectedItem as Voertuig;
        }

        private void VoertuigToevoegenDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //voertuig mag niet leeg zijn anders geen terugkeer
            if (GekozenVoertuig != null)
            {
                DialogResult = true;
            }
        }

        private void VoegVoertuigToe_Click(object sender, RoutedEventArgs e)
        {
            //voertuig mag niet leeg zijn anders geen terugkeer
            if (GekozenVoertuig != null)
            {
                DialogResult = true;
            }
        }

        private void TextBoxFilterVoertuig_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(ZoekWeergaveVoertuig.Text != Placeholder)
            {
                VoertuigenLijst.ItemsSource = _manager.SelecteerZonderBestuurderFilter(ZoekWeergaveVoertuig.Text);
            }
        }

        private void ButtonAnnuleer_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TextBoxFilterOpVoertuig_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ZoekWeergaveVoertuig.Text == Placeholder)
            {
                ZoekWeergaveVoertuig.Text = string.Empty;
                ZoekWeergaveVoertuig.Foreground = Brushes.Black;
                VoertuigenLijst.SelectedItem = null;
            }
        }

        private void TextBoxFilterOpVoertuig_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ZoekWeergaveVoertuig.Text))
            {
                ZoekWeergaveVoertuig.Text = Placeholder;
                ZoekWeergaveVoertuig.Foreground = Brushes.LightSlateGray;
            }
        }
    }
}
