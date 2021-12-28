using StringArguments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCounterToTheEnd
{
    public class AddCounterArgunments: IStringArguments, INotifyPropertyChanged
    {
        public string _start;
        public string _step;
        public string _numberOfDigits;

        public string Start
        {
            get => _start; set
            {
                _start = value;
                NotifyChanged("Start");
                NotifyChanged("Details");
            }
        }

        public string Step
        {
            get => _step; set
            {
                _step = value;
                NotifyChanged("Step");
                NotifyChanged("Details");
            }
        }
        public string NumberOfDigits
        {
            get => _numberOfDigits; set
            {
                _numberOfDigits = value;
                NotifyChanged("NumberOfDigits");
                NotifyChanged("Details");
            }
        }

        public string Details => $"Add Counter To The End With Start: {Start}, Step: {Step}, NumberOfDigits: {NumberOfDigits}";

        private void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void setValueChild(List<string> list)
        {

            Start = list.ElementAt(0);
            Step = list.ElementAt(1);
            NumberOfDigits = list.ElementAt(2);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
