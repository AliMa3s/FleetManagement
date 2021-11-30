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
        private Adres _ingevoegdAdres;
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
            SelecteerTankkaart selecteerTankkaart = new(_managers.TankkaartManager)
            {
                Owner = Window.GetWindow(this),
                Tankkaart = _gekozenTankkaart
            };

            bool? geslecteerd = selecteerTankkaart.ShowDialog();
            if (geslecteerd == true) {
                _gekozenTankkaart = selecteerTankkaart.Tankkaart;
                GekozenTankkaartText.Text = _gekozenTankkaart.TankKaartNummer;
                TankkaarSelecteren.Content = "Tankkaart wijzigen";
            }
        }

        private void AdresInvoegen_Click(object sender, RoutedEventArgs e)
        {
            UpdateAdres UpdateAdres = new(_ingevoegdAdres)
            {
                Owner = Window.GetWindow(this),
                AdresGegevens = _ingevoegdAdres
            };

            bool? geslecteerd = UpdateAdres.ShowDialog();
            if (geslecteerd == true)
            {
                _ingevoegdAdres = UpdateAdres.AdresGegevens;

                GekozenAdresText.Text = _ingevoegdAdres.Straat
                    + " " + _ingevoegdAdres.Nr
                    + " " + _ingevoegdAdres.Postcode
                    + " " + _ingevoegdAdres.Gemeente;
            }
            else
            {
                _ingevoegdAdres = UpdateAdres.AdresGegevens;
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
            GekozenTankkaartText.Text = string.Empty;
            AdresInvoegen.Content = "Adres ingeven";
            TankkaarSelecteren.Content = "Tankkaart Selecteren";

            _ingevoegdAdres = null;
            _gekozenTankkaart = null;
        }

        private void BestuurderAanmakenButton_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            infoBestuurderMess.Text = string.Empty;

            try
            {
                string geboortedatum = Geboortejaar.Text + "-" + Geboortemaand.Text + "-" + Geboortedag.Text;

                Bestuurder nieuwBestuurder = new(
                    Voornaam.Text,
                    Achternaam.Text,
                    geboortedatum,
                    RijBewijs.Text,
                    RijBewijsNummer.Text,
                    RijksRegisterNummer.Text
                );

                if (_ingevoegdAdres != null)
                {
                    nieuwBestuurder.Adres = _ingevoegdAdres;
                }

                _managers.BestuurderManager.VoegBestuurderToe(nieuwBestuurder);

                infoBestuurderMess.Foreground = Brushes.Green;
                infoBestuurderMess.Text = "Bestuurder succesvol toegevoegd";

                //Update tankkaart. Belangrijk is dat tankkaart eerst wordt toevoegoegd in Bestuurder, zodat Tankkaart ook bestuurder herkent
                if (_gekozenTankkaart != null)
                {
                    nieuwBestuurder.VoegTankKaartToe(_gekozenTankkaart);
                    _managers.TankkaartManager.UpdateTankKaart(nieuwBestuurder.Tankkaart);

                    infoBestuurderMess.Foreground = Brushes.Green;
                    infoBestuurderMess.Text = "Bestuurder succesvol toegevoegd & Tankkaart aan bestuurder gelinkt";
                }
            }
            catch (Exception ex)
            {
                infoBestuurderMess.Foreground = Brushes.Red;
                infoBestuurderMess.Text = ex.Message;
            }
        }
    }
}
