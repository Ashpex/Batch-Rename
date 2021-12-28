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

namespace AddCounterToTheEnd
{
    /// <summary>
    /// Interaction logic for AddCounterDialog.xaml
    /// </summary>
    public partial class AddCounterDialog : Window
    {
        public string Start;
        public string Step;
        public string NumberOfDigits;
        public AddCounterDialog(AddCounterArgunments args)
        {
            InitializeComponent();
            startTextBox.Text = Start;
            stepTexBox.Text = Step;
            numberOfDigits.Text = NumberOfDigits;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if(startTextBox.Text == null || stepTexBox.Text == null || numberOfDigits.Text == null)
            {
                MessageBox.Show("You Have To Input Number!", "Error Detected in Input", MessageBoxButton.OK);
                return;
            }
            if (IsNumeric(startTextBox.Text) && IsNumeric(stepTexBox.Text) && IsNumeric(numberOfDigits.Text))
            {
                Start = startTextBox.Text;
                Step = stepTexBox.Text;
                NumberOfDigits = numberOfDigits.Text;

                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("You Have To Input Number!", "Error Detected in Input", MessageBoxButton.OK);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
    }
}
