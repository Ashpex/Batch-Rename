using StringArguments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangExtensionRules
{
    public class ChangeExtensionArguments:IStringArguments, INotifyPropertyChanged
    {
        public string _needle;
        public string _hammer;

        public string Needle
        {
            get => _needle; set
            {
                _needle = value;
                NotifyChanged("Needle");
                NotifyChanged("Details");
            }
        }

        public string Hammer
        {
            get => _hammer; set
            {
                _hammer = value;
                NotifyChanged("Hammer");
                NotifyChanged("Details");
            }
        }

        public string Details => $"Change Extension {Needle} with {Hammer}";

        private void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void setValueChild(List<string> list)
        {
            Needle = list.ElementAt(0);
            Hammer = list.ElementAt(1);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
