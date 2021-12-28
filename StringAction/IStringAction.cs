using StringArguments;
using System;

namespace StringAction
{
    public delegate string StringProcessor(string origin, int index);
    public interface IStringAction
    {
        string name { get; }
        StringProcessor Processor { get; }
        IStringArguments Args { get; set; }
        IStringAction Clone();

        void ShowEditDialog();
    }
}
