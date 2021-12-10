using FleetManagement.Manager;
using FleetManagement.Model;
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
using System.Windows.Shapes;

namespace FleetManagement.WPF.DetailWindows {
    /// <summary>
    /// Interaction logic for BestuurderDetails.xaml
    /// </summary>
    public partial class BestuurderDetails : Window {

        private readonly Managers _managers;

        private Bestuurder _bestuurderDetail;

        public bool? Updatetet { get; set; }

        public BestuurderDetails(Managers managers, Bestuurder bestuurder) {

            InitializeComponent();
            _managers = managers;
            _bestuurderDetail = bestuurder;

            SetDefault();

            //bind de bestuurder
            DataContext = _bestuurderDetail;
        }

        private void SluitForm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SetDefault()
        {
            Updatetet = false;

            if (_bestuurderDetail.HeeftBestuurderVoertuig)
            {
                StringBuilder stringBuilder = new(_bestuurderDetail.Voertuig.AutoModel.Merk + " " + _bestuurderDetail.Voertuig.AutoModel.AutoModelNaam);
                stringBuilder.AppendLine(Environment.NewLine + "Chassis: " + _bestuurderDetail.Voertuig.ChassisNummer);
                stringBuilder.AppendLine("Nummerplaat: " + _bestuurderDetail.Voertuig.NummerPlaat);

                HeeftVoertuig.Text = stringBuilder.ToString();
            }

            if (_bestuurderDetail.HeeftBestuurderTankKaart)
            {
                StringBuilder stringBuilder = new("Nr: " + _bestuurderDetail.Tankkaart.TankKaartNummer);
                stringBuilder.AppendLine(Environment.NewLine + "Geldig tot: " + _bestuurderDetail.Tankkaart.GeldigheidsDatum.ToString("dd/MM/yyyy"));

                if (_bestuurderDetail.Tankkaart.Actief)
                {
                    stringBuilder.AppendLine("Tankkaart is actief");
                }
                else
                {
                    if (_bestuurderDetail.Tankkaart.IsGeldigheidsDatumVervallen) { stringBuilder.AppendLine("Tankkaart is vervallen"); }
                    else { stringBuilder.AppendLine("Tankkaart is geblokkeerd"); }
                }

                HeeftTankkaart.Text = stringBuilder.ToString();
            }

            if (_bestuurderDetail.Adres != null)
            {
                StringBuilder stringBuilder = new(_bestuurderDetail.Adres.Straat + " " + _bestuurderDetail.Adres.Nr);
                stringBuilder.AppendLine(Environment.NewLine + _bestuurderDetail.Adres.Postcode + " " + _bestuurderDetail.Adres.Gemeente);

                Adresgegevens.Text = stringBuilder.ToString();
            }
        }

        private void WijzigButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateBestuurder updateBestuurder = new(_managers, _bestuurderDetail)
            {
                Owner = Window.GetWindow(this),
                BestuurderDetail = _bestuurderDetail
            };

            Updatetet = updateBestuurder.ShowDialog();
            if (Updatetet == true)
            {
                _bestuurderDetail = updateBestuurder.BestuurderDetail;

                //uitzetten
                VoornaamTextBlock.Text = _bestuurderDetail.Voornaam;
                AchternaamTextBlock.Text = _bestuurderDetail.Achternaam;
                GeboorteDatumTextBlock.Text = _bestuurderDetail.GeboorteDatum;
                RijbewijsTextBlock.Text = _bestuurderDetail.TypeRijbewijs;
                RijksRegisterTextBlock.Text = _bestuurderDetail.RijksRegisterNummer;

                SetDefault();
            }
        }

        private void VerwijderButton_Click(object sender, RoutedEventArgs e)
        {
            BevestigingWindow bevestigingWindow = new()
            {
                Owner = Window.GetWindow(this),
            };

            bool? verwijderen = bevestigingWindow.ShowDialog();
            if (verwijderen == true)
            {
                //_managers.BestuurderManager.VerwijderBestuurder(_bestuurderDetail);

                Window.GetWindow(this).Close();

                //Verwijder via manager; bij succes sluit scherm
                //+ update list en verwijder uit de lijst of vraag terug result aan manager
            }
        }
    }
}
