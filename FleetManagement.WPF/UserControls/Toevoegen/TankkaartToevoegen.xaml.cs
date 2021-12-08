using FleetManagement.Manager;
using FleetManagement.ManagerExceptions;
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
                if (DateTime.TryParse(GeldigheidsDatumDatePicker.SelectedDate?.ToString(), out DateTime geldigheidsDatum))
                {
                    DateTime? uitgeefDatum = null;
                    if (DateTime.TryParse(UitgeefDatumDatePicker.SelectedDate?.ToString(), out DateTime datum))
                    {
                        uitgeefDatum = datum;
                    }

                    TankKaart tankkaart = new TankKaart(TankKaartTextBox.Text, geldigheidsDatum) { UitgeefDatum = uitgeefDatum };

                    //Wanneer brandstof is ingegeven
                    if (_keuzeBrandstoffen.Count > 0)
                    {
                        _keuzeBrandstoffen.ForEach(naam =>
                        {
                            //Haal BrandstofType op (met ID) via manager
                            BrandstofType brandstofType = _managers.Brandstoffen.ToList().Find(e => e.BrandstofNaam == naam);

                            if (brandstofType != null)
                            {
                                //Controleer of deze al niet in de lijst staat en dan toevoegen
                                if (!tankkaart.IsBrandstofAanwezig(brandstofType))
                                {
                                    tankkaart.VoegBrandstofToe(brandstofType);
                                };
                            }
                        });
                    }

                    if(tankkaart.IsGeldigheidsDatumVervallen || (uitgeefDatum.HasValue && uitgeefDatum > tankkaart.GeldigheidsDatum))
                    {
                        infoTankkaartMess.Text = "Kan niet toevoegen want tankkaart is reeds vervallen";
                        infoTankkaartMess.Foreground = Brushes.Red;
                    }
                    else
                    {
                        //Pincode is niet verplicht
                        if (!string.IsNullOrWhiteSpace(PincodeTextBox.Text))
                            tankkaart.VoegPincodeToe(PincodeTextBox.Text);

                        if (GekozenBestuurder != null)
                        {
                            tankkaart.VoegBestuurderToe(GekozenBestuurder);
                        }

                        //Kaart kan toegevoegd worden
                        _managers.TankkaartManager.VoegTankKaartToe(tankkaart);

                        //Moet apart staan voor message te tonen want ResetVelden wist de ingegevenBestuurder
                        if (GekozenBestuurder != null)
                        {
                            ResetVelden();
                            infoTankkaartMess.Text += "Tankkaart succesvol toegevoegd en bestuurder succesvol aan tankkaart gelinkt";
                            infoTankkaartMess.Foreground = Brushes.Green;
                        }
                        else
                        {
                            ResetVelden();
                            infoTankkaartMess.Text = "Tankkaart succesvol toegevoegd";
                            infoTankkaartMess.Foreground = Brushes.Green;
                        }
                    }
                }
                else
                {
                    infoTankkaartMess.Foreground = Brushes.Red;
                    infoTankkaartMess.Text = "Geldigheidsdatum moet ingevuld zijn";
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
                GekozenbrandstoffenString.Text = string.Join(", ", _keuzeBrandstoffen);
            }

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
            GekozenbrandstoffenString.Text = "Geen brandstoffen";

            _keuzeBrandstoffen = new();
            ResetGekozenBrandstofButton.Visibility = Visibility.Hidden;
        }

        private void ResetVelden()
        {
            UitgeefDatumDatePicker.SelectedDate = null;
            TankKaartTextBox.Text = null;
            GekozenBestuurderNaam.Text = string.Empty;
            GekozenBestuurder = null;
            GeldigheidsDatumDatePicker.SelectedDate = null;
            PincodeTextBox.Text = string.Empty;
            KiesBestuurder.Visibility = Visibility.Visible;
            AnnuleerBestuurder.Visibility = Visibility.Hidden;
            ResestDropown();

            infoTankkaartMess.Text = string.Empty;
        }

        private void KiesBestuurder_Click(object sender, RoutedEventArgs e)
        {
            SelecteerBestuurder selecteerBestuurder = new(_managers.BestuurderManager, "tankkaart")
            {
                Owner = Window.GetWindow(this),
                Bestuurder = GekozenBestuurder
            };

            bool? geslecteerd = selecteerBestuurder.ShowDialog();
            if (geslecteerd == true)
            {
                //Wis bij elke nieuw poging de message info
                infoTankkaartMess.Text = string.Empty;

                GekozenBestuurder = selecteerBestuurder.Bestuurder;
                GekozenBestuurderNaam.Text = GekozenBestuurder.Achternaam + " " + GekozenBestuurder.Voornaam;
                KiesBestuurder.Visibility = Visibility.Hidden;
                AnnuleerBestuurder.Visibility = Visibility.Visible;
            }
        }

        private void AnnuleerBestuurder_Click(object sender, RoutedEventArgs e)
        {
            KiesBestuurder.Visibility = Visibility.Visible;
            AnnuleerBestuurder.Visibility = Visibility.Hidden;
            GekozenBestuurderNaam.Text = string.Empty;
            GekozenBestuurder = null;
        }
    }
}
