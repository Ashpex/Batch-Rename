using StringAction;
using StringArguments;
using System;

namespace AddCounterToTheEnd
{
    public class AddCouterAction : IStringAction
    {
        public string name => "Add counter action:";

        public StringProcessor Processor => _addCounter;

        private string _addCounter(string origin,int index)
        {
            var myArgs = Args as AddCounterArgunments;
            var start = myArgs.Start;
            var step = myArgs.Step;
            var numberOfDigits = Int32.Parse(myArgs.NumberOfDigits);
            var res = origin;

            int lastDot = origin.LastIndexOf(".");
            if (lastDot != -1)
            {
                res = res.Remove(lastDot);
                string extension = origin.Substring(lastDot + 1, origin.Length - 1 - lastDot);

                int tmp = numberOfDigits - index.ToString().Length;
                if (tmp < 0)
                {
                    return origin;
                }
                while (tmp > 0)
                {
                    res += "0";
                    tmp--;
                }
                res += Int32.Parse(start) + (index) * Int32.Parse(step);
                res += ".";
                res += extension;
            }
            else
            {
                int tmp = numberOfDigits - index.ToString().Length;
                if (tmp < 0)
                {
                    return origin;
                }
                while (tmp > 0)
                {
                    res += "0";
                    tmp--;
                }
                res += Int32.Parse(start) + (index) * Int32.Parse(step);
            }

            return res;
        }

        public IStringArguments Args { get ; set ; }

        public IStringAction Clone()
        {
            return new AddCouterAction()
            {
                Args = new AddCounterArgunments()
            };
        }

        public void ShowEditDialog()
        {
            var screen = new AddCounterDialog(Args as AddCounterArgunments);

            if (screen.ShowDialog() == true)
            {
                var myArgs = Args as AddCounterArgunments;
                myArgs.Start = screen.Start;
                myArgs.Step = screen.Step;
                myArgs.NumberOfDigits = screen.NumberOfDigits;
            }
        }
    }
}
