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

namespace FleetManagement.WPF.UserControls.Zoeken
{
    /// <summary>
    /// Interaction logic for TankkaartZoeken.xaml
    /// </summary>
    public partial class TankkaartZoeken : UserControl
    {
        private readonly Managers _managers;

        public TankkaartZoeken(Managers managers)
        {
            InitializeComponent();
            _managers = managers;
        }

        private void ZoekWeergave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ZoekenMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
