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

namespace FleetManagement.WPF.UpdateWindows
{
    /// <summary>
    /// Interaction logic for UpdateTankkaart.xaml
    /// </summary>
    public partial class UpdateTankkaart : Window
    {
        private readonly Managers _managers;
        private List<string> _keuzeBrandstoffen;

        private Bestuurder GekozenBestuurder { get; set; }

        public string DisplayFirst { get; set; } = "Selecteer";

        private TankKaart _tankaart;

        public TankKaart TankkaartDetail
        {
            get => _tankaart;
            set
            {
                _tankaart = value;
            }
        }

        public UpdateTankkaart(Managers managers, TankKaart tankkaart)
        {
            InitializeComponent();
            _managers = managers;
            _tankaart = tankkaart;

            BrandstofNamenComboBox.Items.Add(DisplayFirst);

            _managers.Brandstoffen.ToList().ForEach(brandstof => {

                if (!_tankaart.IsBrandstofAanwezig(brandstof))
                {
                    BrandstofNamenComboBox.Items.Add(brandstof.BrandstofNaam);
                }
            });

            if (TankkaartDetail.HeeftTankKaartBestuurder)
            {
                StringBuilder stringBuilder = new("Naam: " + TankkaartDetail.Bestuurder.Achternaam);
                stringBuilder.Append(" " + TankkaartDetail.Bestuurder.Voornaam);
                stringBuilder.AppendLine(Environment.NewLine + "Rijksregister: " + TankkaartDetail.Bestuurder.RijksRegisterNummer);
                TankKaartTextBox.Text = stringBuilder.ToString();
            }


            DataContext = TankkaartDetail;
        }

        private void TankKaartUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ResetVeldenButton_Click(object sender, RoutedEventArgs e)
        {
            ResetVelden();
        }

        private void ResestDropown() {
            BrandstofNamenComboBox.Items.Clear();

            BrandstofNamenComboBox.Items.Add(DisplayFirst);
            _managers.Brandstoffen.ToList().ForEach(brandstof => {

                BrandstofNamenComboBox.Items.Add(brandstof.BrandstofNaam);

            });

            BrandstofNamenComboBox.SelectedIndex = 0;
            GekozenbrandstoffenString.Text = "Geen brandstoffen";

            _keuzeBrandstoffen = new();
            ResetGekozenBrandstofButton.Visibility = Visibility.Hidden;
        }

        private void ResetVelden() {
            UitgeefDatumDatePicker.SelectedDate = null;
            TankKaartTextBox.Text = null;
            GekozenBestuurder = null;
            GeldigheidsDatumDatePicker.SelectedDate = null;
            PincodeTextBox.Text = string.Empty;
            KiesBestuurder.Visibility = Visibility.Visible;
            AnnuleerBestuurder.Visibility = Visibility.Hidden;
            ResestDropown();

            infoTankkaartMess.Text = string.Empty;
        }

        private void SluitTankKaartWindow_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void ResetGekozenBrandstofButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BrandstofToevoegenButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TankkaartAanmakenButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void KiesBestuurder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AnnuleerBestuurder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UitgeefDatumDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}







