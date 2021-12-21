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
    /// Interaction logic for UpdateTankkaart.xaml
    /// </summary>
    public partial class UpdateTankkaart : Window
    {
        private readonly Managers _managers;
        private List<string> _keuzeBrandstoffen = new();

        private Bestuurder GekozenBestuurder { get; set; }
        public Bestuurder VerwijderdBestuurder { get; private set; }

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
            TankkaartDetail = tankkaart;

            SetDefault();

            if (TankkaartDetail.HeeftTankKaartBestuurder)
            {
                GekozenBestuurder = null;
                KiesBestuurder.Visibility = Visibility.Hidden;
                AnnuleerBestuurder.Visibility = Visibility.Visible;
            }
            else
            {
                KiesBestuurder.Visibility = Visibility.Visible;
                AnnuleerBestuurder.Visibility = Visibility.Hidden;
            }

            DataContext = TankkaartDetail;
        }

        private void SetDefault()
        {
            StringBuilder brandstoffenString = new();
            BrandstofNamenComboBox.Items.Clear(); 
            BrandstofNamenComboBox.SelectedIndex = 0;
            BrandstofNamenComboBox.Items.Add(DisplayFirst);
            _keuzeBrandstoffen.Clear();

            if (_managers.Brandstoffen.Count > 0)
            {
                _managers.Brandstoffen.ToList().ForEach(brandstof => {

                    if (TankkaartDetail.Brandstoffen.Exists(b => b.BrandstofNaam == brandstof.BrandstofNaam))
                    {
                        brandstoffenString.Append(brandstof.BrandstofNaam+ ", ");
                        _keuzeBrandstoffen.Add(brandstof.BrandstofNaam);
                    }
                    else
                    {
                        BrandstofNamenComboBox.Items.Add(brandstof.BrandstofNaam);
                    }
                });

                if (TankkaartDetail.Brandstoffen.Count > 0)
                {
                    Brandstoffen.Text = brandstoffenString.ToString()[0..^2];
                }
                else
                {
                    ResestDropown();
                }

                if (_keuzeBrandstoffen.Count > 0)
                {
                    ResetGekozenBrandstofButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ResetGekozenBrandstofButton.Visibility = Visibility.Hidden;
                }
            }

            if (TankkaartDetail.HeeftTankKaartBestuurder)
            {
                StringBuilder stringBuilder = new("Naam: " + TankkaartDetail.Bestuurder.Achternaam);
                stringBuilder.Append(" " + TankkaartDetail.Bestuurder.Voornaam);
                stringBuilder.AppendLine(Environment.NewLine + "Rijksregister: " + TankkaartDetail.Bestuurder.RijksRegisterNummer.Substring(0, 2) + "."
                    + TankkaartDetail.Bestuurder.RijksRegisterNummer.Substring(2, 2) + "."
                    + TankkaartDetail.Bestuurder.RijksRegisterNummer.Substring(4, 2) + "-"
                    + TankkaartDetail.Bestuurder.RijksRegisterNummer.Substring(6, 3) + "."
                    + TankkaartDetail.Bestuurder.RijksRegisterNummer.Substring(9, 2));
                GekozenBestuurderText.Text = stringBuilder.ToString();
            }
            else
            {
                GekozenBestuurderText.Text = "Geen Bestuurder";
                GekozenBestuurder = null;
            }
        }

        private void TankkaartUpdatenButton_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            infoTankkaartMess.Text = string.Empty;

            try
            {
                if (DateTime.TryParse(GeldigheidsDatumDatePicker.SelectedDate?.ToString(), out DateTime geldigheidsDatum))
                {
                    DateTime? uitgeefDatum = null;
                    if (DateTime.TryParse(UitgeefDatumDatePicker.SelectedDate?.ToString(), out DateTime datum))
                    {
                        uitgeefDatum = datum;
                    }

                    //stap 1: bestuurder verwijderen (indien van toepassing)
                    if(VerwijderdBestuurder != null)
                    {
                        TankkaartDetail.VerwijderBestuurder(VerwijderdBestuurder);                        
                    }

                    TankKaart updateTankkaart = new(TankkaartDetail.TankKaartNummer.Trim(),
                        ActiefJa.IsChecked.HasValue && (bool)ActiefJa.IsChecked,
                        geldigheidsDatum
                    )
                    {
                        UitgeefDatum = uitgeefDatum
                    };

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
                                if (!updateTankkaart.IsBrandstofAanwezig(brandstofType))
                                {
                                    updateTankkaart.VoegBrandstofToe(brandstofType);
                                };
                            }
                        });
                    }

                    if (updateTankkaart.IsGeldigheidsDatumVervallen || (uitgeefDatum.HasValue && uitgeefDatum > updateTankkaart.GeldigheidsDatum))
                    {
                        RolbackBestuurder();
                        infoTankkaartMess.Text = "Kan niet toevoegen want tankkaart is reeds vervallen";
                        infoTankkaartMess.Foreground = Brushes.Red;
                    }
                    else
                    {
                        //Pincode is niet verplicht
                        if (!string.IsNullOrWhiteSpace(PincodeTextBox.Text))
                            updateTankkaart.VoegPincodeToe(PincodeTextBox.Text);

                        //stap 2: kijk of een ander bestuurder is gekozen
                        if(GekozenBestuurder != null)
                        {
                            updateTankkaart.VoegBestuurderToe(GekozenBestuurder);
                        }
                        else
                        {
                            if(TankkaartDetail.HeeftTankKaartBestuurder)
                            {
                                Bestuurder bestaandeBestuurder = new(
                                    TankkaartDetail.Bestuurder.BestuurderId,
                                    TankkaartDetail.Bestuurder.Voornaam,
                                    TankkaartDetail.Bestuurder.Achternaam,
                                    TankkaartDetail.Bestuurder.GeboorteDatum,
                                    TankkaartDetail.Bestuurder.TypeRijbewijs,
                                    TankkaartDetail.Bestuurder.RijksRegisterNummer
                                )
                                {
                                  Adres = TankkaartDetail.Bestuurder.Adres
                                };

                                if (TankkaartDetail.Bestuurder.AanmaakDatum.HasValue)
                                    bestaandeBestuurder.AanmaakDatum = TankkaartDetail.Bestuurder.AanmaakDatum;

                                updateTankkaart.VoegBestuurderToe(bestaandeBestuurder);
                            }
                        }
       
                        //Kaart kan geüpdatet worden
                        if(updateTankkaart.TankKaartNummer == TankKaartNummer.Text)
                        {
                            _managers.TankkaartManager.UpdateTankKaart(updateTankkaart);
                        }
                        else
                        {
                            _managers.TankkaartManager.UpdateTankKaart(updateTankkaart, TankKaartNummer.Text);
                        }

                        TankkaartDetail = updateTankkaart;
                        GekozenBestuurderText = null;
                        DialogResult = true;
                    }
                }
                else
                {
                    infoTankkaartMess.Foreground = Brushes.Red;
                    infoTankkaartMess.Text = "Geldigheidsdatum moet ingevuld zijn";
                }
            }
            catch (Exception ex)
            {
                RolbackBestuurder();
                infoTankkaartMess.Foreground = Brushes.Red;
                infoTankkaartMess.Text = ex.Message;
            }
        }

        private void RolbackBestuurder()
        {
            //Rolback huidig bestuurder (voor reset knop na fouten in update)
            if (VerwijderdBestuurder != null)
            {
                TankkaartDetail.VoegBestuurderToe(VerwijderdBestuurder);
                VerwijderdBestuurder = null;
            }
        }

        private void ResetVeldenButton_Click(object sender, RoutedEventArgs e)
        {
            SetDefault();
            infoTankkaartMess.Text = string.Empty;

            if (TankkaartDetail.UitgeefDatum.HasValue)
            {
                UitgeefDatumDatePicker.SelectedDate = TankkaartDetail.UitgeefDatum.Value;
            }

            TankKaartNummer.Text = TankkaartDetail.TankKaartNummer;
            
            GeldigheidsDatumDatePicker.SelectedDate = TankkaartDetail.GeldigheidsDatum;
            PincodeTextBox.Text = TankkaartDetail.Pincode;

            if(TankkaartDetail.HeeftTankKaartBestuurder)
            {
                GekozenBestuurder = TankkaartDetail.Bestuurder;
                KiesBestuurder.Visibility = Visibility.Hidden;
                AnnuleerBestuurder.Visibility = Visibility.Visible;
            }
            else
            {
                KiesBestuurder.Visibility = Visibility.Visible;
                AnnuleerBestuurder.Visibility = Visibility.Hidden;
            }


            if (TankkaartDetail.Actief)
            {
                ActiefJa.IsChecked = true;
                ActiefNeen.IsChecked = false;
            } 
            else 
            {
                ActiefJa.IsChecked = false;
                ActiefNeen.IsChecked = true;
            }

            GekozenBestuurder = null;
            infoTankkaartMess.Text = string.Empty;
        }

        private void ResestDropown() {

            VerwijderdBestuurder = null;
            BrandstofNamenComboBox.Items.Clear();

            BrandstofNamenComboBox.Items.Add(DisplayFirst);
            _managers.Brandstoffen.ToList().ForEach(brandstof => {

                BrandstofNamenComboBox.Items.Add(brandstof.BrandstofNaam);

            });

            BrandstofNamenComboBox.SelectedIndex = 0;
            Brandstoffen.Text = "Geen brandstoffen";

            _keuzeBrandstoffen = new();
            ResetGekozenBrandstofButton.Visibility = Visibility.Hidden;
        }

        private void SluitTankKaartWindow_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ResetGekozenBrandstofButton_Click(object sender, RoutedEventArgs e)
        {
            ResestDropown();
        }

        private void BrandstofToevoegenButton_Click(object sender, RoutedEventArgs e)
        {
            if (BrandstofNamenComboBox.SelectedItem.ToString() != DisplayFirst)
            {
                _keuzeBrandstoffen.Add(BrandstofNamenComboBox.SelectedItem.ToString());
                BrandstofNamenComboBox.Items.Remove(BrandstofNamenComboBox.SelectedItem.ToString());
                BrandstofNamenComboBox.SelectedIndex = 0;
                Brandstoffen.Text = string.Join(", ", _keuzeBrandstoffen);
            }

            if (_keuzeBrandstoffen.Count > 0)
            {
                ResetGekozenBrandstofButton.Visibility = Visibility.Visible;
            }
        }

        private void TankkaartAanmakenButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
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
                StringBuilder stringBuilder = new("Naam: " + GekozenBestuurder.Achternaam);
                stringBuilder.Append(" " + GekozenBestuurder.Voornaam);
                stringBuilder.AppendLine(Environment.NewLine + "Rijksregister: " + GekozenBestuurder.RijksRegisterNummer.Substring(0, 2) + "."
                    + GekozenBestuurder.RijksRegisterNummer.Substring(2, 2) + "."
                    + GekozenBestuurder.RijksRegisterNummer.Substring(4, 2) + "-"
                    + GekozenBestuurder.RijksRegisterNummer.Substring(6, 3) + "."
                    + GekozenBestuurder.RijksRegisterNummer.Substring(9, 2));
                GekozenBestuurderText.Text = stringBuilder.ToString();
                KiesBestuurder.Visibility = Visibility.Hidden;
                AnnuleerBestuurder.Visibility = Visibility.Visible;
            }
        }

        private void AnnuleerBestuurder_Click(object sender, RoutedEventArgs e)
        {
            GekozenBestuurder = null;
            GekozenBestuurderText.Text = "Geen bestuurder";
            KiesBestuurder.Visibility = Visibility.Visible;
            AnnuleerBestuurder.Visibility = Visibility.Hidden;
            VerwijderdBestuurder = TankkaartDetail.Bestuurder;
        }
    }
}







