using FleetManagement.Bouwers;
using FleetManagement.Manager;
using FleetManagement.Manager.Interfaces;
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

namespace FleetManagement.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UnitOfManager _manager;
        private readonly VoertuigBouwer _voertuigBouwer;

        public MainWindow(UnitOfManager unitOfManager)
        {
            InitializeComponent();
            _manager = unitOfManager;
            autoTypes.ItemsSource = _manager.VoertuigManager.AutoTypes;

            _voertuigBouwer = _manager.VoertuigBouwer = new VoertuigBouwer(_manager.VoertuigManager)
            {
                VoertuigKleur = null,
                AantalDeuren = null
            };
        }

        private void AutoTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _voertuigBouwer.AutoModel = new AutoModel("BWM","5-Reeks", (AutoType)autoTypes.SelectedIndex);
        }
    }
}
