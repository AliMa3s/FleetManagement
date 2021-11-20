using FleetManagement.Manager;
using FleetManagement.Model;
using FleetManagement.WPF.exceptions;
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

            //set dropdown Kleuren (DB)
            //VoertuigManager.Kleuren.ToList().ForEach(kleur =>
            //{
            //    _ = VoertuigKleur.Items.Add(kleur);
            //});

            //set dropdown Aantal Deuren (DB)
            //VoertuigManager.Brandstoffen.ToList().ForEach(kleur =>
            //{
            //    _ = Brandstof.Items.Add(kleur);
            //});
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

            try
            {
                Voertuig nieuwVoertuig = new(
                    GekozenAutoModel,
                    ChassisNummer.Text,
                    Nummerplaat.Text,
                    new BrandstofVoertuig(
                        Brandstof.SelectedItem.ToString(),
                        HybrideJa.IsChecked.HasValue && (bool)HybrideJa.IsChecked
                    )
                );

                //Voeg bestuurder toe indien ingevuld in het formulier
                if(GekozenBestuurder != null)
                {
                    nieuwVoertuig.VoegBestuurderToe(GekozenBestuurder);
                }

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

                //Voertuig voertuigDB = _managers.VoertuigManager.VoegVoertuigToe(nieuwVoertuig);

                ResetForm();

                InfoVoertuigMess.Foreground = Brushes.Green;
                InfoVoertuigMess.Text = $"Voertuig is succesvol aangemaakt";
            }
            catch (Exception ex)
            {
                InfoVoertuigMess.Foreground = Brushes.Red;
                InfoVoertuigMess.Text = ex.Message;
            }
        }

        //Voeg bestuurder toe uit een bestaande lijst
        private void KiesBestuurder_Click(object sender, RoutedEventArgs e)
        {
            SelecteerBestuurder selecteerBestuurder = new(_managers.BestuurderManager)
            {
                Owner = Window.GetWindow(this),
                Bestuurder = GekozenBestuurder
            };

            bool? geslecteerd = selecteerBestuurder.ShowDialog();
            if (geslecteerd == true)
            {
                GekozenBestuurder = selecteerBestuurder.Bestuurder;
                GekozenBestuurderNaam.Text = GekozenBestuurder.Achternaam + " " + GekozenBestuurder.Voornaam;
                KiesBestuurder.Content = "Bestuurder wijzigen";
            }
        }

        //Voeg AutoModel toe uit een bestaande lijst
        private void KiesAutoModel_Click(object sender, RoutedEventArgs e)
        {

        }

        //reset Formulier
        private void ResetForm()
        {
            InfoVoertuigMess.Text = string.Empty;
            GekozenAutoModelNaam.Text = string.Empty;
            ChassisNummer.Text = string.Empty;
            Nummerplaat.Text = string.Empty;
            HybrideJa.IsChecked = false;
            HybrideNeen.IsChecked = true;
            Brandstof.SelectedIndex = 0;
            VoertuigKleur.SelectedIndex = 0;
            Deuren.SelectedIndex = 0;
            GekozenBestuurderNaam.Text = string.Empty;
            KiesBestuurder.Content = "Bestuurder selecteren";

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