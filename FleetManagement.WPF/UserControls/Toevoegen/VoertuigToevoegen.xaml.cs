﻿using FleetManagement.Bouwers;
using FleetManagement.Manager;
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
    /// Interaction logic for VoertuigToevoegen.xaml
    /// </summary>
    public partial class VoertuigToevoegen : UserControl
    {
        private readonly UnitOfManager _manager;
        private VoertuigBouwer _bouwer;

        public string DisplayFirst { get; set; } = "Selecteer";
        private VoertuigBouwer VoertuigBouwerInstance => new(_manager.VoertuigManager) { Bestuurder = null };

        public VoertuigToevoegen(UnitOfManager unitOfManager)
        {
            InitializeComponent();

            _manager = unitOfManager;
            FormVoertuig.Content = $"Nieuw voertuig aanmaken (by {_manager.LoggedIn.Naam})";

            _bouwer = VoertuigBouwerInstance;
            SetDefault();
        }

        //Set Default waarde van het formulier
        private void SetDefault()
        {
            HybrideNeen.IsChecked = true;

            //set dropdown Aantal Deuren
            Deuren.Items.Add(DisplayFirst);
            _manager.VoertuigManager.AantalDeuren.ToList().ForEach(aantal =>
            {
                Deuren.Items.Add(aantal);
            });

            //set dropdown Aantal Deuren
            //ToDo

            //set dropdown Aantal Deuren
            //ToDo
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
                Voertuig nieuwVoertuig = _bouwer.BouwVoertuig();
                int voertuigId = _manager.VoertuigManager.VoegVoertuigToe(nieuwVoertuig);

                ResetForm();

                InfoVoertuigMess.Foreground = Brushes.Green;
                InfoVoertuigMess.Text = $"Voertuig met ID: {voertuigId} succesvol aangemaakt";
            }
            catch
            {
                InfoVoertuigMess.Foreground = Brushes.Red;
                InfoVoertuigMess.Text = _bouwer.Status();
            }
        }

        //events
        private void KiesBestuurder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChassisNummer_TextChanged(object sender, TextChangedEventArgs e)
        {
            _bouwer.Chassisnummer = ChassisNummer.Text;
        }

        private void Nummerplaat_TextChanged(object sender, TextChangedEventArgs e)
        {
            _bouwer.Nummerplaat = Nummerplaat.Text;
        }

        private void HybrideNeen_Checked(object sender, RoutedEventArgs e)
        {
            if(HybrideNeen.IsChecked.HasValue)
            {
                _bouwer.Hybride = false;
            }
        }

        private void HybrideJa_Checked(object sender, RoutedEventArgs e)
        {
            if (HybrideJa.IsChecked.HasValue)
            {
                _bouwer.Hybride = true;
            }
        }

        private void Brandstof_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void VoertuigKleur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Deuren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Deuren.SelectedItem != null)
            {
                string selected = Deuren.SelectedItem.ToString();
                _bouwer.AantalDeuren = selected != DisplayFirst ? selected : null;
            }
        }

        private void KiesAutoModel_Click(object sender, RoutedEventArgs e)
        {

        }

        //reset Formulier
        private void ResetForm()
        {
            InfoVoertuigMess.Text = string.Empty;
            GekozenAutoModel.Text = string.Empty;
            ChassisNummer.Text = string.Empty;
            Nummerplaat.Text = string.Empty;
            HybrideJa.IsChecked = false;
            HybrideNeen.IsChecked = true;
            Brandstof.SelectedIndex = 0;
            VoertuigKleur.SelectedIndex = 0;
            Deuren.SelectedIndex = 0;
            GekozenBestuurder.Text = string.Empty;

            _bouwer = null;
            _bouwer = VoertuigBouwerInstance;
        }

        //sluit vernster
        private void SluitVoertuigForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}