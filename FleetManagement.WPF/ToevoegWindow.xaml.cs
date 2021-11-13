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
using System.Windows.Shapes;

namespace FleetManagement.WPF
{
    /// <summary>
    /// Interaction logic for ToevoegWindow.xaml
    /// </summary>
    public partial class ToevoegWindow : Window
    {
        private readonly UnitOfManager _manager; 
            
        public ToevoegWindow(UnitOfManager unitOfManager)
        {
            InitializeComponent();
            Title = "FleetManagement Toevoegscherm";
            _manager = unitOfManager;
            authUser.Text = "Welkom " + _manager.LoggedIn.Naam;
        }
    }
}
