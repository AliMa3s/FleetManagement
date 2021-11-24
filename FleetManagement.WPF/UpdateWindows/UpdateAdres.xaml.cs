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

namespace FleetManagement.WPF.UpdateWindows {
    /// <summary>
    /// Interaction logic for UpdateAdres.xaml
    /// </summary>
    public partial class UpdateAdres : Window {

        private Adres _adres;

        public Adres AdresGegevens
        {
            get => _adres;
            set
            {
                _adres = value;
            }
        }

        public UpdateAdres(Adres adres) {

            InitializeComponent();
            _adres = adres;

            if(_adres != null)
            {
                FormAdres.Content = "Update Adres";
                invoegEnUpdateButton.Content = "Updaten";
            }
            else
            {
                FormAdres.Content = "Nieuw Adres ingeven";
                invoegEnUpdateButton.Content = "Aanmaken";
            }

            DataContext = AdresGegevens;
        }

        private void SluitForm_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        private void InvoegEnUpdateButton(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(StraatTextBox.Text) || !string.IsNullOrWhiteSpace(NummerTextBox.Text) 
                || !string.IsNullOrWhiteSpace(PostcodeTextBox.Text)
                || !string.IsNullOrWhiteSpace(GemeenteTextBox.Text))
            {
                _adres = new Adres(
                    StraatTextBox.Text ?? "",
                    NummerTextBox.Text ?? "",
                    PostcodeTextBox.Text ?? "",
                    GemeenteTextBox.Text ?? ""
                );

                DialogResult = true;
            }
            else
            {
                DialogResult = false;
            }
        }
    }
}
