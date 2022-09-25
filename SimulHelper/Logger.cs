using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimulHelper
{
    public static class Logger
    {
        private static FileStream _writer;

        public static void Init()
        {
            _writer = File.OpenWrite("latest.log");
        }

        public static void Log(string msg)
        {
            string final = "[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond + "]" + msg;
            Console.WriteLine(final);
            _writer.Write(Encoding.ASCII.GetBytes(final));
        }

        public static void Finish()
        {
            _writer.Flush();
            _writer.Close();
        }
    }
}
