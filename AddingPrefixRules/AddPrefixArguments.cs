using StringArguments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddingPrefixRules
{
    public class AddPrefixArguments : IStringArguments, INotifyPropertyChanged
    {
        public string _strAdd;


        public string StrAdd
        {
            get => _strAdd; set
            {
                _strAdd = value;
                NotifyChanged("StrAdd");
                NotifyChanged("Details");
            }
        }



        public string Details => $"Add Prefix  with {StrAdd}";

        private void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void setValueChild(List<string> list)
        {
            StrAdd = list.ElementAt(0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
