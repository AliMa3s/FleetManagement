﻿using FleetManagement.Manager;
using FleetManagement.Model;
using FleetManagement.WPF.UpdateWindows;
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

namespace FleetManagement.WPF.DetailWindows {
    /// <summary>
    /// Interaction logic for VoertuigDetails.xaml
    /// </summary>
    public partial class VoertuigDetails : Window {

        private readonly Managers _managers;
        private Voertuig _voertuigDetail;

        public bool? Updatetet { get; set; }

        public VoertuigDetails(Managers managers, Voertuig voertuig) {

            InitializeComponent();
            _managers = managers;
            _voertuigDetail = voertuig;

            if (_voertuigDetail.HeeftVoertuigBestuurder)
            {
                string rijkregnr = _voertuigDetail.Bestuurder.RijksRegisterNummer;
                
                StringBuilder stringBuilder = new("Naam: " + _voertuigDetail.Bestuurder.Achternaam);
                stringBuilder.Append(" " + _voertuigDetail.Bestuurder.Voornaam);
                stringBuilder.AppendLine(Environment.NewLine + "Rijksregister: " + rijkregnr.Substring(0, 2) + "."
                    + rijkregnr.Substring(2, 2) + "."
                    + rijkregnr.Substring(4, 2) + "-"
                    + rijkregnr.Substring(6, 3) + "."
                    + rijkregnr.Substring(9, 2));
                BestuurderDetail.Text = stringBuilder.ToString();
            }

            AutoModelGegevens.Text = _voertuigDetail.AutoModel.Merk + " "
                + _voertuigDetail.AutoModel.AutoModelNaam + " "
                + _voertuigDetail.AutoModel.AutoType.AutoTypeNaam;

            if (_voertuigDetail.AantalDeuren.HasValue)
            {
                AutoModelGegevens.Text += " (" + _voertuigDetail.AantalDeuren.Value + " deurs)";
            }

            //Bind voertuig
            DataContext = _voertuigDetail;
        }

        private void SluitForm_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        private void WijzigButton_Click(object sender, RoutedEventArgs e) {

            UpdateVoertuig updateVoertuig = new(_managers, _voertuigDetail)
            {
                Owner = Window.GetWindow(this),
            };

            bool? updatetet = updateVoertuig.ShowDialog();
            if (updatetet == true)
            {
                _voertuigDetail = updateVoertuig.VoertuigDetail;

                //Changer aanspreken indien nodig
            }
        }

        private void VerwijderButton_Click(object sender, RoutedEventArgs e)
        {
            BevestigingWindow bevestigingWindow = new()
            {
                Owner = Window.GetWindow(this),
            };

            bool? verwijderen = bevestigingWindow.ShowDialog();
            if (verwijderen == true)
            {
                try
                {
                    _managers.VoertuigManager.VerwijderVoertuig(_voertuigDetail);
                    Updatetet = true;
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    infoVoertuigMess.Foreground = Brushes.Red;
                    infoVoertuigMess.Text = ex.Message;
                }
            }
        }
    }
}
