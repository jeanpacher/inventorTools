
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bosch_ImportData
{
    public static class Log
    {
        static string logPath { get; set; } = @"C:\VaultWorkFolderBosch\log.txt";

        public static void gravarLog(string msg)
        {
            File.AppendAllText(logPath, $"{DateTime.Now}: {msg} {System.Environment.NewLine}");
        }
    }
}
