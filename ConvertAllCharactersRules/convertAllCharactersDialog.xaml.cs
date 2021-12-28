using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ConvertAllCharactersRules
{
    /// <summary>
    /// Interaction logic for convertAllCharactersDialog.xaml
    /// </summary>
    public partial class convertAllCharactersDialog : Window
    {
        public string NewConvertAllCharactersName;
        public int choice;
        BindingList<convertAllCharactersArguments> _choices;
        public convertAllCharactersDialog(convertAllCharactersArguments args)
        {
            InitializeComponent();
            convertAllCharacterCombobox.SelectedIndex = args.Choice;
            convertAllCharacterCombobox.SelectedValue = args.NewConvertAllCharacters;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _choices = new BindingList<convertAllCharactersArguments>
            {
                new convertAllCharactersArguments() {_choice = 1, _newConvertAllCharacters = "All Upper Case"},
                new convertAllCharactersArguments() {_choice = 2, _newConvertAllCharacters = "All Lower Case"},
                new convertAllCharactersArguments() {_choice = 3, _newConvertAllCharacters = "Remove All Space"},
                new convertAllCharactersArguments() {_choice = 4, _newConvertAllCharacters = "Remove All Space From The Beginning And Ending"},
                new convertAllCharactersArguments() {_choice = 5, _newConvertAllCharacters = "Convert To PascalCase"},
            };

            convertAllCharacterCombobox.ItemsSource = _choices;

            convertAllCharacterCombobox.SelectedValuePath = "_newConvertAllCharacters";

            DataContext = this;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            choice = convertAllCharacterCombobox.SelectedIndex;
            if(choice == -1)
            {
                MessageBox.Show("You Have To Choose Method!", "Error Detected in Input", MessageBoxButton.OK);
                return;
            }
            NewConvertAllCharactersName = _choices[choice].NewConvertAllCharacters;
            MessageBox.Show(NewConvertAllCharactersName);
            this.DialogResult = true;
        }
    }
}
