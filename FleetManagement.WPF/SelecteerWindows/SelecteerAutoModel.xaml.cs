﻿using System;
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

namespace FleetManagement.WPF.SelecteerWindows
{
    /// <summary>
    /// Interaction logic for SelecteerAutoModel.xaml
    /// </summary>
    public partial class SelecteerAutoModel : Window
    {
        public SelecteerAutoModel()
        {
            InitializeComponent();
        }
        //Automodel bewaren telkens een Bestuurder wordt geselecteerd
        private void BewaarAutoModel_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        }

        //Ga terug wanneer een automodel is gekozen
        private void ButtonKiesToevoegen_Click(object sender, RoutedEventArgs e) {
       
        }

        //Ga terug bij dubbelklik op een rij in de lijst 
        private void AutoModelToevoegenDoubleClick(object sender, MouseButtonEventArgs e) {
           
        }

        private void ButtonAnnuleer_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }

        //Filteren van naam
        private void TextBoxFilterTextChanged(object sender, TextChangedEventArgs e) {
        }
    }
}
