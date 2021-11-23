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
        TankKaart tankkaart;

         
        private List<string> _optieBrandstoffen = new();
        private List<string> _keuzeBrandstoffen = new();

        public TankkaartToevoegen(Managers managers)
        {
            InitializeComponent();
            _managers = managers;
            FormTankkaart.Content = "Tankkaart ingeven";

            //Zodra Brandstofanager werkt schakel deze dan in en all de add's uit
            //_optieBrandstoffen.AddRange(_managers.BrandstofManager.brabdstoffen);
            _optieBrandstoffen.Add("Selecteer");
            _optieBrandstoffen.Add("Diesel");
            _optieBrandstoffen.Add("Gas");
            _optieBrandstoffen.Add("Benzine");
            _optieBrandstoffen.Add("Elektrich");

            _optieBrandstoffen.ForEach(brandstof => {

                BrandstofNamenComboBox.Items.Add(brandstof);
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
                tankkaart = new TankKaart(TankKaartTextBox.Text, DateTime.Parse(GeldigheidsDatumDatePicker.Text));

                if(!string.IsNullOrWhiteSpace(PincodeTextBox.Text))
                    tankkaart.VoegPincodeToe(PincodeTextBox.Text);


                tankkaart.UitgeefDatum = DateTime.Parse(UitgeefDatumDatePicker.Text);

                //if (TankKaartTextBox.Text == null) infoTankkaartMess.Text = "TankkaartNummer niet ingevuld";
                //if (PincodeTextBox.Text == null) infoTankkaartMess.Text = "Pincode niet ingevuld";
                //if (UitgeefDatumDatePicker.SelectedDate == null) infoTankkaartMess.Text = "UitgeefDatum niet ingevuld";
                //if (GeldigheidsDatumDatePicker.SelectedDate == null) infoTankkaartMess.Text = "geldigheidsDatum niet ingevuld";
                //else

                _managers.TankkaartManager.VoegTankKaartToe(tankkaart); //deze lijn code is fout.
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

        }

        private void SluitTankKaartWindow_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void BrandstofToevoegenButton_Click(object sender, RoutedEventArgs e)
        {
            _keuzeBrandstoffen.Add(BrandstofNamenComboBox.SelectedItem.ToString());
            BrandstofNamenComboBox.Items.Remove(BrandstofNamenComboBox.SelectedItem.ToString());
            BrandstofNamenComboBox.SelectedIndex = 0;

            GekozenbrandstoffenString.Text = string.Join(", ", _keuzeBrandstoffen);
        }

        private void ResetGekozenBrandstofButton_Click(object sender, RoutedEventArgs e)
        {
            BrandstofNamenComboBox.Items.Clear();

            _optieBrandstoffen.ForEach(brandstof => {

                BrandstofNamenComboBox.Items.Add(brandstof);
            });

            BrandstofNamenComboBox.SelectedIndex = 0;
            GekozenbrandstoffenString.Text = string.Empty;

            _keuzeBrandstoffen = new();
        }
    }
}
