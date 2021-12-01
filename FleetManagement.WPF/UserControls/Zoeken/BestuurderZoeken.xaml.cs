﻿using FleetManagement.Manager;
using FleetManagement.Model;
using FleetManagement.WPF.DetailWindows;
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
    /// Interaction logic for BestuurderZoeken.xaml
    /// </summary>
    public partial class BestuurderZoeken : UserControl
    {
        private readonly Managers _managers;

        private Bestuurder _bestuurder;

        public Bestuurder Bestuurderweergave
        {
            get => _bestuurder;
            set
            {
                _bestuurder = value;
                BestuurderZoekWeergave.SelectedItem = value;
            }
        }

        public string PlaceholderName { get; } = "Achternaam + Voornaam";
        private string PlaceHolderRijksregister { get; } = "RijksregisterNummer";
        public BestuurderZoeken(Managers managers)
        {
            InitializeComponent();
            _managers = managers;

            FilterOpNaam.Text = "Achternaam + Voornaam";
            Rijksregister.Text = PlaceHolderRijksregister;
            BestuurderZoekWeergave.ItemsSource = _managers.BestuurderManager.FilterOpBestuurdersNaam("");
        }

        private void ZoekWeergave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bestuurderweergave = BestuurderZoekWeergave.SelectedItem as Bestuurder;
        }

        private void ZoekenMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BestuurderDetails detailWindow = new BestuurderDetails(_managers, _bestuurder)
            {
                Owner = Window.GetWindow(this),
            };

            //Uitgezet anders geen update status mogelijk
            //detailWindow.Show();

            bool? verwijderd = detailWindow.ShowDialog();
            if (verwijderd == true)
            {
                BestuurderZoekWeergave.ItemsSource = _managers.BestuurderManager.FilterOpBestuurdersNaam("");
            }
        }

        private void ZoekRijksregister_Click(object sender, RoutedEventArgs e)
        {
        }

        private void FilterOpNaam_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterOpNaam.Text != PlaceholderName)
                BestuurderZoekWeergave.ItemsSource = _managers.BestuurderManager.FilterOpBestuurdersNaam(FilterOpNaam.Text);
        }

        private void FilterOpNaam_GotFocus(object sender, RoutedEventArgs e)
        {
           if(FilterOpNaam.Text == PlaceholderName)
            {
                FilterOpNaam.Text = string.Empty;
                FilterOpNaam.Foreground = Brushes.Black;
            }
        }

        private void FilterOpNaam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilterOpNaam.Text))
            {
                FilterOpNaam.Text = PlaceholderName;
                FilterOpNaam.Foreground = Brushes.LightSlateGray;
            }
        }

        private void SluitWindow_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        private void KiesDetail_Click(object sender, RoutedEventArgs e)
        {
            if (Bestuurderweergave != null)
            {
                BestuurderDetails detailWindow = new BestuurderDetails(_managers, _bestuurder)
                {
                    Owner = Window.GetWindow(this),
                };

                //Uitgezet anders geen update status mogelijk, in de plaats hidden scherm
                //detailWindow.Show();

                bool? verwijderd = detailWindow.ShowDialog();
                if (verwijderd == true)
                {
                    BestuurderZoekWeergave.ItemsSource = _managers.BestuurderManager.FilterOpBestuurdersNaam("");
                }
            }
        }

        private void Rijksregister_GotFocus(object sender, RoutedEventArgs e)
        {
            if(Rijksregister.Text == PlaceHolderRijksregister)
            {
                Rijksregister.Text = string.Empty;
                Rijksregister.Foreground = Brushes.LightSlateGray;

            }
        }

        private void Rijksregister_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Rijksregister.Text))
            {
                Rijksregister.Text = PlaceHolderRijksregister;
                Rijksregister.Foreground = Brushes.DarkSlateGray;
            }
        }
    }
}
