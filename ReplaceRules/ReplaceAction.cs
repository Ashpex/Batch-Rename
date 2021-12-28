using StringAction;
using StringArguments;
using System;

namespace ReplaceRules
{
    class ReplaceAction : IStringAction
    {
        public string name => "Replace action:";

        public StringProcessor Processor => _replace;

        public IStringArguments Args { get; set; }

        public IStringAction Clone()
        {
            return new ReplaceAction()
            {
                Args = new ReplaceActionArguments()            
            };
        }

        private string _replace(string origin,int index)
        {
            var myArgs = Args as ReplaceActionArguments;
            var needle = myArgs.Needle;
            var hammer = myArgs.Hammer;
            if(origin.IndexOf(needle) == -1)
            {
                return origin;
            }
            string res = origin.Replace(needle, hammer);

            return res;
        }
        public void ShowEditDialog()
        {
            var screen = new ReplaceActionDialog(Args as ReplaceActionArguments);

            if (screen.ShowDialog() == true)
            {
                var myArgs = Args as ReplaceActionArguments;
                myArgs.Needle = screen.Needle;
                myArgs.Hammer = screen.Hammer;
            }
        }
    }
}
