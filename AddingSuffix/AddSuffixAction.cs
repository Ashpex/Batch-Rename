using AddingSuffixRules;
using StringAction;
using StringArguments;
using System;

namespace AddingSuffix
{
    public class AddSuffixAction : IStringAction
    {
        public string name => "Add Suffix action:";

        public StringProcessor Processor => _addSuffix;

        public IStringArguments Args { get; set; }

        public IStringAction Clone()
        {
            return new AddSuffixAction()
            {
                Args = new AddSuffixArguments()
            };
        }

        private string _addSuffix(string origin, int index)
        {
            var myArgs = Args as AddSuffixArguments;
            var strAdd = myArgs.StrAdd;
            int lastDot = origin.LastIndexOf(".");
            string res = origin;
            if (lastDot != -1)
            {
                string extension = origin.Substring(lastDot + 1, origin.Length - 1 - lastDot);
                res = res.Remove(lastDot);
                res = res + strAdd;
                res += ".";
                res += extension;
                return res;
            }
            else
            {
                res += strAdd;
            }

            return res;
        }
        public void ShowEditDialog()
        {
            var screen = new AddSuffixDialog(Args as AddSuffixArguments);

            if (screen.ShowDialog() == true)
            {
                var myArgs = Args as AddSuffixArguments;
                myArgs.StrAdd = screen.StrToAdd;

            }
        }
    }
}
