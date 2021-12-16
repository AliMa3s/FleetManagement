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

        public Voertuig VerwijderdVoertuig { get; private set; }
        public TankKaart VerwijderdTankkaart { get; private set; }

        public bool? Updatetet { get; set; }

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

        private void SetDefault()
        {
            Geboortedag.Text = BestuurderDetail.GeboorteDatum.Substring(8, 2);
            Geboortemaand.Text = BestuurderDetail.GeboorteDatum.Substring(5, 2);
            Geboortejaar.Text = BestuurderDetail.GeboorteDatum.Substring(0, 4);

            if (BestuurderDetail.HeeftBestuurderVoertuig)
            {
                string nummerplaat = BestuurderDetail.Voertuig.NummerPlaat;

                StringBuilder stringBuilder = new(BestuurderDetail.Voertuig.VoertuigNaam);
                stringBuilder.AppendLine(Environment.NewLine + "Chassis: " + BestuurderDetail.Voertuig.ChassisNummer);
                stringBuilder.AppendLine("Nummerplaat: " + nummerplaat.Substring(0, 1) + "-"
                    + nummerplaat.Substring(1, 3) + "-"
                    + nummerplaat.Substring(4, 3));

                GekozenVoertuigText.Text = stringBuilder.ToString();
                VoegVoertuigToe.Visibility = Visibility.Hidden;
                VerwijderVoertuig.Visibility = Visibility.Visible;
            }
            else
            {
                GekozenVoertuigText.Text = "Geen voertuig";
                _gekozenVoertuig = null;
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
                VoegTankkaartToe.Visibility = Visibility.Hidden;
                VerwijderTankkaart.Visibility = Visibility.Visible;
            }
            else
            {
                GekozenTankkaartText.Text = "Geen tankkaart";
                _gekozenTankkaart = null;
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

        private void SluitForm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            infoBestuurderMess.Text = string.Empty;

            try
            {
                //stap 1: voertuig verwijderen (indien van toepassing)
                if (VerwijderdVoertuig != null)
                {
                    BestuurderDetail.VerwijderVoertuig(BestuurderDetail.Voertuig);
                }

                //stap 2: object aanmaken
                string geboortedatum = Geboortejaar.Text + "-" + Geboortemaand.Text + "-" + Geboortedag.Text;

                Bestuurder updatetBestuurder = new(
                    BestuurderDetail.BestuurderId,
                    VoornaamText.Text,
                    AchternaamText.Text,
                    geboortedatum,
                    RijbewijsText.Text,
                    RijksRegisterText.Text
                );

                if (_ingevoegdAdres != null)
                {
                    updatetBestuurder.Adres = _ingevoegdAdres;
                }
                else
                {
                    if (BestuurderDetail.Adres != null)
                    {
                        updatetBestuurder.Adres = BestuurderDetail.Adres;
                    }
                }

                //stap3: voertuig samenstellen met de huidige situatie
                if (_gekozenVoertuig != null)
                {
                    updatetBestuurder.VoegVoertuigToe(_gekozenVoertuig);
                }
                else
                {
                    if (BestuurderDetail.HeeftBestuurderVoertuig)
                    {
                        updatetBestuurder.VoegVoertuigToe(
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

                //Update bestuurder
                if (BestuurderDetail.RijksRegisterNummer == RijksRegisterText.Text)
                {
                    Bestuurder updatedBestuurder = _managers.BestuurderManager.UpdateBestuurder(updatetBestuurder);
                }
                else
                {
                    Bestuurder updatedBestuurder = _managers.BestuurderManager.UpdateBestuurder(updatetBestuurder, RijksRegisterText.Text);
                }

                //Tankkaart(en) updaten na succesvol update van bestuurder
                if (VerwijderdTankkaart != null) 
                { 
                    BestuurderDetail.VerwijderTankKaart(VerwijderdTankkaart);
                    _managers.TankkaartManager.UpdateTankKaart(VerwijderdTankkaart);
                }

                //Update tankkaart met de nieuwe indien gekozen
                if (_gekozenTankkaart != null)
                {
                    updatetBestuurder.VoegTankKaartToe(_gekozenTankkaart);
                    _managers.TankkaartManager.UpdateTankKaart(updatetBestuurder.Tankkaart);
                }
                else
                {
                    if (BestuurderDetail.HeeftBestuurderTankKaart)
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

                        _gekozenTankkaart = bestaandeTankkaart;
                        updatetBestuurder.VoegTankKaartToe(bestaandeTankkaart);
                    }
                }

                BestuurderDetail = updatetBestuurder;
                _gekozenTankkaart = null;
                _gekozenVoertuig = null;

                Updatetet = true;
                DialogResult = true;
            }
            catch (Exception ex)
            {
                //Rolback huidig voertuig (voor reset knop na fouten in update)
                if (VerwijderdVoertuig != null)
                {
                    BestuurderDetail.VoegVoertuigToe(VerwijderdVoertuig);
                }

                infoBestuurderMess.Foreground = Brushes.Red;
                infoBestuurderMess.Text = ex.Message;
            }
        }

        private void WijzigAdres_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            infoBestuurderMess.Text = string.Empty;

            //Wanneer adres bestaat
            if(_ingevoegdAdres != null) 
            {
                Adres bestaandAdres = new(
                    _ingevoegdAdres.Straat,
                    _ingevoegdAdres.Nr,
                    _ingevoegdAdres.Postcode,
                    _ingevoegdAdres.Gemeente
                );

                bestaandAdres.VoegIdToe(_ingevoegdAdres.AdresId);

                UpdateAdres UpdateAdres = new(bestaandAdres)
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
            }
            else
            {
                //Wanneer adres niet bestaat
                NieuwAdres nieuwAdres = new(_ingevoegdAdres)
                {
                    Owner = Window.GetWindow(this),
                };

                bool? geslecteerd = nieuwAdres.ShowDialog();
                if (geslecteerd == true)
                {
                    _ingevoegdAdres = nieuwAdres.AdresGegevens;

                    Adresgegevens.Text = _ingevoegdAdres.Straat
                        + " " + _ingevoegdAdres.Nr
                        + " " + _ingevoegdAdres.Postcode
                        + " " + _ingevoegdAdres.Gemeente;
                    WijzigAdres.Content = "Adres wijzigen";
                }
                else
                {
                    if (nieuwAdres.AdresGegevens == null)
                    {
                        _ingevoegdAdres = nieuwAdres.AdresGegevens;
                        WijzigAdres.Content = "Adres ingeven";
                        Adresgegevens.Text = string.Empty;
                    }
                    else
                    {
                        nieuwAdres.AdresGegevens = _ingevoegdAdres;
                    }
                }
            }
        }

        private void VoegVoertuigToe_Click(object sender, RoutedEventArgs e)
        {
            SelecteerVoertuig selecteerVoertuig = new(_managers.VoertuigManager)
            {
                Owner = Window.GetWindow(this),
                GekozenVoertuig = _gekozenVoertuig
            };

            bool? geslecteerd = selecteerVoertuig.ShowDialog();
            if ((bool)geslecteerd)
            {
                _gekozenVoertuig = selecteerVoertuig.GekozenVoertuig;
                
                string nummerplaat = _gekozenVoertuig.NummerPlaat;

                StringBuilder stringBuilder = new(_gekozenVoertuig.VoertuigNaam);
                stringBuilder.AppendLine(Environment.NewLine + "Chassis: " + _gekozenVoertuig.ChassisNummer);
                stringBuilder.AppendLine("Nummerplaat: " + nummerplaat.Substring(0, 1) + "-"
                    + nummerplaat.Substring(1, 3) + "-"
                    + nummerplaat.Substring(4, 3));

                GekozenVoertuigText.Text = stringBuilder.ToString();
                VoegVoertuigToe.Visibility = Visibility.Hidden;
                VerwijderVoertuig.Visibility = Visibility.Visible;
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
            if ((bool)geslecteerd)
            {
                _gekozenTankkaart = selecteerTankkaart.Tankkaart;

                StringBuilder stringBuilder = new("Nr: " + _gekozenTankkaart.TankKaartNummer);
                stringBuilder.AppendLine(Environment.NewLine + "Geldig tot: " + _gekozenTankkaart.GeldigheidsDatum.ToString("dd/MM/yyyy"));

                if (_gekozenTankkaart.Actief)
                {
                    stringBuilder.AppendLine("Tankkaart is actief");
                }
                else
                {
                    if (_gekozenTankkaart.IsGeldigheidsDatumVervallen) { stringBuilder.AppendLine("Tankkaart is vervallen"); }
                    else { stringBuilder.AppendLine("Tankkaart is geblokkeerd"); }
                }

                GekozenTankkaartText.Text = stringBuilder.ToString();

                VoegTankkaartToe.Visibility = Visibility.Hidden;
                VerwijderTankkaart.Visibility = Visibility.Visible;
            }
        }

        private void ResetForm()
        {
            VoornaamText.Text = BestuurderDetail.Voornaam;
            AchternaamText.Text = BestuurderDetail.Achternaam;
            RijksRegisterText.Text = BestuurderDetail.RijksRegisterNummer;
            RijbewijsText.Text = BestuurderDetail.TypeRijbewijs;
            VerwijderdVoertuig = null;
            VerwijderdTankkaart = null;
            SetDefault();
        }

        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void VerwijderVoertuig_Click(object sender, RoutedEventArgs e)
        {
            VoegVoertuigToe.Visibility = Visibility.Visible;
            VerwijderVoertuig.Visibility = Visibility.Hidden;
            _gekozenVoertuig = null;
            GekozenVoertuigText.Text = "Geen voertuig";
            VerwijderdVoertuig = BestuurderDetail.Voertuig;
        }

        private void VerwijderTankkaart_Click(object sender, RoutedEventArgs e)
        {
            VoegTankkaartToe.Visibility = Visibility.Visible;
            VerwijderTankkaart.Visibility = Visibility.Hidden;
            _gekozenTankkaart = null;
            GekozenTankkaartText.Text = "Geen tankkaart";
            VerwijderdTankkaart = BestuurderDetail.Tankkaart;
        }
    }    
}
