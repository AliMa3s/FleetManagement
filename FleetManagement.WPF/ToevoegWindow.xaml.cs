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
        public ToevoegWindow(Managers unitOfManager)
        {
            InitializeComponent();
            Title = "FleetManagement Toevoegscherm";
            Title = "Toevoegbeheer";

            VoertuigToevoegTab.Content = new VoertuigToevoegen(unitOfManager);
            BestuurderToevoegTab.Content = new BestuurderToevoegen(unitOfManager);
            TankkaartToevoegTab.Content = new TankkaartToevoegen(unitOfManager);
        }
    }
}
