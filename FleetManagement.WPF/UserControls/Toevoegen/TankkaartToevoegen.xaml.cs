using FleetManagement.Manager;
using FleetManagement.ManagerExceptions;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FleetManagement.WPF.UserControls.Toevoegen
{
    /// <summary>
    /// Interaction logic for TankkaartToevoegen.xaml
    /// </summary>
    public partial class TankkaartToevoegen : UserControl
    {
        private readonly Managers _managers;
        private List<string> _keuzeBrandstoffen = new();

        public string DisplayFirst { get; set; } = "Selecteer";

        public TankkaartToevoegen(Managers managers)
        {
            InitializeComponent();
            _managers = managers;
            FormTankkaart.Content = "Tankkaart ingeven";

            BrandstofNamenComboBox.Items.Add(DisplayFirst);
            _managers.Brandstoffen.ToList().ForEach(brandstof => {

                BrandstofNamenComboBox.Items.Add(brandstof.BrandstofNaam);
            });
        }

        private void SluitTankkaartForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void TankkaartAanmakenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DateTime.TryParse(GeldigheidsDatumDatePicker.SelectedDate?.ToString(), out DateTime geldigheidsDatum)
                    && DateTime.TryParse(UitgeefDatumDatePicker.SelectedDate?.ToString(), out DateTime uitgeefDatum))
                {
                    TankKaart tankkaart = new TankKaart(TankKaartTextBox.Text, DateTime.Parse(GeldigheidsDatumDatePicker.Text));

                    if(!string.IsNullOrWhiteSpace(PincodeTextBox.Text))
                        tankkaart.VoegPincodeToe(PincodeTextBox.Text);

                    tankkaart.UitgeefDatum = DateTime.Parse(UitgeefDatumDatePicker.Text);

                    _managers.TankkaartManager.VoegTankKaartToe(tankkaart); //deze lijn code is fout.          
                }
                else
                {
                    infoTankkaartMess.Foreground = Brushes.Red;
                    infoTankkaartMess.Text = "UitgeefDatum -en Geldigheidsdatum moet ingevuld zijn";
                }
            }
            catch(Exception ex)
            {
                infoTankkaartMess.Foreground = Brushes.Red;
                infoTankkaartMess.Text = ex.Message;
                //throw new TankKaartManagerException("aanmaken van nieuwe tankkaart gefaald",ex);
            } 
        }

        private void ResetVeldenButton_Click(object sender, RoutedEventArgs e)
        {
            UitgeefDatumDatePicker.SelectedDate = null;
            TankKaartTextBox.Text = null;
            GeldigheidsDatumDatePicker.SelectedDate = null;
            PincodeTextBox.Text = null;
            ResestDropown();
        }

        private void SluitTankKaartWindow_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void BrandstofToevoegenButton_Click(object sender, RoutedEventArgs e)
        {
            if(BrandstofNamenComboBox.SelectedItem.ToString() != DisplayFirst)
            {
                _keuzeBrandstoffen.Add(BrandstofNamenComboBox.SelectedItem.ToString());
                BrandstofNamenComboBox.Items.Remove(BrandstofNamenComboBox.SelectedItem.ToString());
                BrandstofNamenComboBox.SelectedIndex = 0;
            }

            GekozenbrandstoffenString.Text = string.Join(", ", _keuzeBrandstoffen);

            if(_keuzeBrandstoffen.Count > 0)
            {
                ResetGekozenBrandstofButton.Visibility = Visibility.Visible;
            }
        }

        private void ResetGekozenBrandstofButton_Click(object sender, RoutedEventArgs e)
        {
            ResestDropown();
        }

        private void ResestDropown()
        {
            BrandstofNamenComboBox.Items.Clear();

            BrandstofNamenComboBox.Items.Add(DisplayFirst);
            _managers.Brandstoffen.ToList().ForEach(brandstof => {

                BrandstofNamenComboBox.Items.Add(brandstof.BrandstofNaam);
            });

            BrandstofNamenComboBox.SelectedIndex = 0;
            GekozenbrandstoffenString.Text = string.Empty;

            _keuzeBrandstoffen = new();
            ResetGekozenBrandstofButton.Visibility = Visibility.Hidden;
        }
    }
}
