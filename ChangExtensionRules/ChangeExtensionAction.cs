using StringAction;
using StringArguments;
using System;

namespace ChangExtensionRules
{
    public class ChangeExtensionAction : IStringAction
    {
        public string name => "Change extention action:";

        public StringProcessor Processor => _changeExtension;

        public IStringArguments Args { get ; set ; }

        public IStringAction Clone()
        {
            return new ChangeExtensionAction()
            {
                Args = new ChangeExtensionArguments()
            };
        }

        public void ShowEditDialog()
        {
            var screen = new ChangeExtensionActionDialog(Args as ChangeExtensionArguments);

            if (screen.ShowDialog() == true)
            {
                var myArgs = Args as ChangeExtensionArguments;
                myArgs.Needle = screen.Needle;
                myArgs.Hammer = screen.Hammer;
            }
        }

        private string _changeExtension(string origin,int index)
        {
            var res = origin;
            int length = origin.Length;
            var myArgs = Args as ChangeExtensionArguments;
            var needle = myArgs.Needle;
            var hammer = myArgs.Hammer;
            int lastDot = origin.LastIndexOf(".");
            if(lastDot == -1)
            {
                return origin;
            }
            string extension = origin.Substring(lastDot + 1, origin.Length - 1 - lastDot);
            if (extension == needle)
            {
                res = res.Remove(lastDot+1);
                res = res + hammer;
            }

            return res;
        }
    }
}
