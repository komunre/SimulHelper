using System;
using System.Collections.Generic;
using System.Text;

namespace SimulHelper
{
    public class CVars
    {
        public Dictionary<string, string> CVarsKV;
        public void Parse(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg[0] == '-' && arg[1] == '-')
                {
                    var sub = arg.Substring(2);
                    var name = "";
                    var valueStart = 0;
                    foreach (var ch in sub)
                    {
                        if (ch != '=')
                        {
                            name += ch;
                            valueStart++;
                            continue;
                        }
                        break;
                    }
                    var value = arg.Substring(valueStart + 2);
                    CVarsKV.Add(name, value);
                }
            }
        }
    }
}
