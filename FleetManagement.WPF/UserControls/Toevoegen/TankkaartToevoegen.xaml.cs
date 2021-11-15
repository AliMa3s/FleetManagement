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
    /// Interaction logic for TankkaartToevoegen.xaml
    /// </summary>
    public partial class TankkaartToevoegen : UserControl
    {
        private readonly UnitOfManager _manager;
        private readonly TankkaartOpbouw _tankkaart;

        public TankkaartToevoegen(UnitOfManager unitOfManager)
        {
            InitializeComponent();
            _manager = unitOfManager;
            FormTankkaart.Content = $"Nieuw tankkaart toevoegen (by {_manager.LoggedIn.Naam})";

            _tankkaart = new TankkaartOpbouw(_manager.TankkaartManager)
            {
                Bestuurder = null
            };
        }

        private void SluitTankkaartForm_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
