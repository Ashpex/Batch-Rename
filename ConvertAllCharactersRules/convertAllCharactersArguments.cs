using StringArguments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllCharactersRules
{
    public class convertAllCharactersArguments : IStringArguments, INotifyPropertyChanged
    {
        public int _choice;
        public string _newConvertAllCharacters;
        public int Choice
        {
            get => _choice; set
            {
                _choice = value;
            }
        }

        public string NewConvertAllCharacters
        {
            get => _newConvertAllCharacters; set
            {
                _newConvertAllCharacters = value;
                NotifyChanged("NewConvertAllCharacters");
                NotifyChanged("Details");
            }
        }

        public string Details => $"Convert all character with {NewConvertAllCharacters}";

        public void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void setValueChild(List<string> list)
        {
            string token = list.ElementAt(0);
            if(token == "All Upper Case"){
                Choice = 1;
                NewConvertAllCharacters = "All Upper Case";
            }
            else if(token == "All Lower Case")
            {
                Choice = 2;
                NewConvertAllCharacters = "All Lower Case";
            }
            else if (token == "Remove All Space")
            {
                Choice = 3;
                NewConvertAllCharacters = "Remove All Space";
            }
            else if (token == "Remove All Space From The Beginning And Ending")
            {
                Choice = 4;
                NewConvertAllCharacters = "Remove All Space From The Beginning And Ending";
            }
            else if (token == "Convert To PascalCase")
            {
                Choice = 5;
                NewConvertAllCharacters = "Convert To PascalCase";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
