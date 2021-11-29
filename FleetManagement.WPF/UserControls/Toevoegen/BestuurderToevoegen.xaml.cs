using FleetManagement.Manager;
using FleetManagement.Model;
using FleetManagement.WPF.SelecteerWindows;
using FleetManagement.WPF.UpdateWindows;
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

namespace FleetManagement.WPF.UserControls.Toevoegen
{
    /// <summary>
    /// Interaction logic for BestuurderToevoegen.xaml
    /// </summary>
    public partial class BestuurderToevoegen : UserControl
    {
        private readonly Managers _managers;

        //VELD !! code behind
        private Adres _gekozenAdres;

        private TankKaart _gekozenTankkaart;

        public BestuurderToevoegen(Managers managers)
        {
            InitializeComponent();
            _managers = managers;
            FormBestuurder.Content = "Bestuurder aanmaken";
        }

        private void SluitBestuurderForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void KiesTankkaart_Click(object sender, RoutedEventArgs e) {
            SelecteerTankkaart selecteerTankkaart = new(_managers.TankkaartManager) {
                Owner = Window.GetWindow(this)
            };

            bool? geslecteerd = selecteerTankkaart.ShowDialog();
            if (geslecteerd == true) {
                //GekozenBestuurder = selecteerBestuurder.Bestuurder;
                //GekozenBestuurderNaam.Text = GekozenBestuurder.Achternaam + " " + GekozenBestuurder.Voornaam;
                //KiesBestuurder.Content = "Bestuurder wijzigen";
            }
        }

        private void AdresInvoegen_Click(object sender, RoutedEventArgs e)
        {
            UpdateAdres UpdateAdres = new(_gekozenAdres)
            {
                Owner = Window.GetWindow(this),
                AdresGegevens = _gekozenAdres
            };

            bool? geslecteerd = UpdateAdres.ShowDialog();
            if (geslecteerd == true)
            {
                _gekozenAdres = UpdateAdres.AdresGegevens;

                GekozenAdresText.Text = _gekozenAdres.Straat
                    + " " + _gekozenAdres.Nr
                    + " " + _gekozenAdres.Postcode
                    + " " + _gekozenAdres.Gemeente;
            }
            else
            {
                _gekozenAdres = UpdateAdres.AdresGegevens;
                GekozenAdresText.Text = string.Empty;
            }
        }

        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e) {
            ResetForm();
        }

        //reset Formulier
        private void ResetForm() {
            Voornaam.Text = string.Empty;
            Achternaam.Text = string.Empty;
            Geboortedag.Text = string.Empty;
            Geboortemaand.Text = string.Empty;
            Geboortejaar.Text= string.Empty;
            RijksRegisterNummer.Text = string.Empty;
            RijBewijs.Text = string.Empty;
            RijBewijsNummer.Text = string.Empty;
            GekozenAdresText.Text = string.Empty;
            AdresInvoegen.Content = "Adres Invoegen";
            TankkaarSelecteren.Content = "Tankkaart Selecteren";

            _gekozenAdres = null;
            _gekozenTankkaart = null;
        }

        private void BestuurderAanmakenButton_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            infoBestuurderMess.Text = string.Empty;
        }
    }
}
