using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;

namespace QL.Core
{
    public static class QLLog
    {
        public const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private static object log_lock_ = new object();

        static QLLog()
        {
            try
            {
                string path = Path.Combine(
                    System.AppDomain.CurrentDomain.BaseDirectory,
                    "log",
                    "log.txt"
                    );

                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                Trace.Listeners.Add(new TextWriterTraceListener(path));
            }
            catch
            {
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            }
            Trace.AutoFlush = true;
        }

        public static void Inf(string msg, params object[] args)
        {
            PrintMessage("INF", string.Format(msg, args));
        }

        public static void Wrn(string msg, params object[] args)
        {
            PrintMessage("WRN", string.Format(msg, args));
        }

        public static void Err(string msg, params object[] args)
        {
            PrintMessage("ERR", string.Format(msg, args));
        }

        private static void PrintMessage(string type, string msg)
        {
            string s = string.Format("{0} [{1}] {2}",
                DateTime.Now.ToString(DATETIME_FORMAT),
                type,
                msg);

            lock (log_lock_)
            {
                Trace.WriteLine(s);
            }
        }
    }
}
