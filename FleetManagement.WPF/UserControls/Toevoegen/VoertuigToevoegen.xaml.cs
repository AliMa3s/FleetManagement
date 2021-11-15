using FleetManagement.Bouwers;
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

        public VoertuigToevoegen(UnitOfManager unitOfManager)
        {
            InitializeComponent();
            _manager = unitOfManager;
            FormVoertuig.Content = $"Nieuw voertuig aanmaken ( by {_manager.LoggedIn.Naam})";

            _manager.VoertuigBouwer = new VoertuigBouwer(_manager.VoertuigManager)
            {
                VoertuigKleur = null,
                AantalDeuren = null
            };
        }

        //Wis het formulier en begin opnieuw
        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e)
        {
            RestForm();
        }

        //Voertuig aanmaken
        private void VoertuigAanmakenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Voertuig nieuwVoertuig = _manager.VoertuigBouwer.BouwVoertuig();
                _manager.VoertuigManager.VoegVoertuigToe(nieuwVoertuig);
            }
            catch
            {
                infoVoertuigMess.Text = _manager.VoertuigBouwer.Status();
            }
        }

        //reset Formulier
        private void RestForm()
        {
            infoVoertuigMess.Text = string.Empty;
            _manager.VoertuigBouwer = new VoertuigBouwer(_manager.VoertuigManager);

            //Todo velden
            
        }

        //sluit vernster
        private void SluitVoertuigForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        
    }
}