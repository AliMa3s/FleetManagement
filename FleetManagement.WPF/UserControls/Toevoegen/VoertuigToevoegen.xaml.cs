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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FleetManagement.WPF.UserControls.Toevoegen
{
    /// <summary>
    /// Interaction logic for VoertuigToevoegen.xaml
    /// </summary>
    public partial class VoertuigToevoegen : UserControl
    {
        private readonly Managers _managers;

        public string DisplayFirst { get; set; } = "Selecteer";

        //Bewaar het object dat geselecteerd is
        private AutoModel GekozenAutoModel { get; set; }
        private Bestuurder GekozenBestuurder { get; set; }

        public VoertuigToevoegen(Managers managers)
        {
            InitializeComponent();
            FormVoertuig.Content = "Nieuw voertuig aanmaken";
            _managers = managers;
            SetDefault();
        }

        //Set Default waarde van het formulier
        private void SetDefault()
        {
            HybrideNeen.IsChecked = true;

            _ = Brandstof.Items.Add(DisplayFirst);
            _ = VoertuigKleur.Items.Add(DisplayFirst);
            _ = Deuren.Items.Add(DisplayFirst);

            //set dropdown Aantal Deuren
            VoertuigManager.AantalDeuren.ToList().ForEach(aantal =>
            {
                _ = Deuren.Items.Add(aantal);
            });

            //set dropdown Kleuren(DB)
            _managers.Kleuren.ToList().ForEach(kleur =>
            {
                _ = VoertuigKleur.Items.Add(kleur.KleurNaam);
            });

            //set dropdown Brandstof(DB)
            _managers.Brandstoffen.ToList().ForEach(brandstof =>
            {
                _ = Brandstof.Items.Add(brandstof.BrandstofNaam);
            });
        }

        //Wis het formulier en begin opnieuw
        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        //Voertuig aanmaken
        private void VoertuigAanmakenButton_Click(object sender, RoutedEventArgs e)
        {
            //Wis bij elke nieuw poging de message info
            InfoVoertuigMess.Text = string.Empty;

            //Haal BrandstofType op (met ID) via manager
            BrandstofType brandstofType = _managers.Brandstoffen.ToList().Find(e => e.BrandstofNaam == Brandstof.SelectedItem.ToString());

            if (brandstofType != null)
            {
                try
                {
                    Voertuig nieuwVoertuig = new(
                        GekozenAutoModel,
                        ChassisNummer.Text,
                        Nummerplaat.Text,
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

                        nieuwVoertuig.AantalDeuren = Enum.IsDefined(typeof(AantalDeuren), selected)
                            ? (AantalDeuren)Enum.Parse(typeof(AantalDeuren), selected)
                            : throw new AantalDeurenException("Aantal deuren staat niet in de lijst");
                    }

                    //Indien ingevuld checken en casten
                    if (VoertuigKleur.SelectedItem.ToString() != DisplayFirst)
                    {
                        nieuwVoertuig.VoertuigKleur = new Kleur(VoertuigKleur.SelectedItem.ToString());
                    }

                    Voertuig voertuigDB =_managers.VoertuigManager.VoegVoertuigToe(nieuwVoertuig);

                    InfoVoertuigMess.Foreground = Brushes.Green;
                    InfoVoertuigMess.Text = "Voertuig is succesvol aangemaakt";

                    //Voeg bestuurder toe indien ingevuld in het formulier
                    //(Hier is dat heel belangrijk dat een Bestuurder ook het voertuig kent)
                    if (GekozenBestuurder != null)
                    {
                        voertuigDB.VoegBestuurderToe(GekozenBestuurder);
                        _managers.BestuurderManager.UpdateBestuurder(voertuigDB.Bestuurder);

                        InfoVoertuigMess.Foreground = Brushes.Green;
                        InfoVoertuigMess.Text = "Voertuig is succesvol aangemaakt en bestuurder is succesvol aan Voertuig gelinkt";
                    }

                    ResetForm();
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
                InfoVoertuigMess.Text = "BrandstofType staat niet in de lijst";
            }
        }

        //Voeg bestuurder toe uit een bestaande lijst
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

        //Voeg AutoModel toe uit een bestaande lijst
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
                KiesAutoModel.Content = "AutoModel wijzigen";
            }
        }

        //reset Formulier
        private void ResetForm()
        {
            GekozenAutoModelNaam.Text = string.Empty;
            GekozenBestuurderNaam.Text = string.Empty;
            ChassisNummer.Text = string.Empty;
            Nummerplaat.Text = string.Empty;
            HybrideJa.IsChecked = false;
            HybrideNeen.IsChecked = true;
            Brandstof.SelectedIndex = 0;
            VoertuigKleur.SelectedIndex = 0;
            Deuren.SelectedIndex = 0;
            KiesBestuurder.Visibility = Visibility.Visible;
            AnnuleerBestuurder.Visibility = Visibility.Hidden;

            GekozenAutoModel = null;
            GekozenBestuurder = null;
        }

        //sluit venster
        private void SluitVoertuigForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}