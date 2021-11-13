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

namespace FleetManagement.WPF.TabScreens
{
    /// <summary>
    /// Interaction logic for Overzicht.xaml
    /// </summary>
    public partial class ToevoegOverzichtTab : UserControl
    {
        private readonly UnitOfManager _manager;

        public ToevoegOverzichtTab(UnitOfManager unitOfManager)
        {
            InitializeComponent();
            _manager = unitOfManager;
            AuthMess.Text = "Welkom = " + _manager.LoggedIn.Naam;
        }
    }
}
