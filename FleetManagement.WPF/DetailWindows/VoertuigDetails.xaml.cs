using FleetManagement.Manager;
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

        public VoertuigDetails(Managers managers, Voertuig voertuig) {

            InitializeComponent();
            _managers = managers;
            _voertuigDetail = voertuig;

            if (_voertuigDetail.HeeftVoertuigBestuurder)
            {
                StringBuilder stringBuilder = new("Naam: " + _voertuigDetail.Bestuurder.Achternaam);
                stringBuilder.Append(" " + _voertuigDetail.Bestuurder.Voornaam);
                stringBuilder.AppendLine(Environment.NewLine + "Rijksregister: " + _voertuigDetail.Bestuurder.RijksRegisterNummer);
                BestuurderDetail.Text = stringBuilder.ToString();
            }

            AutoModelGegevens.Text = _voertuigDetail.AutoModel.Merk + " "
                + _voertuigDetail.AutoModel.AutoModelNaam + " "
                + _voertuigDetail.AutoModel.AutoType.AutoTypeNaam;

            if(_voertuigDetail.AantalDeuren.HasValue)
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
                VoertuigDetail = _voertuigDetail
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
                //_managers.VoertuigManager.VerwijderVoertuig(_voertuigDetail);

                Window.GetWindow(this).Close();

                //Verwijder via manager; bij succes sluit scherm
                //+ update list en verwijder uit de lijst of vraag terug result aan manager
            }
        }
    }
}
