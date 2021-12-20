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

namespace FleetManagement.WPF.AanmaakWindows {
    /// <summary>
    /// Interaction logic for UpdateAdres.xaml
    /// </summary>
    public partial class NieuwAdres : Window {

        private Adres _adres;

        public Adres AdresGegevens
        {
            get => _adres;
            set
            {
                _adres = value;
            }
        }

        public NieuwAdres(Adres adres) {

            InitializeComponent();
            _adres = adres;

            if(_adres != null)
            {
                FormAdres.Content = "Update Adres";
                invoegEnUpdateButton.Content = "Updaten";
            }
            else
            {
                FormAdres.Content = "Nieuw Adres";
                invoegEnUpdateButton.Content = "Aanmaken";
            }

            DataContext = AdresGegevens;
        }

        private void AnnuleerForm_Click(object sender, RoutedEventArgs e) {

            AdresGegevens = null;
            DialogResult = false;
        }

        private void InvoegEnUpdateButton_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(StraatTextBox.Text) || !string.IsNullOrWhiteSpace(NummerTextBox.Text)
                || !string.IsNullOrWhiteSpace(PostcodeTextBox.Text)
                || !string.IsNullOrWhiteSpace(GemeenteTextBox.Text))
            {


                AdresGegevens = new Adres(
                    string.IsNullOrWhiteSpace(StraatTextBox.Text.Trim()) ? "" : StraatTextBox.Text.Trim(),
                    string.IsNullOrWhiteSpace(NummerTextBox.Text.Trim()) ? "" : NummerTextBox.Text.Trim(),
                    string.IsNullOrWhiteSpace(PostcodeTextBox.Text.Trim()) ? "" : PostcodeTextBox.Text.Trim(),
                    string.IsNullOrWhiteSpace(GemeenteTextBox.Text.Trim()) ? "" : GemeenteTextBox.Text.Trim()
                ); ;

                DialogResult = true;
            }
            else
            {
                AdresGegevens = null;
                DialogResult = false;
            }

        }
    }
}
