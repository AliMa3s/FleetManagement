using FleetManagement.Manager;
using FleetManagement.Model;
using FleetManagement.WPF.Exceptions;
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
    /// Interaction logic for UpdateVoertuig.xaml
    /// </summary>
    public partial class UpdateVoertuig : Window
    {
        private readonly Managers _managers;
        private Voertuig _voertuig;

        public string DisplayFirst { get; set; } = "Selecteer";
        private AutoModel GekozenAutoModel { get; set; }
        private Bestuurder GekozenBestuurder { get; set; }
        public Bestuurder VerwijderdBestuurder { get; private set; }
        public bool? Updatetet { get; set; } = false;

        public Voertuig VoertuigDetail
        {
            get => _voertuig;
            set
            {
                _voertuig = value;
            }
        }

        public UpdateVoertuig(Managers managers, Voertuig voertuig)
        {
            InitializeComponent();
            _managers = managers;
            _voertuig = voertuig;

            SetDefault();
            DataContext = VoertuigDetail;
        }

        private void SetDefault()
        {
            Brandstof.Items.Add(DisplayFirst);
            VoertuigKleur.Items.Add(DisplayFirst);
            Deuren.Items.Add(DisplayFirst);

            //set dropdown Aantal Deuren
            VoertuigManager.AantalDeuren.ToList().ForEach(aantal =>
            {
                _ = Deuren.Items.Add(aantal);
            });

            if(VoertuigDetail.AantalDeuren.HasValue)
            {
                Deuren.SelectedItem = VoertuigDetail.AantalDeuren.Value;
            }

            //set dropdown Kleuren(DB)
            _managers.Kleuren.ToList().ForEach(kleur =>
            {
                _ = VoertuigKleur.Items.Add(kleur.KleurNaam);
            });

            if (VoertuigDetail.VoertuigKleur != null)
            {
                VoertuigKleur.SelectedItem = VoertuigDetail.VoertuigKleur.KleurNaam;
            }

            //set dropdown Brandstof(DB)
            _managers.Brandstoffen.ToList().ForEach(brandstof =>
            {
                _ = Brandstof.Items.Add(brandstof.BrandstofNaam);
            });

            Brandstof.SelectedItem = VoertuigDetail.Brandstof.BrandstofNaam;

            if(VoertuigDetail.HeeftVoertuigBestuurder)
            {
                StringBuilder stringBuilder = new("Naam: " + VoertuigDetail.Bestuurder.Achternaam);
                stringBuilder.Append(" " + VoertuigDetail.Bestuurder.Voornaam);
                stringBuilder.AppendLine(Environment.NewLine + "Rijksregister: " + VoertuigDetail.Bestuurder.RijksRegisterNummer.Substring(0, 2) + "."
                    + VoertuigDetail.Bestuurder.RijksRegisterNummer.Substring(2, 2) + "."
                    + VoertuigDetail.Bestuurder.RijksRegisterNummer.Substring(4, 2) + "-"
                    + VoertuigDetail.Bestuurder.RijksRegisterNummer.Substring(6, 3) + "."
                    + VoertuigDetail.Bestuurder.RijksRegisterNummer.Substring(9, 2));
                GekozenBestuurderNaam.Text = stringBuilder.ToString();

                KiesBestuurder.Visibility = Visibility.Hidden;
                AnnuleerBestuurder.Visibility = Visibility.Visible;
            }
            else
            {
                GekozenBestuurderNaam.Text = "Geen Bestuurder";
                KiesBestuurder.Visibility = Visibility.Visible;
                AnnuleerBestuurder.Visibility = Visibility.Hidden;
            }

            GekozenAutoModelNaam.Text = VoertuigDetail.AutoModel.Merk + " "
                + VoertuigDetail.AutoModel.AutoModelNaam + " "
                + VoertuigDetail.AutoModel.AutoType.AutoTypeNaam;

            KiesAutoModel.Content = "Automodel wijzigen";
        }

        private void UpdateVoertuigButton_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            InfoVoertuigMess.Text = string.Empty;

            //Haal BrandstofType op (met ID) via manager
            BrandstofType brandstofType = _managers.Brandstoffen.ToList().Find(e => e.BrandstofNaam == Brandstof.SelectedItem.ToString());

            if (brandstofType != null)
            {
                try
                {
                    Voertuig updateVoertuig = new(
                        VoertuigDetail.VoertuigId,
                        GekozenAutoModel ?? VoertuigDetail.AutoModel,
                        Chassisnummer.Text.Trim(),
                        Nummerplaat.Text.Trim(),
                        new BrandstofVoertuig(
                            brandstofType.BrandstofTypeId,
                            Brandstof.SelectedItem.ToString(),
                            HybrideJa.IsChecked.HasValue && (bool)HybrideJa.IsChecked
                        )
                    );

                    //Indien ingevuld checken en casten
                    if (Deuren.SelectedItem.ToString() != DisplayFirst)
                    {
                        string selected = Deuren.SelectedItem.ToString();

                        updateVoertuig.AantalDeuren = Enum.IsDefined(typeof(AantalDeuren), selected)
                            ? (AantalDeuren)Enum.Parse(typeof(AantalDeuren), selected)
                            : throw new AantalDeurenException("Aantal deuren staat niet in de lijst");
                    }
                    else
                    {
                        updateVoertuig.AantalDeuren = null;
                    }

                    //Indien ingevuld checken en casten
                    updateVoertuig.VoertuigKleur = VoertuigKleur.SelectedItem.ToString() != DisplayFirst 
                        ? (new(VoertuigKleur.SelectedItem.ToString())) : null;

                    //Maak onderscheid wanneer chassisnummer en/of nummerplaat wijzigt
                    _managers.VoertuigManager.UpdateVoertuig(
                        updateVoertuig,
                        VoertuigDetail.ChassisNummer == Chassisnummer.Text ? null : Chassisnummer.Text,
                        VoertuigDetail.NummerPlaat == Nummerplaat.Text ? null : Nummerplaat.Text
                    );
                    
                    if(VerwijderdBestuurder != null)
                    {
                        VoertuigDetail.VerwijderBestuurder(VoertuigDetail.Bestuurder);
                    }

                    //Voeg bestuurder toe indien ingevuld in het formulier
                    if (GekozenBestuurder != null)
                    {
                        updateVoertuig.VoegBestuurderToe(GekozenBestuurder);
                        _managers.BestuurderManager.UpdateBestuurder(updateVoertuig.Bestuurder);
                    }
                    else
                    {
                        if(VoertuigDetail.HeeftVoertuigBestuurder)
                        {
                            Bestuurder move = VoertuigDetail.Bestuurder;
                            VoertuigDetail.VerwijderBestuurder(move);
                            updateVoertuig.VoegBestuurderToe(move);
                            _managers.BestuurderManager.UpdateBestuurder(updateVoertuig.Bestuurder);
                        }
                        else if (VerwijderdBestuurder != null)
                        {
                            _managers.BestuurderManager.UpdateBestuurder(VerwijderdBestuurder);
                        }
                    }

                    VoertuigDetail = updateVoertuig;
                    Updatetet = true;
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    InfoVoertuigMess.Foreground = Brushes.Red;
                    InfoVoertuigMess.Text = ex.Message;
                }
            }
            else
            {
                InfoVoertuigMess.Foreground = Brushes.Red;
                InfoVoertuigMess.Text = "Brandstof is niet ingevuld";
            }
        }

        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e)
        {
            InfoVoertuigMess.Text = string.Empty;

            DataContext = null;
            DataContext = VoertuigDetail;
            VerwijderdBestuurder = null;
            GekozenAutoModel = null;
            GekozenBestuurder = null;
            SetDefault();

            if (VoertuigDetail.Brandstof.Hybride)
            {
                HybrideJa.IsChecked = true;
                HybrideNeen.IsChecked = false;
            }
            else
            {
                HybrideJa.IsChecked = false;
                HybrideNeen.IsChecked = true;
            }
        }

        private void SluitVoertuigForm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void KiesBestuurder_Click(object sender, RoutedEventArgs e)
        {
            SelecteerBestuurder selecteerBestuurder = new(_managers.BestuurderManager, "voertuig")
            {
                Owner = Window.GetWindow(this),
                Bestuurder = GekozenBestuurder
            };

            bool? geslecteerd = selecteerBestuurder.ShowDialog();
            if (geslecteerd == true)
            {
                //Wis bij elke nieuw poging de message info
                InfoVoertuigMess.Text = string.Empty;

                GekozenBestuurder = selecteerBestuurder.Bestuurder;

                StringBuilder stringBuilder = new("Naam: " + selecteerBestuurder.Bestuurder.Achternaam);
                stringBuilder.Append(" " + selecteerBestuurder.Bestuurder.Voornaam);
                stringBuilder.AppendLine(Environment.NewLine + "Rijksregister: " + selecteerBestuurder.Bestuurder.RijksRegisterNummer.Substring(0, 2) + "."
                    + selecteerBestuurder.Bestuurder.RijksRegisterNummer.Substring(2, 2) + "."
                    + selecteerBestuurder.Bestuurder.RijksRegisterNummer.Substring(4, 2) + "-"
                    + selecteerBestuurder.Bestuurder.RijksRegisterNummer.Substring(6, 3) + "."
                    + selecteerBestuurder.Bestuurder.RijksRegisterNummer.Substring(9, 2));
                GekozenBestuurderNaam.Text = stringBuilder.ToString();

                KiesBestuurder.Visibility = Visibility.Hidden;
                AnnuleerBestuurder.Visibility = Visibility.Visible;
            }
        }
        private void AnnuleerBestuurder_Click(object sender, RoutedEventArgs e)
        {
            GekozenBestuurder = null;
            GekozenBestuurderNaam.Text = "Geen bestuurder";
            KiesBestuurder.Visibility = Visibility.Visible;
            AnnuleerBestuurder.Visibility = Visibility.Hidden;
            VerwijderdBestuurder = VoertuigDetail.Bestuurder;
        }

        private void KiesAutoModel_Click(object sender, RoutedEventArgs e)
        {
            SelecteerAutoModel selecteerAutoModdel = new(_managers.AutoModelManager)
            {
                Owner = Window.GetWindow(this),
                AutoModel = GekozenAutoModel
            };

            bool? geslecteerd = selecteerAutoModdel.ShowDialog();
            if (geslecteerd == true)
            {
                //Wis bij elke nieuw poging de message info
                InfoVoertuigMess.Text = string.Empty;

                GekozenAutoModel = selecteerAutoModdel.AutoModel;
                GekozenAutoModelNaam.Text = GekozenAutoModel.Merk + " " + GekozenAutoModel.AutoModelNaam;
                KiesAutoModel.Content = "Automodel wijzigen";
            }
        }
    }
}
