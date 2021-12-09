using FleetManagement.Manager;
using FleetManagement.WPF.UserControls.Zoeken;
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

namespace FleetManagement.WPF
{
    /// <summary>
    /// Interaction logic for ZoekWindow.xaml
    /// </summary>
    public partial class ZoekWindow : Window
    {
        private readonly Managers _managers; 
           
        public ZoekWindow(Managers managers)
        {
            InitializeComponent();
            Title = "FleetManagement Zoekscherm";
            _managers = managers;

            VoertuigZoekTab.Content = new VoertuigZoeken(managers);
            BestuurderZoekTab.Content = new BestuurderZoeken(managers);
            TankkaartZoekTab.Content = new TankkaartZoeken(managers);
            AutomodelZoekTab.Content = new AutoModelZoeken(managers);
        }
    }
}
