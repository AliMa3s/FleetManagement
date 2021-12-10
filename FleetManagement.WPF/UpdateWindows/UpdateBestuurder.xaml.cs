using FleetManagement.Manager;
using FleetManagement.Model;
using FleetManagement.WPF.NieuwWindows;
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
            SetDefault();
        }

        private void SluitForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            infoBestuurderMess.Text = string.Empty;

            try
            {
                string geboortedatum = Geboortejaar.Text + "-" + Geboortemaand.Text + "-" + Geboortedag.Text;

                Bestuurder nieuwBestuurder = new(
                    BestuurderDetail.BestuurderId,
                    VoornaamText.Text,
                    AchternaamText.Text,
                    geboortedatum,
                    RijbewijsText.Text,
                    RijksRegisterText.Text
                );

                if (_ingevoegdAdres != null)
                {
                    nieuwBestuurder.Adres = _ingevoegdAdres;
                }
                else
                {
                    if(BestuurderDetail.Adres != null)
                    {
                        nieuwBestuurder.Adres = BestuurderDetail.Adres;
                    }
                }

                //Update voertuig
                //Deze keuze is omdat we steeds weten wat de vorige toestand van het object is, zodat we terug kunnen resetten
                if (_gekozenVoertuig != null)
                {
                    nieuwBestuurder.VoegVoertuigToe(_gekozenVoertuig);
                }
                else
                {
                    if(BestuurderDetail.HeeftBestuurderVoertuig)
                    {
                        nieuwBestuurder.VoegVoertuigToe(
                            new(
                               BestuurderDetail.Voertuig.VoertuigId,
                               BestuurderDetail.Voertuig.AutoModel,
                               BestuurderDetail.Voertuig.ChassisNummer,
                               BestuurderDetail.Voertuig.NummerPlaat,
                               BestuurderDetail.Voertuig.Brandstof
                            )
                            {
                                AantalDeuren = BestuurderDetail.Voertuig.AantalDeuren,
                                VoertuigKleur = BestuurderDetail.Voertuig.VoertuigKleur,
                                InBoekDatum = BestuurderDetail.Voertuig.InBoekDatum
                            }
                        );
                    }
                }

                if(BestuurderDetail.RijksRegisterNummer == RijksRegisterText.Text)
                {
                    Bestuurder updatedBestuurder = _managers.BestuurderManager.UpdateBestuurder(nieuwBestuurder);
                }
                else
                {
                    Bestuurder updatedBestuurder = _managers.BestuurderManager.UpdateBestuurder(nieuwBestuurder, RijksRegisterText.Text);
                }

                if (_gekozenVoertuig != null)
                {
                    infoBestuurderMess.Text = "Bestuurder succesvol geüpdatet, Voertuig succesvol aan bestuurder gelinkt";
                }
                else
                {
                    infoBestuurderMess.Text = "Bestuurder succesvol geüpdatet";
                }

                infoBestuurderMess.Foreground = Brushes.Green;

                //Update tankkaart
                if (_gekozenTankkaart != null)
                {
                    //Verwijder tankaart zodat tankkaart geen bestuurder meer kent en update via manager
                    if (BestuurderDetail.HeeftBestuurderTankKaart)
                    {
                        TankKaart tankKaart = BestuurderDetail.Tankkaart;
                        BestuurderDetail.VerwijderTankKaart(tankKaart);
                        _managers.TankkaartManager.UpdateTankKaart(tankKaart);
                    }
                        
                    nieuwBestuurder.VoegTankKaartToe(_gekozenTankkaart);
                    _managers.TankkaartManager.UpdateTankKaart(nieuwBestuurder.Tankkaart);

                    infoBestuurderMess.Foreground = Brushes.Green;
                    infoBestuurderMess.Text += ", tankkaart aan bestuurder gelinkt";
                }
                else
                {
                    if(BestuurderDetail.HeeftBestuurderTankKaart)
                    {
                        TankKaart bestaandeTankkaart = new(
                            BestuurderDetail.Tankkaart.TankKaartNummer,
                            BestuurderDetail.Tankkaart.Actief,
                            BestuurderDetail.Tankkaart.GeldigheidsDatum,
                            BestuurderDetail.Tankkaart.Pincode
                        )
                        { 
                            UitgeefDatum = BestuurderDetail.Tankkaart.UitgeefDatum
                        };

                        nieuwBestuurder.VoegTankKaartToe(bestaandeTankkaart);
                    }
                }

                BestuurderDetail = nieuwBestuurder;
                _gekozenTankkaart = null;
                _gekozenVoertuig = null;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                infoBestuurderMess.Foreground = Brushes.Red;
                infoBestuurderMess.Text = ex.Message;
            }
        }

        private void WijzigAdres_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            infoBestuurderMess.Text = string.Empty;

            //Wanneer adres bestaat
            if(BestuurderDetail.Adres != null) 
            {
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
                    _ingevoegdAdres = UpdateAdres.AdresGegevens;
                    Adresgegevens.Text = _ingevoegdAdres.AdresId.ToString() ?? "";
                }
            }
            else
            {
                //Wanneer adres niet bestaat
                NieuwAdres UpdateAdres = new(_ingevoegdAdres)
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

        private void SetDefault()
        {
            Geboortedag.Text = BestuurderDetail.GeboorteDatum.Substring(8, 2);
            Geboortemaand.Text = BestuurderDetail.GeboorteDatum.Substring(5, 2);
            Geboortejaar.Text = BestuurderDetail.GeboorteDatum.Substring(0, 4);

            if (BestuurderDetail.HeeftBestuurderVoertuig)
            {
                StringBuilder stringBuilder = new(BestuurderDetail.Voertuig.AutoModel.Merk + " " + BestuurderDetail.Voertuig.AutoModel.AutoModelNaam);
                stringBuilder.AppendLine(Environment.NewLine + "Chassis: " + BestuurderDetail.Voertuig.ChassisNummer);
                stringBuilder.AppendLine("Nummerplaat: " + BestuurderDetail.Voertuig.NummerPlaat);

                GekozenVoertuigText.Text = stringBuilder.ToString();
                WijzigVoertuig.Content = "Voertuig wijzigen";
            }
            else
            {
                GekozenVoertuigText.Text = string.Empty;
                WijzigVoertuig.Content = "Voertuig invoegen";
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
            }
            else
            {
                GekozenTankkaartText.Text = string.Empty;
                WijzigTankkaart.Content = "Tankkaart invoegen";
            }

            if (BestuurderDetail.Adres != null)
            {
                StringBuilder stringBuilder = new(BestuurderDetail.Adres.Straat + " " + BestuurderDetail.Adres.Nr);
                stringBuilder.AppendLine(Environment.NewLine + BestuurderDetail.Adres.Postcode + " " + BestuurderDetail.Adres.Gemeente);

                Adresgegevens.Text = stringBuilder.ToString();
                WijzigAdres.Content = "Adres wijzigen";

                _ingevoegdAdres = BestuurderDetail.Adres;
            }
            else
            {
                Adresgegevens.Text = string.Empty;
                WijzigAdres.Content = "Adres ingeven";
            }

        }

        private void ResetForm()
        {
            VoornaamText.Text = BestuurderDetail.Voornaam;
            AchternaamText.Text = BestuurderDetail.Achternaam;
            RijksRegisterText.Text = BestuurderDetail.RijksRegisterNummer;
            RijbewijsText.Text = BestuurderDetail.TypeRijbewijs;
            SetDefault();
        }

        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }
    }    
}
