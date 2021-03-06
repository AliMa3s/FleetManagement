using FleetManagement.Manager;
using FleetManagement.Model;
using FleetManagement.WPF.AanmaakWindows;
using FleetManagement.WPF.SelecteerWindows;
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
        private Voertuig _gekozenVoertuig;

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
                TankkaartSelecteren.Visibility = Visibility.Hidden;
                TankkaartAnnuleren.Visibility = Visibility.Visible;
            }
        }

        private void AdresInvoegen_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            infoBestuurderMess.Text = string.Empty;

            NieuwAdres UpdateAdres = new(_ingevoegdAdres)
            {
                Owner = Window.GetWindow(this),
            };

            bool? geslecteerd = UpdateAdres.ShowDialog();
            if (geslecteerd == true)
            {
                _ingevoegdAdres = UpdateAdres.AdresGegevens;

                GekozenAdresText.Text = _ingevoegdAdres.Straat
                    + " " + _ingevoegdAdres.Nr
                    + " " + _ingevoegdAdres.Postcode
                    + " " + _ingevoegdAdres.Gemeente;
                AdresInvoegen.Content = "Adres wijzigen";
            }
            else
            {
                if(UpdateAdres.AdresGegevens == null)
                {
                    _ingevoegdAdres = UpdateAdres.AdresGegevens;
                    AdresInvoegen.Content = "Adres ingeven";
                    GekozenAdresText.Text = string.Empty;
                }
                else
                {
                    UpdateAdres.AdresGegevens = _ingevoegdAdres;
                }
            }
        }

        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e) {
            infoBestuurderMess.Text = string.Empty;
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
            GekozenAdresText.Text = string.Empty;
            GekozenTankkaartText.Text = string.Empty;
            GekozenVoertuigText.Text = string.Empty;
            AdresInvoegen.Content = "Adres ingeven";

            VoertuigSelecteren.Visibility = Visibility.Visible;
            VoertuigAnnuleren.Visibility = Visibility.Hidden;
            TankkaartSelecteren.Visibility = Visibility.Visible;
            TankkaartAnnuleren.Visibility = Visibility.Hidden;

            _ingevoegdAdres = null;
            _gekozenTankkaart = null;
            _gekozenVoertuig = null;
        }

        private void BestuurderAanmakenButton_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            infoBestuurderMess.Text = string.Empty;

            try
            {
                string geboortedatum = Geboortejaar.Text.PadLeft(4, '0') + "-" 
                    + Geboortemaand.Text.PadLeft(2, '0') + "-" 
                    + Geboortedag.Text.PadLeft(2, '0');

                string voornaam = char.ToUpper(Voornaam.Text.Trim()[0]) + Voornaam.Text.Trim().ToLower()[1..];
                string achternaam = char.ToUpper(Achternaam.Text.Trim()[0]) + Achternaam.Text.Trim().ToLower()[1..];

                Bestuurder nieuwBestuurder = new(
                    voornaam,
                    achternaam,
                    geboortedatum,
                    RijBewijs.Text.Trim().ToUpper(),
                    RijksRegisterNummer.Text.Trim()
                );

                if (_ingevoegdAdres != null)
                {
                    nieuwBestuurder.Adres = _ingevoegdAdres;
                }

                //Update voertuig
                if (_gekozenVoertuig != null)
                {
                    nieuwBestuurder.VoegVoertuigToe(_gekozenVoertuig);
                }

                Bestuurder bestuurderDB = _managers.BestuurderManager.VoegBestuurderToe(nieuwBestuurder);

                if (_gekozenVoertuig != null)
                {   
                    infoBestuurderMess.Text = "Bestuurder succesvol toegevoegd, Voertuig succesvol aan bestuurder gelinkt";
                }
                else
                {
                    infoBestuurderMess.Text = "Bestuurder succesvol toegevoegd";
                }

                infoBestuurderMess.Foreground = Brushes.Green;
                
                //Update tankkaart
                if (_gekozenTankkaart != null)
                {
                    bestuurderDB.VoegTankKaartToe(_gekozenTankkaart);
                    _managers.TankkaartManager.UpdateTankKaart(bestuurderDB.Tankkaart);

                    infoBestuurderMess.Foreground = Brushes.Green;
                    infoBestuurderMess.Text += ", tankkaart aan bestuurder gelinkt";
                }

                ResetForm();
            }
            catch (Exception ex)
            {
                if (_gekozenVoertuig != null)
                {
                    if (_gekozenVoertuig.HeeftVoertuigBestuurder)
                        _gekozenVoertuig.VerwijderBestuurder(_gekozenVoertuig.Bestuurder);
                }

                infoBestuurderMess.Foreground = Brushes.Red;
                infoBestuurderMess.Text = ex.Message;
            }
        }

        private void VoertuigSelecteren_Click(object sender, RoutedEventArgs e)
        {
            SelecteerVoertuig selecteerVoertuig = new(_managers.VoertuigManager)
            {
                Owner = Window.GetWindow(this),
                GekozenVoertuig = _gekozenVoertuig
            };

            bool? geslecteerd = selecteerVoertuig.ShowDialog();
            if (geslecteerd == true)
            {
                _gekozenVoertuig = selecteerVoertuig.GekozenVoertuig;
                GekozenVoertuigText.Text = _gekozenVoertuig.VoertuigNaam;
                VoertuigSelecteren.Visibility = Visibility.Hidden;
                VoertuigAnnuleren.Visibility = Visibility.Visible;
            }
        }

        private void TankkaartAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            _gekozenTankkaart = null;
            GekozenTankkaartText.Text = string.Empty;
            TankkaartSelecteren.Visibility = Visibility.Visible;
            TankkaartAnnuleren.Visibility = Visibility.Hidden;
        }

        private void VoertuigAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            _gekozenVoertuig = null;
            GekozenVoertuigText.Text = string.Empty;
            VoertuigSelecteren.Visibility = Visibility.Visible;
            VoertuigAnnuleren.Visibility = Visibility.Hidden;
        }
    }
}
