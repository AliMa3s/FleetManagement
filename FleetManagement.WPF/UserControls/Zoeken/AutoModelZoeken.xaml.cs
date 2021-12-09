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
    /// Interaction logic for AutoModelZoeken.xaml
    /// </summary>
    public partial class AutoModelZoeken : UserControl
    {
        private readonly Managers _managers;

        public string DisplayFirst { get; set; } = "Selecteer";

        public AutoModelZoeken(Managers managers)
        {
            InitializeComponent();
            _managers = managers;

            AutoTypesComboBox.Items.Add(DisplayFirst);
            _managers.AutoTypes.ToList().ForEach(autoType => {

                AutoTypesComboBox.Items.Add(autoType.Value);
            });
        }
    }
}
