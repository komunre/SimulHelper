using System;
using System.Collections.Generic;
using System.Text;

namespace SimulHelper
{
    public class Logger
    {
        public static void Log(string msg)
        {
            Console.WriteLine("[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond + "]" + msg);
        }
    }
}
