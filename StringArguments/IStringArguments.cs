using System;
using System.Collections.Generic;

namespace StringArguments
{
    public interface IStringArguments
    {
        string Details { get; }

        void setValueChild(List<string> list);
    }
}
