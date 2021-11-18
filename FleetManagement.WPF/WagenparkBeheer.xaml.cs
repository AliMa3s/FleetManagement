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
    /// Interaction logic for WagenparkBeheer.xaml
    /// </summary>
    public partial class WagenparkBeheer : Window
    {
        private readonly Managers _manager;

        public WagenparkBeheer(Managers unitOfManager)
        {
            InitializeComponent();
            _manager = unitOfManager;
        }
    }
}
