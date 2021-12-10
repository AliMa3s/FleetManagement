﻿using FleetManagement.Model;
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

namespace FleetManagement.WPF.UpdateWindows
{
    /// <summary>
    /// Interaction logic for UpdateNieuwAdres.xaml
    /// </summary>
    public partial class UpdateAdres : Window
    {

        private Adres _adres;

        public Adres AdresGegevens
        {
            get => _adres;
            set
            {
                _adres = value;
            }
        }

        public UpdateAdres(Adres adres)
        {

            InitializeComponent();
            _adres = adres;

            DataContext = AdresGegevens;
        }

        private void AnnuleerForm_Click(object sender, RoutedEventArgs e)
        {
            AdresGegevens.Straat = "";
            AdresGegevens.Nr = "";
            AdresGegevens.Postcode = "";
            AdresGegevens.Gemeente = "";
            DialogResult = false;
        }

        private void InvoegEnUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(StraatTextBox.Text) || !string.IsNullOrWhiteSpace(NummerTextBox.Text)
                || !string.IsNullOrWhiteSpace(PostcodeTextBox.Text)
                || !string.IsNullOrWhiteSpace(GemeenteTextBox.Text))
            {
                AdresGegevens.Straat = StraatTextBox.Text;
                AdresGegevens.Nr = NummerTextBox.Text;
                AdresGegevens.Postcode = PostcodeTextBox.Text;
                AdresGegevens.Gemeente = GemeenteTextBox.Text;

                DialogResult = true;
            }
            else
            {
                AdresGegevens.Straat = "";
                AdresGegevens.Nr = "";
                AdresGegevens.Postcode = "";
                AdresGegevens.Gemeente = "";
                DialogResult = false;
            }
        }
    }
}
