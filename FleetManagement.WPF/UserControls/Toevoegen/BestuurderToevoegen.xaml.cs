using FleetManagement.Bouwers;
using FleetManagement.Manager;
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
    /// Interaction logic for BestuurderToevoegen.xaml
    /// </summary>
    public partial class BestuurderToevoegen : UserControl
    {
        private readonly UnitOfManager _manager;
        private readonly BestuurderOpbouw _bestuurder; 

        public BestuurderToevoegen(UnitOfManager unitOfManager)
        {
            InitializeComponent();
            _manager = unitOfManager;

            FormBestuurder.Content = $"Bestuurder aanmaken (by {_manager.LoggedIn.Naam})";

            _bestuurder = new BestuurderOpbouw(_manager.BestuurderManager)
            {
               Adres = null,
               Voertuig = null,
               TankKaart = null,
            };
        }

        private void SluitBestuurderForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
