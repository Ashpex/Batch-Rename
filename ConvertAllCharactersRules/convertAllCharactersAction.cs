using StringAction;
using StringArguments;
using System;
using System.Text.RegularExpressions;

namespace ConvertAllCharactersRules
{
    public class convertAllCharactersAction : IStringAction
    {
        public string name => "Convert All Character:";

        public StringProcessor Processor => _covertAllCharacters;

        public IStringArguments Args { get; set; }

        public IStringAction Clone()
        {
            return new convertAllCharactersAction()
            {
                Args = new convertAllCharactersArguments()
            };
        }

        public void ShowEditDialog()
        {
            var screen = new convertAllCharactersDialog(Args as convertAllCharactersArguments);

            if (screen.ShowDialog() == true)
            {
                var myArgs = Args as convertAllCharactersArguments;
                myArgs.Choice = screen.choice;
                myArgs.NewConvertAllCharacters = screen.NewConvertAllCharactersName;
            }
        }
        private string _covertAllCharacters(string origin,int index)
        {
            var myArgs = Args as convertAllCharactersArguments;
            var choice = myArgs.Choice;
            string res = "";

            if (choice == 0)
            {
                res = origin.ToUpper();
            }
            else if (choice == 1)
            {
                res = origin.ToLower();
            }
            else if (choice == 2)
            {
                string[] tokens  = origin.Split(' ');
                foreach(string token in tokens)
                {
                    res += token;
                }
            }else if(choice == 3)
            {
                res = origin;
                int lastDot = origin.LastIndexOf(".");
                if (lastDot != -1)
                {
                    string extension = origin.Substring(lastDot + 1, origin.Length - 1 - lastDot);
                    res = res.Remove(lastDot);
                    res = res.Trim();
                    res += ".";
                    res += extension;
                }
                else
                {
                    res = origin.Trim();
                }
            }else if(choice == 4)
            {
                res = origin;
                int lastDot = origin.LastIndexOf(".");
                if (lastDot != -1)
                {
                    string extension = origin.Substring(lastDot + 1, origin.Length - 1 - lastDot);
                    res = res.Remove(lastDot);
                    Regex rx = new Regex("[a-zA-Z]+");
                    Regex rx1 = new Regex(@"\W|_");
                    res = rx.Replace(res, new MatchEvaluator(convertAllCharactersAction.CapText));
                    res = rx1.Replace(res, "");
                    res += ".";
                    res += extension;
                }
                else
                {
                    Regex rx = new Regex("[a-zA-Z]+");
                    Regex rx1 = new Regex(@"\W|_");
                    res = rx.Replace(res, new MatchEvaluator(convertAllCharactersAction.CapText));
                    res = rx1.Replace(res, "");
                }
                    
            }

            return res;
        }
        static string CapText(Match m)
        {
            // Get the matched string.
            string x = m.ToString();
            // If the first char is lower case...
            if (char.IsLower(x[0]))
            {
                // Capitalize it.
                return char.ToUpper(x[0]) + x.Substring(1, x.Length - 1);
            }
            return x;
        }
    }

}
