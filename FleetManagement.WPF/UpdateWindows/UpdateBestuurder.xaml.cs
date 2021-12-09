using FleetManagement.Manager;
using FleetManagement.Model;
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
using System.Windows.Shapes;

namespace FleetManagement.WPF.UpdateWindows
{
    /// <summary>
    /// Interaction logic for UpdateBestuurder.xaml
    /// </summary>
    public partial class UpdateBestuurder : Window
    {
        private readonly Managers _managers;
        
        //Huidige toestand van het object
        private Bestuurder _bestuurder;

        //VELD !! code behind
        private Adres _ingevoegdAdres;
        private TankKaart _gekozenTankkaart;
        private Voertuig _gekozenVoertuig;

        public Bestuurder BestuurderDetail
        {
            get => _bestuurder;
            set
            {
                _bestuurder = value;
            }
        }

        public UpdateBestuurder(Managers managers, Bestuurder bestuurder)
        {
            InitializeComponent();
            _managers = managers;
            _bestuurder = bestuurder;

            DataContext = BestuurderDetail;

            Geboortedag.Text = BestuurderDetail.GeboorteDatum.Substring(8,2);
            Geboortemaand.Text = BestuurderDetail.GeboorteDatum.Substring(5, 2);
            Geboortejaar.Text = BestuurderDetail.GeboorteDatum.Substring(0, 4);

            if (BestuurderDetail.HeeftBestuurderVoertuig)
            {
                StringBuilder stringBuilder = new(BestuurderDetail.Voertuig.AutoModel.Merk + " " + BestuurderDetail.Voertuig.AutoModel.AutoModelNaam);
                stringBuilder.AppendLine(Environment.NewLine + "Chassis: " + BestuurderDetail.Voertuig.ChassisNummer);
                stringBuilder.AppendLine("Nummerplaat: " + BestuurderDetail.Voertuig.NummerPlaat);

                GekozenVoertuigText.Text = stringBuilder.ToString();
                WijzigVoertuig.Content = "Voertuig wijzigen";

                _gekozenVoertuig = BestuurderDetail.Voertuig;

            }

            if (BestuurderDetail.HeeftBestuurderTankKaart)
            {
                StringBuilder stringBuilder = new("Nr: " + BestuurderDetail.Tankkaart.TankKaartNummer);
                stringBuilder.AppendLine(Environment.NewLine + "Geldig tot: " + BestuurderDetail.Tankkaart.GeldigheidsDatum.ToString("dd/MM/yyyy"));

                if (BestuurderDetail.Tankkaart.Actief)
                {
                    stringBuilder.AppendLine("Tankkaart is actief");
                }
                else
                {
                    if (BestuurderDetail.Tankkaart.IsGeldigheidsDatumVervallen) { stringBuilder.AppendLine("Tankkaart is vervallen"); }
                    else { stringBuilder.AppendLine("Tankkaart is geblokkeerd"); }
                }

                GekozenTankkaartText.Text = stringBuilder.ToString();
                WijzigTankkaart.Content = "Tankkaart wijzigen";

                _gekozenTankkaart = BestuurderDetail.Tankkaart;
            }

            if (BestuurderDetail.Adres != null)
            {
                StringBuilder stringBuilder = new(BestuurderDetail.Adres.Straat + " " + BestuurderDetail.Adres.Nr);
                stringBuilder.AppendLine(Environment.NewLine + BestuurderDetail.Adres.Postcode + " " + BestuurderDetail.Adres.Gemeente);

                Adresgegevens.Text = stringBuilder.ToString();
                WijzigAdres.Content = "Adres wijzigen";

                _ingevoegdAdres = BestuurderDetail.Adres;
            }
        }

        private void SluitForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WijzigAdres_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            infoBestuurderMess.Text = string.Empty;

            UpdateAdres UpdateAdres = new(_ingevoegdAdres)
            {
                Owner = Window.GetWindow(this),
            };

            bool? geslecteerd = UpdateAdres.ShowDialog();
            if (geslecteerd == true)
            {
                _ingevoegdAdres = UpdateAdres.AdresGegevens;

                Adresgegevens.Text = _ingevoegdAdres.Straat
                    + " " + _ingevoegdAdres.Nr
                    + " " + _ingevoegdAdres.Postcode
                    + " " + _ingevoegdAdres.Gemeente;
                WijzigAdres.Content = "Adres wijzigen";
            }
            else
            {
                if (UpdateAdres.AdresGegevens == null)
                {
                    _ingevoegdAdres = UpdateAdres.AdresGegevens;
                    WijzigAdres.Content = "Adres ingeven";
                    Adresgegevens.Text = string.Empty;
                }
                else
                {
                    UpdateAdres.AdresGegevens = _ingevoegdAdres;
                }
            }
        }

        private void WijzigVoertuig_Click(object sender, RoutedEventArgs e)
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
                GekozenVoertuigText.Text = _gekozenVoertuig.ChassisNummer;
                WijzigTankkaart.Content = "Voertuig wijzigen";
            }
        }

        private void WijzigTankkaart_Click(object sender, RoutedEventArgs e)
        {
            SelecteerTankkaart selecteerTankkaart = new(_managers.TankkaartManager)
            {
                Owner = Window.GetWindow(this),
                Tankkaart = _gekozenTankkaart
            };

            bool? geslecteerd = selecteerTankkaart.ShowDialog();
            if (geslecteerd == true)
            {
                _gekozenTankkaart = selecteerTankkaart.Tankkaart;
                GekozenTankkaartText.Text = _gekozenTankkaart.TankKaartNummer;
                WijzigTankkaart.Content = "Tankkaart wijzigen";
            }
        }
    }    
}
