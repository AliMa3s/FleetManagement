using FleetManagement.Manager;
using FleetManagement.Model;
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

namespace FleetManagement.WPF.UserControls.Toevoegen {
    /// <summary>
    /// Interaction logic for AutoModelToevoegen.xaml
    /// </summary>
    public partial class AutoModelToevoegen : UserControl {
        private readonly Managers _managers;

        public string DisplayFirst { get; set; } = "Selecteer";

        public AutoModelToevoegen(Managers managers) {
            InitializeComponent();
            _managers = managers;

            AutoTypesComboBox.Items.Add(DisplayFirst);
            _managers.AutoTypes.ToList().ForEach(autoType => {

                AutoTypesComboBox.Items.Add(autoType.Value);
            });
        }

        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e) {
            infoAutoModelMess.Text = string.Empty;
            ResetForm();
        }

        //reset Formulier
        private void ResetForm() {
            Merknaam.Text = string.Empty;
            AutoModelNaam.Text = string.Empty;
            AutoTypesComboBox.SelectedIndex = 0;
        }


        private void SluitAutoModelForm_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        private void AutoModelAanmakenButton_Click(object sender, RoutedEventArgs e) {
            //Wis bij elke nieuw poging de message info
            infoAutoModelMess.Text = string.Empty;
            try {

                string selectedModel = AutoTypesComboBox.SelectedItem.ToString();

                AutoModel nieuweAutoModel = new(
                    Merknaam.Text.Trim(),
                    AutoModelNaam.Text.Trim(),
                    new AutoType(selectedModel != DisplayFirst ? selectedModel : "")
                );
                _managers.AutoModelManager.VoegAutoModelToe(nieuweAutoModel);
                infoAutoModelMess.Foreground = Brushes.Green;
                infoAutoModelMess.Text = "AutoModel is succesvol aangemaakt.";
                ResetForm();
            } catch (Exception ex) {
                infoAutoModelMess.Foreground = Brushes.Red;
                infoAutoModelMess.Text = ex.Message;
            }
        }
    }
}
