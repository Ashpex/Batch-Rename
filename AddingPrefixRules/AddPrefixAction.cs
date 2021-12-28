using StringAction;
using StringArguments;
using System;

namespace AddingPrefixRules
{
    public class AddPrefixAction : IStringAction
    {
        public string name => "Add Prefix action:";

        public StringProcessor Processor => _addPrefix;

        public IStringArguments Args { get; set; }

        public IStringAction Clone()
        {
            return new AddPrefixAction()
            {
                Args = new AddPrefixArguments()
            };
        }

        private string _addPrefix(string origin, int index)
        {
            var myArgs = Args as AddPrefixArguments;
            var strAdd = myArgs.StrAdd;

            string res = strAdd + origin;


            return res;
        }
        public void ShowEditDialog()
        {
            var screen = new AddPrefixDialog(Args as AddPrefixArguments);

            if (screen.ShowDialog() == true)
            {
                var myArgs = Args as AddPrefixArguments;
                myArgs.StrAdd = screen.StrToAdd;

            }
        }
    }
}
