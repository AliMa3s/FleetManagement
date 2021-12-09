using FleetManagement.Manager;
using FleetManagement.WPF.UserControls.Toevoegen;
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
    /// Interaction logic for ToevoegWindow.xaml
    /// </summary>
    public partial class ToevoegWindow : Window
    {          
        public ToevoegWindow(Managers managers)
        {
            InitializeComponent();
            Title = "FleetManagement Toevoegscherm";
            Title = "Toevoegbeheer";

            VoertuigToevoegTab.Content = new VoertuigToevoegen(managers);
            BestuurderToevoegTab.Content = new BestuurderToevoegen(managers);
            TankkaartToevoegTab.Content = new TankkaartToevoegen(managers);
            AutomodellenToevoegTab.Content = new AutoModelToevoegen(managers);
        }
    }
}
