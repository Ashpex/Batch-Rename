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

namespace AddingSuffixRules
{
    /// <summary>
    /// Interaction logic for AddSuffixDialog.xaml
    /// </summary>
    public partial class AddSuffixDialog : Window
    {
        public string StrToAdd;
        public AddSuffixDialog(AddSuffixArguments args)
        {
            InitializeComponent();
            strNeedToAdd.Text = args.StrAdd;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StrToAdd = strNeedToAdd.Text;
            }
            catch (Exception)
            {
                StrToAdd = "";
            }

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
