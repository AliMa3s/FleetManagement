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

        //Veld code behind
        private Bestuurder GekozenBestuurder { get; set; }

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
            //Wis bij elke nieuw poging de message info
            infoTankkaartMess.Text = string.Empty;

            try
            {
                //Ctor verwacht een datum, om zelf een foutmelding te geven checken we dit
                if (DateTime.TryParse(GeldigheidsDatumDatePicker.SelectedDate?.ToString(), out DateTime geldigheidsDatum)
                    && DateTime.TryParse(UitgeefDatumDatePicker.SelectedDate?.ToString(), out DateTime uitgeefDatum))
                {
                    TankKaart tankkaart = new TankKaart(TankKaartTextBox.Text, geldigheidsDatum) 
                    { 
                         UitgeefDatum = uitgeefDatum
                    };

                    //Pincode is niet verplicht
                    if(!string.IsNullOrWhiteSpace(PincodeTextBox.Text))
                        tankkaart.VoegPincodeToe(PincodeTextBox.Text);

                    //Wanneer een bestuurder is geselecteerd toevoegen
                    if(GekozenBestuurder != null)
                    {
                        tankkaart.VoegBestuurderToe(GekozenBestuurder);
                    }

                    //Wanneer brandstof is ingegeven
                    if (_keuzeBrandstoffen.Count > 0)
                    {
                        _keuzeBrandstoffen.ForEach(naam =>
                        {
                            //Haal ID op via manager
                            BrandstofType brandstofType = _managers.Brandstoffen.ToList().Find(e => e.BrandstofNaam == naam);
                            if (brandstofType != null)
                            {
                                //Conroleren of deze al niet in de lijst staat en dan plaatsen
                                if (tankkaart.IsBrandstofAanwezig(brandstofType))
                                {
                                    tankkaart.VoegBrandstofToe(brandstofType);
                                };
                            }
                        });
                    }

#warning Wie is verantwoordelijk voor het toevoegen van de brandstof? Mag datalaag dat doen via VoegTankaart?
                    //Kaart kan toegevoegd worden
                    _managers.TankkaartManager.VoegTankKaartToe(tankkaart);
                    ResetVelden();
                    infoTankkaartMess.Text = "Tankkaart succesvol toegevoegd";
                    infoTankkaartMess.Foreground = Brushes.Green;
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
            } 
        }

        private void ResetVeldenButton_Click(object sender, RoutedEventArgs e)
        {
            ResetVelden();
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

        private void ResetVelden()
        {
            UitgeefDatumDatePicker.SelectedDate = null;
            TankKaartTextBox.Text = null;
            GeldigheidsDatumDatePicker.SelectedDate = null;
            PincodeTextBox.Text = null;
            ResestDropown();

            infoTankkaartMess.Text = string.Empty;
        }
    }
}
