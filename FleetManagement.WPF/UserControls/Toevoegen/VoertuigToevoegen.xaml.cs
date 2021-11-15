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
        private VoertuigBouwer _voertuig;

        public VoertuigToevoegen(UnitOfManager unitOfManager)
        {
            InitializeComponent();
            _manager = unitOfManager;
            FormVoertuig.Content = $"Nieuw voertuig aanmaken (by {_manager.LoggedIn.Naam})";

            _voertuig = new VoertuigBouwer(_manager.VoertuigManager)
            {
                Bestuurder = null
            };
        }

        //Wis het formulier en begin opnieuw
        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        //Voertuig aanmaken
        private void VoertuigAanmakenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Voertuig nieuwVoertuig = _voertuig.BouwVoertuig();
                int voertuigId = _manager.VoertuigManager.VoegVoertuigToe(nieuwVoertuig);
                //nieuwVoertuig.VoegIdToe(); nog aanmaken
            }
            catch
            {
                infoVoertuigMess.Text = _voertuig.Status();
            }
        }

        //reset Formulier
        private void ResetForm()
        {
            infoVoertuigMess.Text = string.Empty;
            _voertuig = new VoertuigBouwer(_manager.VoertuigManager)
            {
                Bestuurder = null
            };

            //Todo velden  
        }

        //sluit vernster
        private void SluitVoertuigForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        
    }
}