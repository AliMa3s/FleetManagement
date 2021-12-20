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
using System.Windows.Shapes;

namespace FleetManagement.WPF.UpdateWindows
{
    /// <summary>
    /// Interaction logic for UpdateAutoModel.xaml
    /// </summary>
    public partial class UpdateAutoModel : Window
    {
        private readonly Managers _managers;
        private AutoModel _autoModel;

        public AutoModel AutoModel => _autoModel;

        public UpdateAutoModel(Managers managers, AutoModel autoModel)
        {
            InitializeComponent();
            _managers = managers;
            _autoModel = autoModel;

            _managers.AutoTypes.ToList().ForEach(autoType => {

                AutoTypesComboBox.Items.Add(autoType.Value);
            });

            SetDefault();
        }

        private void ResetFormulierButton_Click(object sender, RoutedEventArgs e) {
            ResetForm();
        }

        private void SetDefault()
        {
            foreach (var item in AutoTypesComboBox.Items)
            {
                if(item.ToString() == _autoModel.AutoType.AutoTypeNaam)
                {
                    AutoTypesComboBox.SelectedItem = item;
                }
            }

            DataContext = _autoModel;
        }

        //reset Formulier
        private void ResetForm() {

            SetDefault();
        }


        private void SluitAutoModelForm_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        private void AutoModelUpdateButton_Click(object sender, RoutedEventArgs e) {
            //Wis bij elke nieuw poging de message info
            infoAutoModelMess.Text = string.Empty;

            try {
                string selectedModel = AutoTypesComboBox.SelectedItem.ToString();

                AutoModel UpdateAutoModel = new(
                    _autoModel.AutoModelId,
                    Merknaam.Text.Trim(),
                    AutoModelNaam.Text.Trim(),
                    new AutoType(selectedModel)
                );

                if(!UpdateAutoModel.Equals(_autoModel))
                {
                    _managers.AutoModelManager.UpdateAutoModel(UpdateAutoModel);
                    _autoModel = UpdateAutoModel;

                    DialogResult = true;
                }
                else
                {
                    infoAutoModelMess.Foreground = Brushes.Green;
                    infoAutoModelMess.Text = "Is up to date: alles is hetzelfde gebleven";
                }

            } catch (Exception ex) {
                infoAutoModelMess.Foreground = Brushes.Red;
                infoAutoModelMess.Text = ex.Message;
            }
        }

        private void VerwijderButton_Click(object sender, RoutedEventArgs e)
        {
            infoAutoModelMess.Text = string.Empty;

            BevestigingWindow bevestigingWindow = new("Zeker dat je dit autotype wilt verwijderen?")
            {
                Owner = Window.GetWindow(this),
            };

            bool? verwijderen = bevestigingWindow.ShowDialog();
            if (verwijderen == true)
            {
                try
                {
                    _managers.AutoModelManager.VerwijderAutoModel(_autoModel);
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    infoAutoModelMess.Foreground = Brushes.Red;
                    infoAutoModelMess.Text = ex.Message;
                }
            }
        }
    }
}
