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
    /// Interaction logic for BevestigingWindow.xaml
    /// </summary>
    public partial class BevestigingWindow : Window
    {
        public BevestigingWindow(string mss = "")
        {
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(mss))
                Bevestingmss.Content = mss;
        }

        private void JaVerwijder_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void VerwijderNiet_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
